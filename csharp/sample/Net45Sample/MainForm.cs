using SurfnetSSO;
using System;
using System.Windows.Forms;

namespace Net45Sample {
    public partial class MainForm : Form {

        private static String SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static String CLIENT_ID = "kiu1p8hkyyytes06z12e1t";
        private static String CALLBACK_URL = "https://nonweb.demo.surfconext.nl/php-oauth-as/manage.php"; 

        public MainForm() {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e) {
            // Load the webpage
            WinFormsSSOManager.AuthorizationFinished += WinFormsSSOManager_AuthorizationFinished;
            WinFormsSSOManager.authorize(this.browser, CLIENT_ID, SERVER_ENDPOINT, CALLBACK_URL);
        }

        private void WinFormsSSOManager_AuthorizationFinished(object sender, AuthorizationEventArgs e) {
            this.browser.Navigate("about:blank");
            if (e.IsSuccessful) {
                this.result.Text = "SUCCESS - token is " + e.Token;
            } else {
                this.result.Text = "ERROR: " + e.ErrorType + " - " + e.ErrorMessage;
            }
        }
    }
}
