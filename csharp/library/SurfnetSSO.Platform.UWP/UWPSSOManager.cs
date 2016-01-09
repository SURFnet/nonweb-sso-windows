using System;
using System.Diagnostics;
using Windows.Security.Authentication.Web;

namespace SurfnetSSO.Platform.UWP
{
    public class UWPSSOManager : SSOManager
    {
        public static string GetCallbackUrl() {
            return WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;
        }

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
