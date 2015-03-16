using System;
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
using XOMNI.SDK.Public.Clients.OmniPlay;
using XOMNI.SDK.Public.Models.Catalog;

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
            if (string.IsNullOrEmpty(AppSettings.ApiUri))
            {
                AppSettingsFlyout settingsFlyout = new AppSettingsFlyout();
                settingsFlyout.ShowIndependent();
            }
            if(AppSettings.IsRegistered == "true")
            {
                EnableMainPage();
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
                    int sampleLongitude = 11;
                    int sampleLatitude = 12;
                    string sampleMetadata = "test";

                    clientContext.OmniSession = omniSession.Data;

                    WishlistClient wishlistClient = clientContext.Of<WishlistClient>();
                    var wishlistGuids = await wishlistClient.GetAsync();

                    Guid latestWishlistUniqueKey = wishlistGuids.Data.Last();
                    var latestWishlistItems = await wishlistClient.GetAsync(latestWishlistUniqueKey, sampleLongitude, sampleLatitude, true, false, false,
                        AssetDetailType.IncludeOnlyDefault,
                        AssetDetailType.None,
                        AssetDetailType.None,
                        sampleMetadata,
                        sampleMetadata);
                    WishlistItems.ItemsSource = latestWishlistItems.Data.WishlistItems;
                    WishlistProgressRing.IsActive = false;
                }
            }
            catch (Exception ex)
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
                var generatedQR = await GenerateQRCodeAsync(AppSettings.DeviceId);
                await SetImageFromByteArray(generatedQR, QRImage);
                QRProgressRing.IsActive = false;
            }
            catch (Exception ex)
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
                        var deviceClient = clientContext.Of<DeviceClient>();
                        var result = await deviceClient.GetIncomingsAsync(AppSettings.DeviceId);
                        if (result.Data != null && result.Data.Any())
                        {
                            login_btn.IsEnabled = false;
                            logout_btn.IsEnabled = true;
                            mycart_btn.IsEnabled = true;
                            wishlist_btn.IsEnabled = true;
                            isLoggedIn = true;

                            OmniTicketDetail latestOmniTicket = result.Data.Last();
                            string omniTicket = latestOmniTicket.OmniTicket.Substring(1, latestOmniTicket.OmniTicket.Length - 1);

                            var omniTicketClient = clientContext.Of<OmniTicketClient>();
                            omniSession = await omniTicketClient.PostSessionAsync(new OmniTicket { Ticket = Guid.Parse(omniTicket) });
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
            return new ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri);
        }

        private async Task<byte[]> GenerateQRCodeAsync(string deviceId)
        {
            using (var clientContext = CreateClientContext())
            {
                string loginUrl = string.Format(AppSettings.LoginUrl + "?deviceId={0}", deviceId);
                return await clientContext.Of<QRCodeClient>().GetAsync(8, loginUrl);
            }
        }

        public void EnableMainPage()
        {
            this.mainPageDisabled.Opacity = 0;
            this.mainPageDisabled.IsHitTestVisible = false;
        }

    }
}
