using System;
using Windows.Security.Authentication.Web;

namespace SurfnetSSO.Platform.UWP {
    public class UWPSSOManager : SSOManager
    {
        /// <summary>
        /// Returns the callback URL for this application. Useful method for finding it out.
        /// </summary>
        /// <returns>The callback URL this application responds to</returns>
        public static string GetCallbackUrl() {
            return WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;
        }

        /// <summary>
        /// Starts the authorization flow.
        /// When finished, an AuthorizationFinished event will be sent with the arguments containing the result.
        /// </summary>
        /// <param name="consumerId">The consumer / client ID of the OAuth2 application</param>
        /// <param name="endpoint">The authorization endpoint on the server</param>
        /// <param name="callbackUrl">The callback URL the application responds to (see GetCallbackUrl)</param>
        public static async void Authorize(string consumerId, string endpoint, string callbackUrl) {
            var authorizationUrl = GetAuthorizationUri(consumerId, endpoint, callbackUrl);
            WebAuthenticationResult webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                            WebAuthenticationOptions.None,
                                            authorizationUrl,
                                            new Uri(callbackUrl));
            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success) {
                var args = ExtractArgsFromUrlWhenReady(webAuthenticationResult.ResponseData, callbackUrl);
                OnAuthorizationFinished(args);
            } else {
                var args = AuthorizationEventArgs.Error(webAuthenticationResult.ResponseErrorDetail.ToString(), webAuthenticationResult.ResponseData);;
                OnAuthorizationFinished(args);
            }
        }
    }
}
