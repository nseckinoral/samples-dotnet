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
        public MainWindow()
        {
            InitializeComponent();
            btn_OmniDiscover.Click += btn_OmniDiscover_Click;
            btn_CreateDeviceStorages.Click += btn_CreateDeviceStorages_Click;
        }

        #region Omni-Discovery
        //See for reference: http://dev.xomni.com/v2-1/http-api/public-apis/company/device/fetching-a-list-of-devices-in-the-nearest-store-using-license
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

        #region Creating Device Storage Items
        //See for reference: http://dev.xomni.com/v2-1/http-api/public-apis/company/device-storage/creating-a-new-data-storage-item
        private async void btn_CreateDeviceStorages_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStorageKey1.Text) || string.IsNullOrEmpty(txtStorageValue1.Text) ||
                string.IsNullOrEmpty(txtStorageKey2.Text) || string.IsNullOrEmpty(txtStorageValue2.Text))
            {
                MessageBox.Show("Please enter all storage key and value inputs.");
            }
            else
            {
                XomniClient client = new XomniClient();
                string deviceId = ((Device)list_DevicesFound.SelectedItem).DeviceId;

                DeviceStorageItem item1 = new DeviceStorageItem
                {
                    DeviceId = deviceId,
                    Key = txtStorageKey1.Text,
                    Value = txtStorageValue1.Text
                };

                CreateDeviceStorageItemResponseObject response = await client.CreateDeviceStorageItemAsync(deviceId, item1);
                if (response.IsSuccess)
                {
                    MessageBox.Show("Device storage item 1 has successfully created.");

                    DeviceStorageItem item2 = new DeviceStorageItem
                    {
                        DeviceId = deviceId,
                        Key = txtStorageKey2.Text,
                        Value = txtStorageValue2.Text
                    };

                    response = await client.CreateDeviceStorageItemAsync(deviceId, item2);
                    if (response.IsSuccess)
                    {
                        MessageBox.Show("Device storage item 2 has successfully created.");
                    }
                    else
                    {
                        MessageBox.Show("An error occured while creating device storage item 2.");
                    }
                }
                else
                {
                    MessageBox.Show("An error occured while creating device storage item 1.");
                }
            }
        }
        #endregion
    }
}
