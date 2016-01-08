using System;
using System.Windows.Forms;

namespace SurfnetSSO {
    /// <summary>
    /// !!! Important !!!
    /// Do NOT use callback URLs for pages which do not exist. For optimal use, redirect to an URL that you own.
    /// For more info, see: http://davidquail.com/2010/01/10/c-webbrowser-swallows-redirect-uri/
    /// </summary>
    public class WinFormsSSOManager : SSOManager {

        public static void authorize(WebBrowser webBrowser, String consumerId, String endpoint, String callbackUrl) {
            Uri uri = authorize(consumerId, endpoint, callbackUrl);

            webBrowser.LocationChanged += (sender, e) => {
                _webLocationChanged(webBrowser.Url, callbackUrl);
            };
            webBrowser.Navigating += (sender, e) => {
                _webLocationChanged(e.Url, callbackUrl);
            };
            webBrowser.Navigated += (sender, e) => {
                _webLocationChanged(e.Url, callbackUrl);
            };

            webBrowser.Navigate(uri.AbsoluteUri);
        }

        private static void _webLocationChanged(Uri uri, String callbackUrl) {
            var args = extractArgsFromUrlWhenReady(uri.AbsoluteUri, callbackUrl);
            if (args != null) {
                OnAuthorizationFinished(args);
            }
        }
    }
}