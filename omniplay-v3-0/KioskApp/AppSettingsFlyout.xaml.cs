using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App1
{
    public sealed partial class AppSettingsFlyout : SettingsFlyout
    {

        public const string apiServiceUriConfigKey = "ApiURI";

        public const string apiUserNameConfigKey = "ApiUserName";

        public const string apiUserPassConfigKey = "ApiUserPass";

        public const string loginUrlConfigKey = "LoginURL";

        public AppSettingsFlyout()
        {
            this.InitializeComponent();
            this.Loaded += AppSettingsFlyout_Loaded;
            this.Unloaded += AppSettingsFlyout_Unloaded;
        }

        void AppSettingsFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            var apiURI = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey];
            var apiUserName = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey];
            var apiUserPass = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey];
            var loginUrl = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.loginUrlConfigKey];
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
        }

        void AppSettingsFlyout_Unloaded(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey] = txtApiEndpoint.Text;
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey] = txtApiUserName.Text;
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey] = txtApiUserPass.Password;
            ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.loginUrlConfigKey] = txtLoginUrl.Text;
        }
    }
}
