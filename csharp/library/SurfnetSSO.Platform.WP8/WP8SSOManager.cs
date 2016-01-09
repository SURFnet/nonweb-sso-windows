using System;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;

namespace SurfnetSSO.Platform.WP8
{
    public class WP8SSOManager : SSOManager
    {
        public static string GetCallbackUrl() {
            return WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;
        }

        public static void Authorize(string consumerId, string endpoint, string callbackUrl) {
            var authorizationUrl = GetAuthorizationUri(consumerId, endpoint, callbackUrl);
            WebAuthenticationBroker.AuthenticateAndContinue(authorizationUrl, new Uri(callbackUrl));
        }

        public static AuthorizationEventArgs OnAppActivated(IActivatedEventArgs args) {
            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation) {
                var webArgs = args as WebAuthenticationBrokerContinuationEventArgs;
                var authorizedUrl = webArgs.WebAuthenticationResult.ResponseData;
                Debug.WriteLine(authorizedUrl);
                var authorizationArgs = ExtractArgsFromUrlWhenReady(authorizedUrl, null);
                OnAuthorizationFinished(authorizationArgs);
                return authorizationArgs;
            } else {
                return null;
            }
        }
    }
}
