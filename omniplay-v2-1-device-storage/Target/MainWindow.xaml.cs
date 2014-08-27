using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Target
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentTargetDeviceId;

        public MainWindow()
        {
            InitializeComponent();
            btn_RegisterDevice.Click += btn_RegisterDevice_Click;
            btn_PollStorageItems.Click += btn_PollStorageItems_Click;
            timerPolling.Tick += timerPolling_Tick;
        }

        #region Device Storage Item Polling
        async void timerPolling_Tick(object sender, EventArgs e)
        {
            //See for reference : http://dev.xomni.com/v2-1/http-api/public-apis/company/device-storage/fetching-storage-data-of-an-device-by-device-id

            timerPolling.Stop();

            XomniClient client = new XomniClient();
            PollingResponseObject response = await client.GetDeviceStorageItemsAsync(currentTargetDeviceId, cbDeleteAfterFetching.IsChecked.Value);
            if (response.IsSuccess)
            {
                btn_PollStorageItems.Content = "Polling Stopped";

                StringBuilder builder = new StringBuilder();
                builder.AppendLine(string.Format("Total {0} device storage items polled.\n", response.Data.Count));

                for (int i = 0; i < response.Data.Count; i++)
                {
                    builder.AppendLine(string.Format("Key[{0}]={1}  -  Value[{0}]={2}", i + 1, response.Data[i].Key, response.Data[i].Value));
                }

                MessageBox.Show(builder.ToString());

                if (cbDeleteAfterFetching.IsChecked.Value)
                {
                    btn_PollStorageItems.Content = "Polling Started";
                    timerPolling.Start();
                }
            }
            else if(response.HttpStatusCode == HttpStatusCode.NotFound)
            {
                btn_PollStorageItems.Content = "Polling Started";
                timerPolling.Start();
            }
            else
            {
                btn_PollStorageItems.Content = "Polling Stopped";
                MessageBox.Show("An error occured while polling.");
            }
        }

        DispatcherTimer timerPolling = new DispatcherTimer();
        void btn_PollStorageItems_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentTargetDeviceId))
            {
                btn_PollStorageItems.Content = "Polling Started";
                timerPolling.Interval = TimeSpan.FromSeconds(1);
                timerPolling.Start();
            }
            else
            {
                MessageBox.Show("Please register a device first.");
            }
        }
        #endregion

        #region Device Registration
        async void btn_RegisterDevice_Click(object sender, RoutedEventArgs e)
        {
            //See for reference: http://dev.xomni.com/v2-1/http-api/public-apis/company/device/registering-a-device-to-a-specific-license

            XomniClient xomniClient = new XomniClient();
            Device reqOb = new Device()
            {
                Description = "This is a device created on " + DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString(),
                DeviceId = Guid.NewGuid().ToString()
            };
            RegisterDeviceResponseObject response = await xomniClient.RegisterDeviceAsync(reqOb);
            if (response.IsSuccess)
            {
                //Save the DeviceId to be used in Omni-Discovery Polling
                currentTargetDeviceId = reqOb.DeviceId;
                MessageBox.Show("Registration succeeded");
            }
            else
            {
                //Reset the DeviceId
                currentTargetDeviceId = "";
                MessageBox.Show("Registration failed.");
            }
        }
        #endregion

    }
}