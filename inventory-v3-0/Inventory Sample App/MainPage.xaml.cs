using Bing.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Data.Html;
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
                DisablingBlackScreen.IsHitTestVisible = true;
                DisablingBlackScreen.Opacity = 0.6;
                AppSettingsFlyout settingsFlyout = new AppSettingsFlyout();
                settingsFlyout.ShowIndependent();
            }
        }

        private async void InStockItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Reset Scroll Bar Position
            scrollViewer.ChangeView(0,0,null);
       
            //Fetch Item Details
            var sampleInStockItem = new ApiResponse<SingleItemSearchResult<Item>>();
            var sampleOutOfStockItem = new ApiResponse<SingleItemSearchResult<Item>>();
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    var itemClient = clientContext.Of<XOMNI.SDK.Public.Clients.Catalog.ItemClient>();
                    sampleInStockItem = await itemClient.GetAsync(Int32.Parse(AppSettings.InStockItemId), true, true, true, AssetDetailType.IncludeOnlyDefault);
                    sampleInStockItem.Data.Item.LongDescription = FixHtmlTags(sampleInStockItem.Data.Item.LongDescription);

                    ItemDetailScreen.DataContext = sampleInStockItem.Data.Item;
                }
                commonProgressRing.IsActive = false;
                ItemDetailGrid.Opacity = 100;

                //Check InStock Status of InStockItemId  
                var isInStock = sampleInStockItem.Data.Item.InStoreMetadata.Any(x => x.Key == "instock" && x.Value == "true");
                if (!sampleInStockItem.Data.Item.InStoreMetadata.Any() || !isInStock)
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

        private async void OutOfStockItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Reset Scroll Bar Position
            scrollViewer.ChangeView(0, 0, null);

            //Fetch Item Details
            var sampleOutOfStockItem = new ApiResponse<SingleItemSearchResult<Item>>();
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    var itemClient = clientContext.Of<XOMNI.SDK.Public.Clients.Catalog.ItemClient>();
                    sampleOutOfStockItem = await itemClient.GetAsync(Int32.Parse(AppSettings.OutOfStockItemId), true, true, true, AssetDetailType.IncludeOnlyDefault);
                    sampleOutOfStockItem.Data.Item.LongDescription = FixHtmlTags(sampleOutOfStockItem.Data.Item.LongDescription);

                    ItemDetailScreen.DataContext = sampleOutOfStockItem.Data.Item;
                }
                commonProgressRing.IsActive = false;
                ItemDetailGrid.Opacity = 100;

                //Check InStock Status of InStockItemId  
                var isInStock = sampleOutOfStockItem.Data.Item.InStoreMetadata.Any(x => x.Key == "instock" && x.Value == "true");
                if (!sampleOutOfStockItem.Data.Item.InStoreMetadata.Any() || !isInStock)
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

        //Replaces all double "\r\n"s with a single one and removes the first one in the beginning
        private string FixHtmlTags(string sourceString)
        {
            var rawText = HtmlUtilities.ConvertToText(sourceString);
            var croppedText = rawText.Replace("\r\n\r\n", "\r\n");           
            int index = croppedText.IndexOf("\r\n");
            string cleanPath = (index < 0)
                ? croppedText
                : croppedText.Remove(index, "\r\n".Length);
            return cleanPath;
        }
        private async void btnOutOfStock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Map.Children.Clear();
            var storeList = new List<Store>();
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    // Fetching the In-Store Metadata CompanyWide 
                    var metadataList = await clientContext.Of<ItemInStoreMetadataClient>().GetAsync(Int32.Parse(AppSettings.OutOfStockItemId),0,100,keyPrefix: "instock",companyWide: true);
                    
                    //Picking "instock" stores only
                    var inStockStoreMetadataList = new List<InStoreMetadata>();
                    inStockStoreMetadataList = metadataList.Data.Where(x => x.Key == "instock" && x.Value == "true").ToList();

                    //Searching Stores With GPS Location
                    var nearStoreList = await clientContext.Of<StoreClient>().GetAsync(new XOMNI.SDK.Public.Models.Location() { Longitude = -75.952134, Latitude = 40.801112}, 1, 0, 10);

                    //Matching Store Ids of InStock items

                    storeList = nearStoreList.Data.Results.Where(a => inStockStoreMetadataList.Any(b => b.StoreId == a.Id)).ToList();
                }
                StoreList.ItemsSource = storeList;
                commonProgressRing.IsActive = false;
                StoreListContentGrid.Opacity = 100;
                StoreListContentGrid.IsHitTestVisible = true;
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
