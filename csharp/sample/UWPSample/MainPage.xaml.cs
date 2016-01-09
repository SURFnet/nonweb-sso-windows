using SurfnetSSO.Platform.UWP;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static string CLIENT_ID = "u4Af6KtmRxZGDMFr";
        private static string CALLBACK_URL = "ms-app://s-1-15-2-157433811-2579606017-4084747525-3618661302-965243146-571244127-4048958279/";

        public MainPage()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) {
            SurfnetSSO.SSOManager.AuthorizationFinished += UWPSSOManager_AuthorizationFinished;
            UWPSSOManager.Authorize(CLIENT_ID, SERVER_ENDPOINT, CALLBACK_URL);
        }

        private void UWPSSOManager_AuthorizationFinished(object sender, SurfnetSSO.AuthorizationEventArgs e) {
            if (e.IsSuccessful) {
                resultText.Text = "SUCCESS - token is " + e.Token;
            } else {
                resultText.Text = "ERROR: " + e.ErrorType + " - " + e.ErrorMessage;
            }
            
        }
    }
}
