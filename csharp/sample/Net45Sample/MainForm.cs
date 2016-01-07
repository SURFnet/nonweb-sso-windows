using SurfnetSSO;
using System;
using System.Windows.Forms;

namespace Net45Sample {
    public partial class MainForm : Form {

        private static String SERVER_ENDPOINT = "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php";
        private static String CLIENT_ID = "4dca00da67c692296690e90c50c96b79";
        private static String SCHEME = "sfoauth"; 

        public MainForm() {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e) {
            // Load the webpage
            WinFormsSSOManager.AuthorizationFinished += WinFormsSSOManager_AuthorizationFinished;
            WinFormsSSOManager.authorize(this.browser, CLIENT_ID, SERVER_ENDPOINT, SCHEME);
        }

        private void WinFormsSSOManager_AuthorizationFinished(object sender, AuthorizationEventArgs e) {
            throw new NotImplementedException(e.Token);
        }
    }
}
