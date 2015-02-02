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

        public AppSettingsFlyout()
        {
            this.InitializeComponent();
            this.Loaded += AppSettingsFlyout_Loaded;
            this.Unloaded += AppSettingsFlyout_Unloaded;
        }

        void AppSettingsFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            var ApiURI = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiServiceUriConfigKey];
            var ApiUserName = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserNameConfigKey];
            var ApiUserPass = ApplicationData.Current.LocalSettings.Values[AppSettingsFlyout.apiUserPassConfigKey];
            if(ApiURI != null)
            {
                txtApiEndpoint.Text = ApiURI.ToString();
            }
            if(ApiUserName != null)
            {
                txtApiUserName.Text = ApiUserName.ToString();
            }            
            if(ApiUserPass != null)
            {
                txtApiUserPass.Password = ApiUserPass.ToString();
            }           
        }

        void AppSettingsFlyout_Unloaded(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values.Add(AppSettingsFlyout.apiServiceUriConfigKey, txtApiEndpoint.Text);
            ApplicationData.Current.LocalSettings.Values.Add(AppSettingsFlyout.apiUserNameConfigKey, txtApiUserName.Text);
            ApplicationData.Current.LocalSettings.Values.Add(AppSettingsFlyout.apiUserPassConfigKey, txtApiUserPass.Password);
        }
    }
}
