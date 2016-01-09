using System;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;

namespace SurfnetSSO.Platform.WP8 {
    /// <summary>
    ///  Single Sign-On manager for Windows Phone 8+ applications.
    /// </summary>
    public class WP8SSOManager : SSOManager
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
        public static void Authorize(string consumerId, string endpoint, string callbackUrl) {
            var authorizationUrl = GetAuthorizationUri(consumerId, endpoint, callbackUrl);
            WebAuthenticationBroker.AuthenticateAndContinue(authorizationUrl, new Uri(callbackUrl));
        }

        /// <summary>
        /// App activation handler.
        /// Call this method from your application OnActivated method.
        /// Although this method returns the result in its return, the AuthorizationFinished event will
        /// send the same result as well.
        /// </summary>
        /// <param name="args">The arguments of the app's OnActivated method</param>
        /// <returns>Null if the event was not relevant, otherwise the arguments containing the result</returns>
        public static AuthorizationEventArgs OnAppActivated(IActivatedEventArgs args) {
            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation) {
                var webArgs = args as WebAuthenticationBrokerContinuationEventArgs;
                var authorizedUrl = webArgs.WebAuthenticationResult.ResponseData;
                var authorizationArgs = ExtractArgsFromUrlWhenReady(authorizedUrl, null);
                OnAuthorizationFinished(authorizationArgs);
                return authorizationArgs;
            } else {
                // Not relevant for us
                return null;
            }
        }
    }
}
