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
            btn_AnonymousPII.Click += btn_AnonymousPII_Click;
            btn_Move.Click += btn_Move_Click;
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
                bool isSuccess = await xomniClient.SubscribeQueue(PIIUser, PIIPassword, ((Device) list_DevicesFound.SelectedItem).DeviceId);
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
            PII_ResponseObject anonoymousPii = await xomniClient.RequestAnonymousPii(reqOb);
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
            DiscoveryResponseObject pollingResult = await xomniClient.GetDiscoverableDevices();
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
    }
}
