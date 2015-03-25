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
        static Frame  frame = (Frame)Window.Current.Content;
        static MainPage mainPage = (MainPage)frame.Content;
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
            txtInStockItemId.Text = AppSettings.InStockItemId ?? string.Empty;
            txtOutOfStockItemId.Text = AppSettings.OutOfStockItemId ?? string.Empty;
        }

        private async void btnSave_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            AppSettingsFlyout settingsFlyOut = new AppSettingsFlyout();
            //Check if Settings are empty or not
            if (string.IsNullOrEmpty(txtApiEndpoint.Text) || string.IsNullOrEmpty(txtApiUserName.Text) || string.IsNullOrEmpty(txtApiUserPass.Password) || string.IsNullOrEmpty(txtInStockItemId.Text) || string.IsNullOrEmpty(txtOutOfStockItemId.Text))
            {

                MessageDialog messageBox = new MessageDialog("Settings can't be empty.", "An error occured");
                messageBox.Commands.Add(new UICommand("Close", (command) =>
                {
                    settingsFlyOut.Show();
                }));
                await messageBox.ShowAsync();
            }

            else
            {
                try
                {
                    DisableScreen();
                    //Validate through APIs
                    using(var clientContext = new ClientContext(txtApiUserName.Text,txtApiUserPass.Password,txtApiEndpoint.Text))
                    {
                        var inStockItemId = Int32.Parse(txtInStockItemId.Text);
                        var outOfStockItemId = Int32.Parse(txtOutOfStockItemId.Text);
                        var itemClient = clientContext.Of<ItemClient>();
                        var inStockItemValidation = await itemClient.GetAsync(inStockItemId);
                        var outOfStockItemValidation = await itemClient.GetAsync(outOfStockItemId);
                    }

                    //Save
                    AppSettings.ApiUri = txtApiEndpoint.Text;
                    AppSettings.ApiUsername = txtApiUserName.Text;
                    AppSettings.ApiUserPass = txtApiUserPass.Password;
                    AppSettings.InStockItemId = txtInStockItemId.Text;
                    AppSettings.OutOfStockItemId = txtOutOfStockItemId.Text;
                    btnSave.IsEnabled = false;
                    EnableScreen();
                }
                catch(XOMNI.SDK.Public.Exceptions.XOMNIPublicAPIException ex)
                {
                    var messageBox = new MessageDialog("Make sure your settings are valid.", "Validation failed");
                    messageBox.Commands.Add(new UICommand("Close", (command) =>
                    {
                        mainPage.commonProgressRing.IsActive = false;
                        EnableIfSettingsAreNotEmpty();
                        settingsFlyOut.Show();
                    }));
                    messageBox.ShowAsync();
                }
                catch (Exception ex)
                {
                    var messageBox = new MessageDialog("An error occured while sending the request. Please try again.", "An error occurred");
                    messageBox.Commands.Add(new UICommand("Close", (command) =>
                    {
                        mainPage.commonProgressRing.IsActive = false;
                        EnableIfSettingsAreNotEmpty();
                        settingsFlyOut.Show();
                    }));
                    messageBox.ShowAsync();
                }
            }
        }

        private void EnableIfSettingsAreNotEmpty()
        {
            bool IsSaved= !String.IsNullOrEmpty(AppSettings.ApiUri) || !String.IsNullOrEmpty(AppSettings.ApiUsername) || !String.IsNullOrEmpty(AppSettings.ApiUserPass) || !String.IsNullOrEmpty(AppSettings.InStockItemId) || !String.IsNullOrEmpty(AppSettings.OutOfStockItemId);
            if(IsSaved)
            {
                EnableScreen();
            }
        }

        private void EnableScreen()
        {
            mainPage.BlackScreen.Opacity = 0;
            mainPage.BlackScreen.IsHitTestVisible = false;
            mainPage.commonProgressRing.IsActive = false;
        }
        private void DisableScreen()
        {
            mainPage.BlackScreen.Opacity = 0.6;
            mainPage.BlackScreen.IsHitTestVisible = true;
            mainPage.commonProgressRing.IsActive = true;
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

        private void txtOutOfStockItemId_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (AppSettings.OutOfStockItemId == txtOutOfStockItemId.Text)
            {
                btnSave.IsEnabled = false;
            }
            else
            {
                btnSave.IsEnabled = true;
            }
        }

        private void txtInStockItemId_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (AppSettings.OutOfStockItemId == txtInStockItemId.Text)
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
