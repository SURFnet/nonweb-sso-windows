using SurfnetSSO.Platform.WP8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WP8Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static string CLIENT_ID = "2yBQr5UmCP2xWQm6as2lJpLT32R5EXEs";
        private static string CALLBACK_URL = "ms-app://s-1-15-2-1094118375-1795814147-1252274320-1362446442-1945129656-3401392025-3815910212/";


        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) {
            WP8SSOManager.AuthorizationFinished += WP8SSOManager_AuthorizationFinished; 
            WP8SSOManager.Authorize(CLIENT_ID, SERVER_ENDPOINT, CALLBACK_URL);
        }

        private void WP8SSOManager_AuthorizationFinished(object sender, SurfnetSSO.AuthorizationEventArgs e) {
            if (e.IsSuccessful) {
                resultText.Text = "SUCCESS - token is " + e.Token;
            } else {
                resultText.Text = "ERROR: " + e.ErrorType + " - " + e.ErrorMessage;
            }
        }
    }
}
