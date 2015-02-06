using System;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Utility;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients.PII;
using XOMNI.SDK.Public.Models.OmniPlay;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KioskApp
{
    public sealed partial class MainPage : Page
    {
        XOMNI.SDK.Public.Models.ApiResponse<OmniSession> omniSession;
        static bool isLoggedIn = false;
        DispatcherTimer pollingTimer;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            this.pollingTimer = new DispatcherTimer();
            this.pollingTimer.Tick += PollingTimer_TickAsync;
            this.pollingTimer.Interval = new TimeSpan(0, 0, 5);

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Check if the app is already setup with required config data.
            var ApiURI = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey];
            if(ApiURI == null)
            {
                AppSettingsFlyout settingsFlyout = new AppSettingsFlyout();
                settingsFlyout.ShowIndependent();
            }
            else
            {
                pollingTimer.Start();
            }
        }

        private async void wishlist_btn_Click(object sender, RoutedEventArgs e)
        {
            WishlistItems.ItemsSource = null;
            wishlist_btn.IsHitTestVisible = false;

            try
            {
                using (var clientContext = CreateClientContext())
                {
                    var longitude = 11;
                    var latitude = 12;
                    var examplemetadata = "test";

                    clientContext.OmniSession = omniSession.Data;

                    var wishlistClient = clientContext.Of<WishlistClient>();
                    var wishlistGuids = await wishlistClient.GetAsync();

                    var latestWishlist = wishlistGuids.Data.Last();
                    var latestWishlistItems = await wishlistClient.GetAsync(latestWishlist, longitude, latitude, true, false, false, 
                        XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, 
                        XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, 
                        XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, 
                        examplemetadata, 
                        examplemetadata);
                    WishlistItems.ItemsSource = latestWishlistItems.Data.WishlistItems;
                    WishlistProgressRing.IsActive = false;
                }
            }
            catch(Exception ex)
            {
                WishlistItems.ItemsSource = null;
                WishlistProgressRing.IsActive = true;
                MessageDialog messageBox = new MessageDialog(ex.Message, "An error occured.");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    Wishlist_outgoing.Begin();
                    wishlist_btn.IsHitTestVisible = true;
                }));
                messageBox.ShowAsync();

            }
        }

        private async void login_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var generatedQR = await GenerateQRCodeAsync(ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.deviceIdConfigKey].ToString());
                await SetImageFromByteArray(generatedQR, QRImage);
                QRProgressRing.IsActive = false;

            }
            catch(Exception ex)
            {
                QRImage.Source = null;
                QRProgressRing.IsActive = true;

                MessageDialog messageBox = new MessageDialog(ex.Message, "An error occured.");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    Qr_Out.Begin();
                    QR.IsHitTestVisible = false;
                }));
                messageBox.ShowAsync();
            }
        }

        async void PollingTimer_TickAsync(object sender, object e)
        {
            if (isLoggedIn == false)
            {
                try
                {
                    using (var clientContext = CreateClientContext())
                    {
                        var deviceClient = clientContext.Of<XOMNI.SDK.Public.Clients.OmniPlay.DeviceClient>();
                        var result = await deviceClient.GetIncomingsAsync(ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.deviceIdConfigKey].ToString());
                        if (result.Data != null && result.Data.Any())
                        {
                            login_btn.IsEnabled = false;
                            logout_btn.IsEnabled = true;
                            mycart_btn.IsEnabled = true;
                            wishlist_btn.IsEnabled = true;
                            isLoggedIn = true;

                            var latestOmniTicket = result.Data.Last();
                            var omniTicketString = latestOmniTicket.OmniTicket.Substring(1, latestOmniTicket.OmniTicket.Length - 1);

                            var omniTicketClient = clientContext.Of<XOMNI.SDK.Public.Clients.OmniPlay.OmniTicketClient>();
                            omniSession = await omniTicketClient.PostSessionAsync(new OmniTicket { Ticket = Guid.Parse(omniTicketString) });
                        }
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void logout_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            isLoggedIn = false;
        }

        public async Task SetImageFromByteArray(byte[] data, Windows.UI.Xaml.Controls.Image image)
        {
            using (InMemoryRandomAccessStream randomStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(randomStream))
                {
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                    await writer.FlushAsync();
                    writer.DetachStream();
                }

                randomStream.Seek(0);

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.SetSource(randomStream);

                image.Source = bitMapImage;
            }
        }

        private ClientContext CreateClientContext()
        {
            return new ClientContext(
                ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey].ToString(),
                ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey].ToString(),  
                ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey].ToString()
                );
        }

        private async Task<byte[]> GenerateQRCodeAsync(string deviceId)
        {
            using (var clientContext = CreateClientContext())
            {
                return await clientContext.Of<QRCodeClient>().GetAsync(8, string.Format(ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.loginUrlConfigKey] + "?deviceId={0}", deviceId));
            }
        }



    }
}
