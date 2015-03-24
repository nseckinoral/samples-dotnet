using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.Catalog;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Inventory_Sample_App
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
            txtItemId.Text = AppSettings.ItemId ?? string.Empty;
        }

        private async void btnSave_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

            //Check if Settings are empty or not
            if (string.IsNullOrEmpty(txtApiEndpoint.Text) || string.IsNullOrEmpty(txtApiUserName.Text) || string.IsNullOrEmpty(txtApiUserPass.Password) || string.IsNullOrEmpty(txtItemId.Text))
            {

                MessageDialog messageBox = new MessageDialog("Settings can't be empty.", "An error occured");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                    settingsFlyOut.Show();
                }));
                await messageBox.ShowAsync();
            }

            else
            {
                try
                {
                    //Validate through APIs
                    using(var clientContext = new ClientContext(txtApiUserName.Text,txtApiUserPass.Password,txtApiEndpoint.Text))
                    {
                        var itemId = Int32.Parse(txtItemId.Text);
                        var itemClient = clientContext.Of<ItemClient>();
                        var itemValidation = await itemClient.GetAsync(itemId);
                    }

                    //Save
                    AppSettings.ApiUri = txtApiEndpoint.Text;
                    AppSettings.ApiUsername = txtApiUserName.Text;
                    AppSettings.ApiUserPass = txtApiUserPass.Password;
                    AppSettings.ItemId = txtItemId.Text;
                    btnSave.IsEnabled = false;
                }
                catch(XOMNI.SDK.Public.Exceptions.XOMNIPublicAPIException ex)
                {
                    var messageBox = new MessageDialog("Make sure your settings are valid.", "Validation failed");
                    messageBox.Commands.Add(new UICommand("Close", (command) =>
                    {
                        AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                        settingsFlyOut.Show();
                    }));
                    messageBox.ShowAsync();                  
                }
                catch (Exception ex)
                {
                    var messageBox = new MessageDialog(ex.Message, "An error occurred");
                    messageBox.Commands.Add(new UICommand("Close", (command) =>
                    {
                        AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
                        settingsFlyOut.Show();
                    }));
                    messageBox.ShowAsync();
                }
            }
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

        private void txtItemId_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (AppSettings.ItemId == txtItemId.Text)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }
        }

        private void txtItemId_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((e.Key < VirtualKey.Number0) || (e.Key > VirtualKey.Number9))
            {
                // If it's not a numeric character, prevent the TextBox from handling the keystroke
                e.Handled = true;
            }
        }
    }

}
