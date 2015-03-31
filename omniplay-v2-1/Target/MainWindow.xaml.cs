using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Target.Models;
using System.Configuration;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Company;

namespace Target
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentTargetDeviceId;
        string incommingOmniTicket;
        private static readonly string ApiEndpointUri = ConfigurationManager.AppSettings["XOMNI:ApiEndpointUri"];
        private static readonly string ApiClientAccessLicenceName = ConfigurationManager.AppSettings["XOMNI:ApiClientAccessLicenceName"];
        private static readonly string ApiClientAccessLicencePass = ConfigurationManager.AppSettings["XOMNI:ApiClientAccessLicencePass"];

        public MainWindow()
        {
            InitializeComponent();
            btn_RegisterDevice.Click += btn_RegisterDevice_Click;
            btn_PollSession.Click += btn_PollSession_Click;
            timerPolling.Tick += timerPolling_Tick;
        }

        #region Incomming Session Polling

        async void timerPolling_Tick(object sender, EventArgs e)
        {
            timerPolling.Stop();

            //Check for incomming sessions.
            //See for reference: http://dev.xomni.com/v2-1/http-api/public-apis/omniplay/device/fetching-pii-user-omnitickets-on-omniplay-device-queue

            XomniClient xomniClient = new XomniClient();
            PollingResponseObject pollingResult = await xomniClient.GetIncomingDevicesAsync(currentTargetDeviceId);
            if (pollingResult.IsSuccess)
            {
                //Getting the first session in the queue
                if (pollingResult.Data != null)
                {
                    incommingOmniTicket = pollingResult.Data.FirstOrDefault().OmniTicket;
                    //Removing the "P" prefix from the ticket which means it's a PII related OmniTicket
                    incommingOmniTicket = incommingOmniTicket.Substring(1, incommingOmniTicket.Length - 1);
                    btn_PollSession.Content = "Polling stopped.";
                    //Polling needs to be started again in order to receive 
                    //the rest of incomming sessions or future incomming sessions.

                    //Time to exchange incomming Omni-Token with a PII Session.
                    //See for reference: http://dev.xomni.com/v2-1/http-api/public-apis/omniplay/omniticket/using-omniticket-for-a-pii
                    PIISession session = await xomniClient.ExchangeOmniTokenWithPiiSessionAsync(incommingOmniTicket);
                    if (session != null)
                    {
                        //Fetching wishlists of PII User
                        //See for reference : http://dev.xomni.com/v2-1/http-api/public-apis/pii/wishlist/fetching-all-wish-lists
                        WishlistUniqueKeyResponse uniqueKeys = await xomniClient.GetWishlistUniqueKeysAsync(session.Data.SessionGuid);
                        if (uniqueKeys.IsSuccess)
                        {
                            Guid uniqueKey = uniqueKeys.Data.FirstOrDefault();
                            if (uniqueKey != default(Guid))
                            {
                                //Fetching wishlist items in a wishlist.
                                //See for reference : http://dev.xomni.com/v2-1/http-api/public-apis/pii/wishlist/fetching-a-wish-list-with-a-wish-list-unique-key
                                WishlistResponse wishlist = await xomniClient.GetWishlistAsync(uniqueKey.ToString(), session.Data.SessionGuid);
                                List<int> itemIds = wishlist.Data.WishlistItems.Select(t => t.Item.Id).ToList();
                                MessageBox.Show(string.Format("Wishlist name : {0}\nIds of items in wishlist:{1},{2}", wishlist.Data.Name, itemIds[0], itemIds[1]));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Omni-Ticket Exchange failed.");
                    }
                }
                else
                {
                    incommingOmniTicket = "";
                    if (!string.IsNullOrEmpty(currentTargetDeviceId))
                    {
                        timerPolling.Start();
                    }
                }
            }
            else if (pollingResult.HttpStatusCode == HttpStatusCode.NotFound)
            {
                //No incomming session found.
                incommingOmniTicket = "";
                if (!string.IsNullOrEmpty(currentTargetDeviceId))
                {
                    timerPolling.Start();
                }
            }
        }

        DispatcherTimer timerPolling = new DispatcherTimer();
        void btn_PollSession_Click(object sender, RoutedEventArgs e)
        {
            btn_PollSession.Content = "Polling Started";
            timerPolling.Interval = TimeSpan.FromSeconds(1);
            if (!string.IsNullOrEmpty(currentTargetDeviceId))
            {
                timerPolling.Start();
            }
        }

        #endregion

        #region Device Registration

        async void btn_RegisterDevice_Click(object sender, RoutedEventArgs e)
        {
            ////See for reference: http://dev.xomni.com/v3-0/http-api/public-apis/company/device/registering-a-device-to-a-specific-license
            using (ClientContext clientContext = new ClientContext(ApiClientAccessLicenceName, ApiClientAccessLicencePass, ApiEndpointUri))
            {
                XOMNI.SDK.Public.Models.Company.Device sampleDevice = new XOMNI.SDK.Public.Models.Company.Device()
                {
                    Description = "This is a device created on " + DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString(),
                    DeviceId = Guid.NewGuid().ToString()
                };
                var deviceClient = clientContext.Of<DeviceClient>();
                try
                {
                    var response = await deviceClient.PostAsync(sampleDevice);
                    currentTargetDeviceId = response.Data.DeviceId;
                    MessageBox.Show("Registration succeeded");
                }
                catch
                {
                    currentTargetDeviceId = "";
                    MessageBox.Show("Registration failed.");
                }
            }
        }
        #endregion

    }
}