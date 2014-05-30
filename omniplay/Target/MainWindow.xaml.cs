using System;
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
        string incommingOmniTicket;

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
            //See for reference: http://dev.xomni.com/v2-0/http-api/public-apis/omniplay/device/fetching-pii-user-omnitickets-on-omniplay-device-queue

            XomniClient xomniClient = new XomniClient();
            PollingResponseObject pollingResult = await xomniClient.GetIncomingDevices(currentTargetDeviceId);
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
                    //See for reference: http://dev.xomni.com/v2-0/http-api/public-apis/omniplay/omniticket/using-omniticket-for-a-pii
                    PIISession session = await xomniClient.ExchangeOmniTokenWithPiiSession(incommingOmniTicket);
                    if(session != null)
                    {
                        MessageBox.Show("This is the new PII Header on target device: " + Convert.ToBase64String(Encoding.UTF8.GetBytes("sessionGuid:" + session.Data.SessionGuid )));
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
            //See for reference: http://dev.xomni.com/v2-0/http-api/public-apis/omniplay/device/registering-a-device-to-a-specific-license

            XomniClient xomniClient = new XomniClient();
            RegisterRequestObject reqOb = new RegisterRequestObject()
            {
                Description = "This is a device created on " + DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString(),
                DeviceId = Guid.NewGuid().ToString()
            };
            bool isRegistered = await xomniClient.RegisterDevice(reqOb);
            if (isRegistered)
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