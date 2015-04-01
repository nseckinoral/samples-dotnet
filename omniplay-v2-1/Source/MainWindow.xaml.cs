using Source.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Catalog;
using XOMNI.SDK.Public.Clients.OmniPlay;
using XOMNI.SDK.Public.Clients.PII;
using XOMNI.SDK.Public.Exceptions;
using XOMNI.SDK.Public.Models.PII;

namespace Source
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Sample item ids. We're using these hardcoded items to populate sample wishlist.
        private const int SampleItemId1 = 1;
        private const int SampleItemId2 = 2;
        private static readonly string ApiEndpointUri = ConfigurationManager.AppSettings["XOMNI:ApiEndpointUri"];
        private static readonly string ApiClientAccessLicenceName = ConfigurationManager.AppSettings["XOMNI:ApiClientAccessLicenceName"];
        private static readonly string ApiClientAccessLicencePass = ConfigurationManager.AppSettings["XOMNI:ApiClientAccessLicencePass"];

        public MainWindow()
        {
            InitializeComponent();
            btn_OmniDiscover.Click += btn_OmniDiscover_Click;
            btn_AnonymousPII.Click += btn_AnonymousPII_Click;
            btn_Move.Click += btn_Move_Click;
            btn_CreateAndPopulateWishlist.Click += Btn_CreateAndPopulateWishlist_Click;
        }

        #region Omni-Play
        //See for reference : http://dev.xomni.com/v3-0/http-api/public-apis/omniplay/device/subscribing-to-omniplay-device-queue
        async void btn_Move_Click(object sender, RoutedEventArgs e)
        {
            if (list_DevicesFound.SelectedItem != null)
            {
                //Getting Anonymous PII credentails
                //This will be the PII moving to another device

                string PIIPassword = txt_PIIPassword.Text;
                string PIIUser = (string)txt_PIIPassword.Tag;
                using(ClientContext clientContext = new ClientContext(ApiClientAccessLicenceName,ApiClientAccessLicencePass,ApiEndpointUri))
                {
                    try
                    {
                        var deviceClient = clientContext.Of<DeviceClient>();
                        var deviceId = ((XOMNI.SDK.Public.Models.Company.Device)list_DevicesFound.SelectedItem).DeviceId;
                        await deviceClient.SubscribeToDeviceAsync(deviceId);
                        MessageBox.Show("Session successfully queued for Omni-Play.");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Omni-Play failed.");
                    }
                }

            }
        }
        #endregion

        #region Creating An Anonymous PII

        async void btn_AnonymousPII_Click(object sender, RoutedEventArgs e)
        {
            //See for reference: http://dev.xomni.com/v3-0/http-api/public-apis/pii/anonymous/requesting-an-anonymous-pii

            using(ClientContext clientContext = new ClientContext(ApiClientAccessLicenceName,ApiClientAccessLicencePass,ApiEndpointUri))
            {
                var anonymousUser = new AnonymousUser(){
                UserName = "Sample User" + Guid.NewGuid().ToString(),
                Name = "SampleUser" + Guid.NewGuid().ToString()};
                var anonymousClient = clientContext.Of<AnonymousClient>();
                try
                {
                    var response = await anonymousClient.PostAsync(new AnonymousUserRequest() { Name = anonymousUser.Name, UserName = anonymousUser.UserName });
                    if (response != null)
                    {
                        txt_PIIPassword.Text = response.Data.Password;
                        txt_PIIPassword.Tag = response.Data.UserName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Requesting an anonymous PII failed.");
                }
            }
        }
        #endregion

        #region Omni-Discovery

        //See for reference: http://dev.xomni.com/v3-0/http-api/public-apis/company/device/fetching-a-list-of-devices-in-the-nearest-store-using-license
        async void btn_OmniDiscover_Click(object sender, RoutedEventArgs e)
        {
            //Getting a list of discoverable devices in the current store
            //The current store refers to the store where the client access license for current client is mapped in XOMNI.
            using(ClientContext clientContext = new ClientContext(ApiClientAccessLicenceName,ApiClientAccessLicencePass,ApiEndpointUri))
            {
                var deviceClient = clientContext.Of<XOMNI.SDK.Public.Clients.Company.DeviceClient>();
                try
                {
                    //Getting the first session in the queue
                    var response = await deviceClient.GetAsync();
                    list_DevicesFound.ItemsSource = response.Data;
                }
                catch(XOMNIPublicAPIException ex)
                {
                    if(ex.ApiExceptionResult.HttpStatusCode == HttpStatusCode.NotFound)
                    {
                        //No device found.
                        MessageBox.Show("No device found");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("An error occured while processing the request.");
                }
            }
        }
        #endregion

        #region Creating and Populating Wishlist

        private async void Btn_CreateAndPopulateWishlist_Click(object sender, RoutedEventArgs e)
        {

            if (txt_PIIPassword.Tag == null || string.IsNullOrEmpty(txt_PIIPassword.Tag.ToString()))
            {
                MessageBox.Show("Please create an anonymous PII first.");
            }
            else
            {
                string piiUser = txt_PIIPassword.Tag.ToString();
                string piiPassword = txt_PIIPassword.Text;
                var wishlist = new XOMNI.SDK.Public.Models.PII.Wishlist()
                {
                    IsPublic = true,
                    //Name of a wishlist.PII User can enter any name for this field.
                    //We put a random GUID for demo purposes. 
                    Name = Guid.NewGuid().ToString(),
                    LastSeenLocation = new XOMNI.SDK.Public.Models.Location() { Latitude=12,Longitude=12 }
                };
                using(ClientContext clientContext = new ClientContext(ApiClientAccessLicenceName,ApiClientAccessLicencePass,ApiEndpointUri))
                {
                    try
                    {
                        clientContext.PIIUser = new User() { UserName = piiUser, Password = piiPassword };
                        //Creating a wishlist
                        //See for reference : http://dev.xomni.com/v3-0/http-api/public-apis/pii/wishlist/creating-a-wish-list
                        var wishlistClient = clientContext.Of<WishlistClient>();
                        var wishlistResponse = await wishlistClient.PostWishlistAsync(wishlist);

                        //Fetching all items. We're going to use some of them to add in our wishlist
                        var itemClient = clientContext.Of<ItemClient>();
                        var itemResponse = await itemClient.SearchAsync(new XOMNI.SDK.Public.Models.Catalog.ItemSearchRequest() {Skip = 0, Take = 2, IncludeItemStaticProperties= true });

                        var firstSampleItem = new XOMNI.SDK.Public.Models.PII.WishlistItem()
                        {
                           ItemId = itemResponse.Data.SearchResult.Items.FirstOrDefault().Id
                        };
                        var secondSampleItem = new XOMNI.SDK.Public.Models.PII.WishlistItem()
                        {
                           ItemId = itemResponse.Data.SearchResult.Items.LastOrDefault().Id
                        };

                        //Adding sample items to wishlist created above.
                        //See for reference : http://dev.xomni.com/v3-0/http-api/public-apis/pii/wishlist-item/adding-an-item-to-a-wish-list
                        var wishlistItemClient = clientContext.Of<WishlistItemClient>();
                        var firstSampleItemResponse = await wishlistItemClient.PostAsync(wishlistResponse.Data.UniqueKey,firstSampleItem);
                        var secondSampleItemResponse = await wishlistItemClient.PostAsync(wishlistResponse.Data.UniqueKey,secondSampleItem);
                        MessageBox.Show(String.Format("Wishlist created and populated.\nWishlistName: {0}\nId of items in wishlist: {1},{2}", wishlistResponse.Data.Name, firstSampleItem.ItemId.ToString(), secondSampleItem.ItemId.ToString()));
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("An error occured while populating wishlist. Please try again.");
                    }

                }
            }
        }

        #endregion
    }
}
