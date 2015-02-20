using System;
using System.Threading.Tasks;
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
            //Check if API Settings are empty or not
            if (string.IsNullOrEmpty(txtApiEndpoint.Text) || string.IsNullOrEmpty(txtApiUserName.Text) || string.IsNullOrEmpty(txtApiUserPass.Password))
            {

                MessageDialog messageBox = new MessageDialog("API settings can't be empty.", "An error occured");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                    settingsFlyOut.Show();
                }));
                await messageBox.ShowAsync();
            }

            //Check if API Settings are empty or not
            if(string.IsNullOrEmpty(txtLoginUrl.Text))
            {
                MessageDialog messageBox = new MessageDialog("Login URL can't be empty.", "An error occured");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                    settingsFlyOut.Show();
                }));
                await messageBox.ShowAsync();

            }
            //If Login URL is the only changed textbox, save it and don't register.
            else if (AppSettings.ApiUri == txtApiEndpoint.Text && AppSettings.ApiUsername == txtApiUserName.Text && AppSettings.ApiUserPass == txtApiUserPass.Password && AppSettings.DeviceId == txtDeviceId.Text && AppSettings.LoginUrl != txtLoginUrl.Text)
            {
                AppSettings.LoginUrl = txtLoginUrl.Text;
                btnSave.IsEnabled = false;
                return;
            }

            //Check if Device ID is empty or not
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
            //Check if Device ID contains whitespace
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

            //Registration
            else
            {
                var frame = (Frame)Window.Current.Content;
                var mainPage = (MainPage)frame.Content;
                mainPage.mainPageProgressRing.IsActive = true;
                mainPage.mainPageDisabled.Opacity = 100;
                mainPage.mainPageDisabled.IsHitTestVisible = false;

                var confirmationMessage = new MessageDialog("Previous Device ID will be unregistered. Do you confirm?","Warning!");
                confirmationMessage.Commands.Add(new UICommand("OK", async (command) =>
                {
                    await UnRegisterDevice();
                    await RegisterDevice();
                }));

                confirmationMessage.Commands.Add(new UICommand("Cancel",(command) =>
                {
                    mainPage.mainPageProgressRing.IsActive = false;
                    if(AppSettings.IsRegistered == "true")
                    {
                        mainPage.EnableMainPage();                     
                    }
                    AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                    settingsFlyOut.Show();
                }));
                await confirmationMessage.ShowAsync();
            }


        }


        private async Task RegisterDevice()
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
                            messageBox.Commands.Add(new UICommand("OK", (command) =>
                            {
                                var frame = (Frame)Window.Current.Content;
                                var mainPage = (MainPage)frame.Content;
                                mainPage.EnableMainPage();
                                mainPage.mainPageProgressRing.IsActive = false;
                            }));
                            await messageBox.ShowAsync();
                            AppSettings.IsRegistered = "true";
                        }

                        catch (XOMNI.SDK.Public.Exceptions.XOMNIPublicAPIException ex)
                        {
                            var messageBox = new MessageDialog(ex.Message, "Please try again!");
                            messageBox.Commands.Add(new UICommand("Close", (command) =>
                            {
                                AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                                settingsFlyOut.Show();

                            }));
                            messageBox.ShowAsync();

                            var frame = (Frame)Window.Current.Content;
                            var mainPage = (MainPage)frame.Content;
                            mainPage.mainPageDisabled.IsHitTestVisible = true;
                            mainPage.mainPageProgressRing.IsActive = false;
                        }
                        catch (System.Exception)
                        {
                            var messageBox = new MessageDialog("We're having trouble accomplishing your request. Please make sure your API Settings are correct.", "Please try again!");
                            messageBox.Commands.Add(new UICommand("Close", (command) =>
                            {
                                AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                                settingsFlyOut.Show();

                            }));
                            messageBox.ShowAsync();
                            var frame = (Frame)Window.Current.Content;
                            var mainPage = (MainPage)frame.Content;
                            mainPage.mainPageDisabled.IsHitTestVisible = true;
                            mainPage.mainPageProgressRing.IsActive = false;
                        }
                    }
        }

        private async Task UnRegisterDevice()
        {

                using (var clientContext = new ClientContext(AppSettings.ApiUsername, AppSettings.ApiUserPass, AppSettings.ApiUri))
                {
                    var deviceClient = clientContext.Of<DeviceClient>();
                    try
                    {
                        await deviceClient.DeleteAsync(AppSettings.DeviceId);
                        AppSettings.IsRegistered = "false";
                    }

                    catch
                    {
                        AppSettings.IsRegistered = "false";
                    }
                }
                AppSettings.ApiUri = txtApiEndpoint.Text;
                AppSettings.ApiUsername = txtApiUserName.Text;
                AppSettings.ApiUserPass = txtApiUserPass.Password;
                AppSettings.LoginUrl = txtLoginUrl.Text;
        }

        private void txtApiEndpoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AppSettings.ApiUri == txtApiEndpoint.Text)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }
        }

        private void txtApiUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AppSettings.ApiUsername == txtApiUserName.Text)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }

        }

        private void txtApiUserPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (AppSettings.ApiUserPass == txtApiUserPass.Password)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }
        }

        private void txtLoginUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AppSettings.LoginUrl == txtLoginUrl.Text)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }

        }

        private void txtDeviceId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AppSettings.DeviceId == txtDeviceId.Text)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }
        }

    }

}

