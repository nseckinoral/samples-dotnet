using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Company;
using XOMNI.SDK.Public.Models.Company;

namespace KioskApp
{
    public sealed partial class AppSettingsFlyout : SettingsFlyout
    {
        public AppSettingsFlyout()
        {
            this.InitializeComponent();
            this.Loaded += AppSettingsFlyout_Loaded;
        }

        void AppSettingsFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            txtApiEndpoint.Text = AppSettings.ApiUri ?? string.Empty;
            txtApiUserName.Text = AppSettings.ApiUsername ?? string.Empty;
            txtApiUserPass.Password = AppSettings.ApiUserPass ?? string.Empty;
            txtLoginUrl.Text = AppSettings.LoginUrl ?? string.Empty;
            txtDeviceId.Text = AppSettings.DeviceId ?? string.Empty;
        }

        private async void btnSave_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            AppSettings.ApiUri = txtApiEndpoint.Text;
            AppSettings.ApiUsername = txtApiUserName.Text;
            AppSettings.ApiUserPass = txtApiUserPass.Password;
            AppSettings.LoginUrl = txtLoginUrl.Text;

            if (string.IsNullOrEmpty(txtDeviceId.Text))
            {
                var messageBox = new MessageDialog("Data validation failed. Device ID is required.", "An error occured");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                    settingsFlyOut.Show();
                }));
                await messageBox.ShowAsync();
            }
            else if (txtDeviceId.Text.Contains(" "))
            {
                var messageBox = new MessageDialog("Data validation failed. Device ID can not contain any whitespace.", "An error occured");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                    settingsFlyOut.Show();
                }));
                await messageBox.ShowAsync();
            }
            else if (AppSettings.DeviceId != txtDeviceId.Text)
            {
                try
                {
                    using (var clientContext = new ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                    {
                        var deviceClient = clientContext.Of<DeviceClient>();
                        try
                        {
                            var registeredDevice = (await deviceClient.PostAsync(new Device()
                            {
                                DeviceId = txtDeviceId.Text,
                                Description = Helpers.DeviceIdentity.GetFriendlyName()
                            })).Data;
                            AppSettings.DeviceId = txtDeviceId.Text;
                            var messageBox = new MessageDialog(string.Format("Device ID '{0}' is successfully registered.", registeredDevice.DeviceId), "Success!");
                            await messageBox.ShowAsync();
                        }

                        catch (Exception ex)
                        {
                            var messageBox = new MessageDialog(ex.Message, "Please try again!");
                            messageBox.Commands.Add(new UICommand("Close", (command) =>
                            {
                                AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                                settingsFlyOut.Show();
                            }));
                            messageBox.ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var messageBox = new MessageDialog(ex.Message, "An error occured");
                    messageBox.Commands.Add(new UICommand("Close", (command) =>
                    {
                        AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                        settingsFlyOut.Show();
                    }));
                    messageBox.ShowAsync();
                }
            }
        }
    }
}
