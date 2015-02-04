using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XOMNI.SDK.Public;

namespace KioskApp
{
    public sealed partial class AppSettingsFlyout : SettingsFlyout
    {

        public const string apiServiceUriConfigKey = "ApiURI";

        public const string apiUserNameConfigKey = "ApiUserName";

        public const string apiUserPassConfigKey = "ApiUserPass";

        public const string loginUrlConfigKey = "LoginURL";

        public const string deviceIdConfigKey = "DeviceId";

        public string deviceDescription;

        public AppSettingsFlyout()
        {
            this.InitializeComponent();
            this.Loaded += AppSettingsFlyout_Loaded;
            this.deviceDescription = Helpers.DeviceIdentity.GetFriendlyName();
        }

        void AppSettingsFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            var apiURI = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey];
            var apiUserName = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey];
            var apiUserPass = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey];
            var loginUrl = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.loginUrlConfigKey];
            var deviceId = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.deviceIdConfigKey];

            if(apiURI != null)
            {
                txtApiEndpoint.Text = apiURI.ToString();
            }
            if(apiUserName != null)
            {
                txtApiUserName.Text = apiUserName.ToString();
            }            
            if(apiUserPass != null)
            {
                txtApiUserPass.Password = apiUserPass.ToString();
            }
            if (loginUrl != null)
            {
                txtLoginUrl.Text = loginUrl.ToString();
            }
            if (deviceId != null)
            {
                txtDeviceId.Text = deviceId.ToString();
            }  
        }

        private async void btnSave_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey] = txtApiEndpoint.Text;
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey] = txtApiUserName.Text;
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey] = txtApiUserPass.Password;
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.loginUrlConfigKey] = txtLoginUrl.Text;


            if (ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.deviceIdConfigKey] != txtDeviceId.Text)
            {
                #region using (Create Client Context)
                using (var clientContext = new ClientContext(
                ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey].ToString(),
                ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey].ToString(),  
                ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey].ToString()
                ))
                #endregion
                {
                    var deviceClient = clientContext.Of<XOMNI.SDK.Public.Clients.Company.DeviceClient>();

                    try
                    {
                        ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.deviceIdConfigKey] = txtDeviceId.Text;
                        #region Register Device
                        var registeredDevice = (await deviceClient.PostAsync(new XOMNI.SDK.Public.Models.Company.Device()
                        {
                            DeviceId = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.deviceIdConfigKey].ToString(),
                            Description = deviceDescription
                        })).Data;
                        #endregion          
                    }
                    catch (XOMNI.SDK.Public.Exceptions.XOMNIPublicAPIException ex)
                    {
                        var messagebox = new MessageDialog(ex.Message,"An error occured");
                        messagebox.ShowAsync();

                    }
               }
           }



        }
      }
    }
