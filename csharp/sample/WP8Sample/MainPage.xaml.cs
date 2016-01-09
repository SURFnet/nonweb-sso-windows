using SurfnetSSO.Platform.WP8;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WP8Sample {
    /// <summary>
    /// The main page containing the login button and result text used for showcasing the library.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static string CLIENT_ID = "2yBQr5UmCP2xWQm6as2lJpLT32R5EXEs";
        private static string CALLBACK_URL = "ms-app://s-1-15-2-1094118375-1795814147-1252274320-1362446442-1945129656-3401392025-3815910212/";

        /// <summary>
        /// Constructor, initializes the navigation and components of the page.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
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

        /// <summary>
        /// Handles the click event of the login button
        /// </summary>
        /// <param name="sender">The sender, in this case the login button</param>
        /// <param name="e">The arguments of the event (not used in this case)</param>
        private void loginButton_Click(object sender, RoutedEventArgs e) {
            // Subscribe to the authorization finished event, and start the flow.
            SurfnetSSO.SSOManager.AuthorizationFinished += WP8SSOManager_AuthorizationFinished; 
            WP8SSOManager.Authorize(CLIENT_ID, SERVER_ENDPOINT, CALLBACK_URL);
        }

        /// <summary>
        /// Event handler for the authorization flow finished event.
        /// In this showcase only the result is printed, ideally you would save the token (or handle the error),
        /// and sign all future requests with it.
        /// </summary>
        /// <param name="sender">The sender of the event (not used)</param>
        /// <param name="e">The event arguments containing the details (error message, token, etc.)</param>
        private void WP8SSOManager_AuthorizationFinished(object sender, SurfnetSSO.AuthorizationEventArgs e) {
            if (e.IsSuccessful) {
                resultText.Text = "SUCCESS - token is " + e.Token;
            } else {
                resultText.Text = "ERROR: " + e.ErrorType + " - " + e.ErrorMessage;
            }
        }
    }
}
