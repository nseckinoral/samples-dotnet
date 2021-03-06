﻿using Bing.Maps;
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
            this.Map.Culture = "en-US";
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
        private int tappedItemId;
        private async void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Reset Scroll Bar Position
            scrollViewer.ChangeView(0, 0, null);

            //Check Item
            var sampleItem = new ApiResponse<SingleItemSearchResult<Item>>();
            var tappedGrid = sender as Grid;
            if(tappedGrid == FirstItem)
            {
                tappedItemId = AppSettings.ItemIds[0];
            }
            else
            {
                tappedItemId = AppSettings.ItemIds[1];
            }

            //Fetch Item Details
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    var itemClient = clientContext.Of<XOMNI.SDK.Public.Clients.Catalog.ItemClient>();
                    sampleItem = await itemClient.GetAsync(tappedItemId, true, true, true, AssetDetailType.IncludeOnlyDefault);
                    sampleItem.Data.Item.LongDescription = FixHtmlTags(sampleItem.Data.Item.LongDescription);

                    ItemDetailScreen.DataContext = sampleItem.Data.Item;
                }
                commonProgressRing.IsActive = false;
                ItemDetailGrid.Opacity = 100;

                //Check InStock Status of InStockItemId  
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
            var storeList = new List<GroupedInStoreMetadataContainer>();
            try
            {
                using (ClientContext clientContext = new XOMNI.SDK.Public.ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    var myLocation = new XOMNI.SDK.Public.Models.Location { Longitude = -75.952134, Latitude = 40.801112 };
                    // Fetching the In-Store Metadata CompanyWide 
                    var metadataList = await clientContext.Of<ItemInStoreMetadataClient>().GetAsync(tappedItemId,keyPrefix: "instock",companyWide: true,location: myLocation,searchDistance: 1);
                    var metadataListData = metadataList.Data;
                    //Picking "instock" stores only
                    storeList = metadataListData.Where(a => a.Metadata.Any(b => b.Key == "instock" && b.Value == "true")).ToList();
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
