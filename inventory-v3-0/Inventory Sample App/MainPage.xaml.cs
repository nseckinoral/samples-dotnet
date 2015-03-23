using Bing.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Catalog;
using XOMNI.SDK.Public.Clients.Company;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;
using XOMNI.SDK.Public.Models.Company;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Inventory_Sample_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {       
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Check if the app is already setup with required config data.
            if (string.IsNullOrEmpty(AppSettings.ApiUri))
            {
                AppSettingsFlyout settingsFlyout = new AppSettingsFlyout();
                settingsFlyout.ShowIndependent();
            }
        }



        private async void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {           
            //Prepare Screen
            ItemDetail_In.Begin();
            ItemDetailScreen.IsHitTestVisible = true;

            //Fetch Item Details
            var sampleItem = new ApiResponse<SingleItemSearchResult<Item>>();
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    var itemClient = clientContext.Of<XOMNI.SDK.Public.Clients.Catalog.ItemClient>();
                    sampleItem = await itemClient.GetAsync(Int32.Parse(AppSettings.ItemId), true, true, true, AssetDetailType.IncludeOnlyDefault);
                    ItemDetailScreen.DataContext = sampleItem.Data.Item;
                }
                commonProgressRing.IsActive = false;
                ItemDetailGrid.Opacity = 100;

                //Check InStock Status  
                var isInStock = sampleItem.Data.Item.InStoreMetadata.Any(x => x.Key == "instock" && x.Value == "true");
                if (!sampleItem.Data.Item.InStoreMetadata.Any() || !isInStock)
                {
                    txtInStock.Visibility = Visibility.Collapsed;
                    btnInStock.Visibility = Visibility.Collapsed;
                    btnOutOfStock.Visibility = Visibility.Visible;
                }
                else if (isInStock)
                {
                    txtInStock.Visibility = Visibility.Visible;
                    btnInStock.Visibility = Visibility.Visible;
                    btnOutOfStock.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
               var messageBox = new MessageDialog(ex.Message, "An error occurred");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    ItemDetailError_Out.Begin();
                    commonProgressRing.IsActive = false;
                    ItemDetailScreen.IsHitTestVisible = false;
                }));
                messageBox.ShowAsync();
            }



            
        }


        private async void btnOutOfStock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var storeList = new List<Store>();
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    // Fetching the In-Store Metadata CompanyWide 
                    var metadataList = await clientContext.Of<ItemInStoreMetadataClient>().GetAsync(Int32.Parse(AppSettings.ItemId),0,100,keyPrefix: "instock",companyWide: true);
                    
                    //Picking "instock" stores only
                    var inStockStoreMetadataList = new List<InStoreMetadata>();
                    inStockStoreMetadataList = metadataList.Data.Where(x => x.Key == "instock" && x.Value == "true").ToList();

                    //Searching Stores With GPS Location
                    var nearStoreList = await clientContext.Of<StoreClient>().GetAsync(new XOMNI.SDK.Public.Models.Location() { Longitude = -75.952134, Latitude = 40.801112}, 1, 0, 10);

                    //Matching Store Ids of InStock items
                    foreach (var firstData in inStockStoreMetadataList)
                    {
                        foreach (var secondData in nearStoreList.Data.Results)
                        {
                            if (firstData.StoreId == secondData.Id)
                            {
                                storeList.Add(secondData);
                            }
                        }
                    }
                }
                StoreList.ItemsSource = storeList;
                commonProgressRing.IsActive = false;
                StoreListContentGrid.Opacity = 100;
            }
            catch (Exception ex)
            {
                commonProgressRing.IsActive = false;
                var messageBox = new MessageDialog(ex.Message, "An error occurred");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    StoreListError_Out.Begin();
                }));
                messageBox.ShowAsync();
            }

            //Preparing the map for fake location
            Bing.Maps.Location myFakeLocation = new Bing.Maps.Location(40.801112, -75.952134);
            Map.SetView(myFakeLocation, 14.0f);
            var myPin = new Pushpin();
            myPin.Background = new SolidColorBrush(Colors.Red);
            MapLayer.SetPosition(myPin, myFakeLocation);
            Map.Children.Add(myPin);

            //Preparing pushpins for stores
            try
            {
                for (int i = 0; i < storeList.Count; i++)
                {
                    var pin = new Pushpin();
                    pin.Text = (i + 1).ToString();
                    MapLayer.SetPosition(pin, new Bing.Maps.Location((double)storeList[i].Location.Latitude, (double)storeList[i].Location.Longitude));
                    Map.Children.Add(pin);
                }
            }
            catch
            {

            }

        }

        private void AnimationCompleted(object sender, object e)
        {
            commonProgressRing.IsActive = true;
        }

    }
}
