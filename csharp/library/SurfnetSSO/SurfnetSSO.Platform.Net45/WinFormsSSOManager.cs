using System;
using System.Windows.Forms;

namespace SurfnetSSO {
    public class WinFormsSSOManager : SSOManager {

        public static void authorize(WebBrowser webForm, String consumerId, String endpoint, String scheme) {
            Uri uri = authorize(consumerId, endpoint, scheme);
            webForm.Navigate(uri.ToString());
            webForm.DocumentCompleted += (sender, e) => {
                Console.WriteLine(e.Url.ToString());
                if (isCallbackUrl(e.Url, scheme)) {
                    OnAuthorizationFinished(AuthorizationEventArgs.Success("lalala"));
                }
            };
        }
    }
}