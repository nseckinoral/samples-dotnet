using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Utility;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients.Company;
using XOMNI.SDK.Public.Clients.OmniPlay;
using XOMNI.SDK.Public.Clients.PII;
using XOMNI.SDK.Public.Models.OmniPlay;
using Windows.UI.Popups;
using XOMNI.SDK.Public.Models.PII;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    public sealed partial class MainPage : Page
    {
        const string isRegisteredKey = "isRegistered";
        static readonly string apiUserName = "StagingApiTestUser";
        static readonly string apiPassword = "StagingApiTestPass";
        static readonly string apiServiceUri = "http://test.apistaging.xomni.com";
        //XOMNI.SDK.Public.Models.ApiResponse<WishlistWithItems> latestWishlistItems;
        XOMNI.SDK.Public.Models.ApiResponse<OmniSession> omniSession;
        string deviceId;
        string deviceDescription;

        static bool isLoggedIn = false;
        DispatcherTimer pollingTimer;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            this.pollingTimer = new DispatcherTimer();
            this.pollingTimer.Tick += PollingTimer_TickAsync;
            this.pollingTimer.Interval = new TimeSpan(0, 0, 5);

            this.deviceId = GetHostName();
            this.deviceDescription = GetHostName();
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var isRegisteredLocalSetting = ApplicationData.Current.LocalSettings.Values[isRegisteredKey];
            if (isRegisteredLocalSetting == null)
            {
                using (var clientContext = CreateClientContext())
                {
                    var deviceClient = clientContext.Of<XOMNI.SDK.Public.Clients.Company.DeviceClient>();
                    var registeredDevice = (await deviceClient.PostAsync(new XOMNI.SDK.Public.Models.Company.Device()
                    {
                        DeviceId = deviceId,
                        Description = deviceDescription
                    })).Data;

                    ApplicationData.Current.LocalSettings.Values.Add(isRegisteredKey, registeredDevice);
                }
            }

            pollingTimer.Start();
        }

        private async void wishlist_btn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                using (var clientContext = CreateClientContext())
                {
                    var longitude = 11;
                    var latitude = 12;
                    var examplemetadata = "test";

                    clientContext.OmniSession = omniSession.Data;

                    var wishlistClient = clientContext.Of<WishlistClient>();

                    //TODO:NSO catch 404 exceptions.
                    var wishlistGuids = await wishlistClient.GetAsync();

                    var latestWishlist = wishlistGuids.Data.Last();
                    var latestWishlistItems = await wishlistClient.GetAsync(latestWishlist, longitude, true, false, false, XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, examplemetadata, examplemetadata, latitude);
                    WishlistItems.ItemsSource = latestWishlistItems.Data.WishlistItems;
                    Wishlistgel.Begin();
                    //var messageDialog = new MessageDialog(latestWishlistItems.Data.WishlistItems.First().Item.Name, "Name of the first item of your wishlist");
                    //await messageDialog.ShowAsync();
                }


            }
            catch
            {


            }
          
        }

        private void back_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Wishlistgit.Begin();
        }

        private async void login_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var generatedQR = await GenerateQRCodeAsync("http://192.168.2.209/MobileLoginPage/", GetHostName());
            await SetImageFromByteArray(generatedQR, QRImage);
            QR.Opacity = 100;
            QR.IsHitTestVisible = true;
            Qrgel.Begin();


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

                        

                        var result = await deviceClient.GetIncomingsAsync(deviceId);
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

        private void QR_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Qrgit.Begin();
            QR.IsHitTestVisible = false;
        }

        private void logout_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            login_btn.IsEnabled = true;
            logout_btn.IsEnabled = false;
            mycart_btn.IsEnabled = false;
            wishlist_btn.IsEnabled = false;
            isLoggedIn = false;

        }

        public async Task SetImageFromByteArray(byte[] data, Windows.UI.Xaml.Controls.Image image)
        {
            using (InMemoryRandomAccessStream randomStream =
                new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(randomStream))
                {
                    // Write the bytes to the stream
                    writer.WriteBytes(data);

                    // Store the bytes to the MemoryStream
                    await writer.StoreAsync();

                    // Not necessary, but do it anyway
                    await writer.FlushAsync();

                    // Detach from the Memory stream so we don't close it
                    writer.DetachStream();
                }

                randomStream.Seek(0);

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.SetSource(randomStream);

                image.Source = bitMapImage;
            }
        }

        public string GetHostName()
        {
            var hostNamesList = Windows.Networking.Connectivity.NetworkInformation
                .GetHostNames();

            foreach (var entry in hostNamesList)
            {
                if (entry.Type == Windows.Networking.HostNameType.DomainName)
                {
                    return entry.CanonicalName;
                }
            }

            return null;
        }

        private ClientContext CreateClientContext()
        {
            return new ClientContext(apiUserName, apiPassword, apiServiceUri);
        }

        private async Task<byte[]> GenerateQRCodeAsync(string qrUri, string deviceId)
        {
            // TODO: qrUri tepedeki readonly string variable lar gibi yazilacak
            using (var clientContext = CreateClientContext())
            {
                return await clientContext.Of<QRCodeClient>().GetAsync(8, qrUri + string.Format("?deviceId={0}", deviceId));
            }
        }
    }
}
