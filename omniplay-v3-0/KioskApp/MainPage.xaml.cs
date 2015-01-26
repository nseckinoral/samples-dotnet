using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App1
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void login_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            QR.Opacity = 100;
            QR.IsHitTestVisible = true;
            Qr_In.Begin();
        }

        private void QR_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Qr_Out.Begin();
            QR.IsHitTestVisible = false;
            login_btn.IsEnabled = false;
            logout_btn.IsEnabled = true;
            mycart_btn.IsEnabled = true;
            wishlist_btn.IsEnabled = true;
        }

        private void logout_btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            login_btn.IsEnabled = true;
            logout_btn.IsEnabled = false;
            mycart_btn.IsEnabled = false;
            wishlist_btn.IsEnabled = false;
        }
    }
}
