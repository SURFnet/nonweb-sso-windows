using System;

namespace SurfnetSSO {
    public class SSOManager {

        public delegate void AuthorizationEventHandler(object sender, AuthorizationEventArgs e);

        public static event AuthorizationEventHandler AuthorizationFinished;

        /// <summary>
        /// Returns the URI for the authorization flow.
        /// </summary>
        /// <param name="consumerId">The consumer ID of this application</param>
        /// <param name="endpoint">The URL of the server</param>
        /// <param name="endpoint">The path relative to the URL which points to the authentication page</param>
        /// <param name="callbackUrl">The callback URL which will be invoked by the server at the end of the flow</param>
        public static Uri GetAuthorizationUri(string consumerId, string endpoint, string callbackUrl) {
            if (string.IsNullOrEmpty(consumerId)) {
                throw new ArgumentNullException("Consumer ID can not be empty!");
            }
            if (string.IsNullOrEmpty(endpoint)) {
                throw new ArgumentNullException("Endpoint can not be empty!");
            }
            if (string.IsNullOrEmpty(callbackUrl)) {
                throw new ArgumentNullException("Scheme can not be empty!");
            }
            string url = _addQueryParameter(endpoint, "client_id", consumerId);
            url = _addQueryParameter(url, "response_type", "token");
            url = _addQueryParameter(url, "state", "surfnet");
            url = _addQueryParameter(url, "scope", "authorize");
            return new Uri(url, UriKind.Absolute);
        }

        public static bool IsCallbackUrl(string url, string callbackUrl) {
            return url.StartsWith(callbackUrl);
        }

        protected static AuthorizationEventArgs ExtractArgsFromUrlWhenReady(string url, string callbackUrl) {
            if (callbackUrl != null && IsCallbackUrl(url, callbackUrl) || callbackUrl == null) {
                url = callbackUrl == null ? url.Substring(url.IndexOf("#")) : url.Replace(callbackUrl, "");
                url = url.Substring(1); // Remove leading # or ?
                var queryParams = url.Split('&');
                string token = null, errorType = null, errorMessage = null;
                foreach (var query in queryParams) {
                    if (query.StartsWith("error=")) {
                        errorType = query.Replace("error=", "");
                    } else if (query.StartsWith("error_description")) {
                        errorMessage = query.Replace("error_description=", "");
                    } else if (query.StartsWith("access_token=")) {
                        token = query.Replace("access_token=", "");
                    }
                }
                if (token != null) {
                    return AuthorizationEventArgs.Success(token);
                } else {
                    return AuthorizationEventArgs.Error(errorType, errorMessage);
                }
            }
            return null;
        }
        private static string _addQueryParameter(string url, string key, string value) {
            string result = url;
            if (url.Contains("?")) {
                result += "&"; 
            } else {
                result += "?";
            }
            result += key + "=" + value;
            return result;
        }

        protected static void OnAuthorizationFinished(AuthorizationEventArgs args) {
            AuthorizationEventHandler handler = AuthorizationFinished;
            if (handler != null) {
                handler(null, args);
            }
        }
    }   
}
