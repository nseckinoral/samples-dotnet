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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    public sealed partial class MainPage : Page
    {
        const string isRegisteredKey = "isRegistered";
        static readonly string apiUserName = "{ApiUserName}";
        static readonly string apiPassword = "{ApiUserPass}";
        static readonly string apiServiceUri = "{ApiURI}";
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

            this.deviceId = Helpers.DeviceIdentity.GetASHWID();
            this.deviceDescription = Helpers.DeviceIdentity.GetFriendlyName();
            
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var isRegisteredLocalSetting = ApplicationData.Current.LocalSettings.Values[isRegisteredKey];
            if (isRegisteredLocalSetting == null)
            {
                using (var clientContext = CreateClientContext())
                {
                    var deviceClient = clientContext.Of<XOMNI.SDK.Public.Clients.Company.DeviceClient>();
                    try
                    {
                        var registeredDevice = (await deviceClient.PostAsync(new XOMNI.SDK.Public.Models.Company.Device()
                        {
                            DeviceId = deviceId,
                            Description = deviceDescription
                        })).Data;
                    }
                    catch (XOMNI.SDK.Public.Exceptions.XOMNIPublicAPIException ex)
                    {
                        if(ex.ApiExceptionResult.HttpStatusCode !=  System.Net.HttpStatusCode.Conflict)
                        {
                            throw ex;
                        }
                    }

                    ApplicationData.Current.LocalSettings.Values.Add(isRegisteredKey, true);
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
                    var latestWishlistItems = await wishlistClient.GetAsync(latestWishlist, longitude, latitude, true, false, false, XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, XOMNI.SDK.Public.Models.Catalog.AssetDetailType.None, examplemetadata, examplemetadata);
                    WishlistItems.ItemsSource = latestWishlistItems.Data.WishlistItems;
                }
            }
            catch
            {


            }
        }

        private async void login_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var generatedQR = await GenerateQRCodeAsync("http://192.168.2.209/MobileLoginPage/", deviceDescription);
            await SetImageFromByteArray(generatedQR, QRImage);
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
