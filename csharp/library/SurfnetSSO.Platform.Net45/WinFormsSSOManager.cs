using System;
using System.Windows.Forms;

namespace SurfnetSSO {
    /// <summary>
    /// !!! Important !!!
    /// Do NOT use callback URLs for pages which do not exist. For optimal use, redirect to an URL that you own.
    /// For more info, see: http://davidquail.com/2010/01/10/c-webbrowser-swallows-redirect-uri/
    /// </summary>
    public class WinFormsSSOManager : SSOManager {

        /// <summary>
        /// Starts the authorization flow.
        /// When finished, an AuthorizationFinished event will be sent with the arguments containing the result.
        /// </summary>
        /// <param name="webBrowser">The WebBrowser the login page will be shown in</param>
        /// <param name="consumerId">The consumer / client ID of the OAuth2 application</param>
        /// <param name="endpoint">The authorization endpoint on the server</param>
        /// <param name="callbackUrl">The callback URL the application responds to (see GetCallbackUrl)</param>
        public static void Authorize(WebBrowser webBrowser, string consumerId, string endpoint, string callbackUrl) {
            Uri uri = GetAuthorizationUri(consumerId, endpoint, callbackUrl);

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

        /// <summary>
        /// Internal event handler which checks if the authorization flow has finished.
        /// </summary>
        /// <param name="uri">The uri of the currently displayed page</param>
        /// <param name="callbackUrl">The callback URL the client is waiting for</param>
        private static void _webLocationChanged(Uri uri, string callbackUrl) {
            var args = ExtractArgsFromUrlWhenReady(uri.AbsoluteUri, callbackUrl);
            if (args != null) {
                OnAuthorizationFinished(args);
            }
        }
    }
}