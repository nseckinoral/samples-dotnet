using Source.Models;
using System;
using System.Net;
using System.Windows;

namespace Source
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Sample item ids. We're using these hardcoded items to populate sample wishlist.
        private const int SampleItemId1 = 56;
        private const int SampleItemId2 = 30;

        public MainWindow()
        {
            InitializeComponent();
            btn_OmniDiscover.Click += btn_OmniDiscover_Click;
            btn_AnonymousPII.Click += btn_AnonymousPII_Click;
            btn_Move.Click += btn_Move_Click;
            btn_CreateAndPopulateWishlist.Click += Btn_CreateAndPopulateWishlist_Click;
        }

        #region Omni-Play
        //See for reference : http://dev.xomni.com/v2-0/http-api/public-apis/omniplay/device/subscribing-to-omniplay-device-queue
        async void btn_Move_Click(object sender, RoutedEventArgs e)
        {
            if (list_DevicesFound.SelectedItem != null)
            {
                //Getting Anonymous PII credentails
                //This will be the PII moving to another device

                string PIIPassword = txt_PIIPassword.Text;
                string PIIUser = (string)txt_PIIPassword.Tag;
                XomniClient xomniClient = new XomniClient();
                bool isSuccess = await xomniClient.SubscribeQueueAsync(PIIUser, PIIPassword, ((Device)list_DevicesFound.SelectedItem).DeviceId);
                if (isSuccess)
                {
                    MessageBox.Show("Session successfull queued for Omni-Play.");
                }
                else
                {
                    MessageBox.Show("Omni-Play failed.");
                }
            }
        }
        #endregion

        #region Creating An Anoymous PII

        async void btn_AnonymousPII_Click(object sender, RoutedEventArgs e)
        {
            //See for reference: http://dev.xomni.com/v2-0/http-api/public-apis/pii/anonymous/requesting-an-anonymous-pii
            PII_RequestObject reqOb = new PII_RequestObject()
            {
                UserName = "Sample User" + Guid.NewGuid().ToString(),
                Name = "SampleUser" + Guid.NewGuid().ToString()
            };
            XomniClient xomniClient = new XomniClient();
            PII_ResponseObject anonoymousPii = await xomniClient.RequestAnonymousPiiAsync(reqOb);
            if (anonoymousPii != null)
            {
                txt_PIIPassword.Text = anonoymousPii.Data.Password;
                txt_PIIPassword.Tag = anonoymousPii.Data.UserName;
            }
        }
        #endregion

        #region Omni-Discovery

        //See for reference: http://dev.xomni.com/v2-0/http-api/public-apis/omniplay/device/fetching-a-list-of-devices-in-the-nearest-store-using-location
        async void btn_OmniDiscover_Click(object sender, RoutedEventArgs e)
        {
            //Getting a list of discoverable devices in the current store
            //The current store refers to the store where the client access license for current client is mapped in XOMNI.
            XomniClient xomniClient = new XomniClient();
            DiscoveryResponseObject pollingResult = await xomniClient.GetDiscoverableDevicesAsync();
            if (pollingResult.IsSuccess)
            {
                //Getting the first session in the queue
                if (pollingResult.Data != null)
                {
                    list_DevicesFound.ItemsSource = pollingResult.Data;
                }
                else
                {
                    MessageBox.Show("No device found");
                }
            }
            else if (pollingResult.HttpStatusCode == HttpStatusCode.NotFound)
            {
                //No device found.
                MessageBox.Show("No device found");
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
                XomniClient client = new XomniClient();
                Wishlist wishlist = new Wishlist()
                {
                    IsPublic = true,
                    //Name of a wishlist.PII User can enter any name for this field.
                    //We put a random GUID for demo purposes. 
                    Name = Guid.NewGuid().ToString(),
                    LastSeenLocation = new Location(12, 12)
                };

                //Creating a wishlist
                //See for reference : http://dev.xomni.com/v2-0/http-api/public-apis/pii/wishlist/creating-a-wish-list
                CreateWishlistResponseObject response = await client.CreateWishlistAsync(wishlist, piiUser, piiPassword);
                if (response.IsSuccess)
                {
                    WishlistItem item1 = new WishlistItem()
                    {
                        ItemId = 56
                    };
                    //Adding sample items to wishlist created above.
                    //See for reference : http://dev.xomni.com/v2-0/http-api/public-apis/pii/wishlist-item/adding-an-item-to-a-wish-list
                    AddWishlistItemResponse itemResponse = await client.AddWishlistItemAsync(item1, response.Data.UniqueKey.ToString(), piiUser, piiPassword);
                    if (itemResponse.IsSuccess)
                    {
                        WishlistItem item2 = new WishlistItem()
                        {
                            ItemId = 30
                        };
                        itemResponse = await client.AddWishlistItemAsync(item2, response.Data.UniqueKey.ToString(), piiUser, piiPassword);
                        if (itemResponse.IsSuccess)
                        {
                            MessageBox.Show(String.Format("Wishlist created and populated.\nWishlistName:{0}\nId of items in wishlist:{1},{2}", response.Data.Name, SampleItemId1.ToString(), SampleItemId2.ToString()));
                        }
                        else
                        {
                            MessageBox.Show("An error occured while populating wishlist. Please try again");
                        }
                    }
                    else
                    {
                        MessageBox.Show("An error occured while populating wishlist. Please try again");
                    }
                }
            }
        }

        #endregion
    }
}
