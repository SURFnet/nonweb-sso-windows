using SurfnetSSO;
using System;
using System.Windows.Forms;

namespace Net45Sample {
    /// <summary>
    /// The main form containing the web browser, the login button, and the result text used for showcasing the library.
    /// </summary>
    public partial class MainForm : Form {

        private static string SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static string CLIENT_ID = "kiu1p8hkyyytes06z12e1t";
        private static string CALLBACK_URL = "https://nonweb.demo.surfconext.nl/php-oauth-as/manage.php"; 

        /// <summary>
        /// Constructor. Initializes the application with its components.
        /// </summary>
        public MainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event of the login button.
        /// </summary>
        /// <param name="sender">The sender, in this case the login button</param>
        /// <param name="e">The arguments of the event (not used in this case)</param>
        private void loginButton_Click(object sender, EventArgs e) {
            // Load the webpage
            SSOManager.AuthorizationFinished += WinFormsSSOManager_AuthorizationFinished;
            WinFormsSSOManager.Authorize(browser, CLIENT_ID, SERVER_ENDPOINT, CALLBACK_URL);
        }

        /// <summary>
        /// Event handler for the authorization flow finished event.
        /// In this showcase only the result is printed, ideally you would save the token (or handle the error),
        /// and sign all future requests with it.
        /// </summary>
        /// <param name="sender">The sender of the event (not used)</param>
        /// <param name="e">The event arguments containing the details (error message, token, etc.)</param>
        private void WinFormsSSOManager_AuthorizationFinished(object sender, AuthorizationEventArgs e) {
            browser.Navigate("about:blank");
            if (e.IsSuccessful) {
                result.Text = "SUCCESS - token is " + e.Token;
            } else {
                result.Text = "ERROR: " + e.ErrorType + " - " + e.ErrorMessage;
            }
        }
    }
}
