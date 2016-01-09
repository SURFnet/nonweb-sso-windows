using SurfnetSSO.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPSample {
    /// <summary>
    /// The main page containing the login button and result text used for showcasing the library.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static string CLIENT_ID = "oByWgpuLVShT7CMVujj5FamwANkDhC2i";
        private static string CALLBACK_URL = "ms-app://s-1-15-2-157433811-2579606017-4084747525-3618661302-965243146-571244127-4048958279/";

        /// <summary>
        /// Constructor, initializes the navigation and components of the page.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event of the login button
        /// </summary>
        /// <param name="sender">The sender, in this case the login button</param>
        /// <param name="e">The arguments of the event (not used in this case)</param>
        private void loginButton_Click(object sender, RoutedEventArgs e) {
            // Subscribe to the authorization finished event, and start the flow.
            SurfnetSSO.SSOManager.AuthorizationFinished += UWPSSOManager_AuthorizationFinished;
            UWPSSOManager.Authorize(CLIENT_ID, SERVER_ENDPOINT, CALLBACK_URL);
        }

        /// <summary>
        /// Event handler for the authorization flow finished event.
        /// In this showcase only the result is printed, ideally you would save the token (or handle the error),
        /// and sign all future requests with it.
        /// </summary>
        /// <param name="sender">The sender of the event (not used)</param>
        /// <param name="e">The event arguments containing the details (error message, token, etc.)</param>
        private void UWPSSOManager_AuthorizationFinished(object sender, SurfnetSSO.AuthorizationEventArgs e) {
            if (e.IsSuccessful) {
                resultText.Text = "SUCCESS - token is " + e.Token;
            } else {
                resultText.Text = "ERROR: " + e.ErrorType + " - " + e.ErrorMessage;
            }
            
        }
    }
}
