using System;

namespace SurfnetSSO {
    /// <summary>
    /// Base class containing the common code for the SSO managers of each platform.
    /// </summary>
    public class SSOManager {

        /// <summary>
        /// Event handler for distributing the event on the end of the authorization flow
        /// </summary>
        /// <param name="sender">Always null</param>
        /// <param name="e">The event arguments containing the result of the authorization</param>
        public delegate void AuthorizationEventHandler(object sender, AuthorizationEventArgs e);

        /// <summary>
        /// Subscribe to this event to receive the result of the authorization flow.
        /// </summary>
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
                throw new ArgumentNullException("Callback URL can not be empty!");
            }
            string url = _addQueryParameter(endpoint, "client_id", consumerId);
            url = _addQueryParameter(url, "response_type", "token");
            url = _addQueryParameter(url, "state", "surfnet");
            url = _addQueryParameter(url, "scope", "authorize");
            return new Uri(url, UriKind.Absolute);
        }

        /// <summary>
        /// Checks if the current URL is the callback URL the client has been waiting for.
        /// </summary>
        /// <param name="url">The URL to check</param>
        /// <param name="callbackUrl">The callback URL of the client</param>
        /// <returns></returns>
        public static bool IsCallbackUrl(string url, string callbackUrl) {
            return url.StartsWith(callbackUrl);
        }

        /// <summary>
        /// Extracts the OAuth2 parameters from the URL at the end of the flow,
        /// and converts it into the event arguments the application is expecting.
        /// </summary>
        /// <param name="url">The URL to extract from</param>
        /// <param name="callbackUrl">The callback URL. If null, there will be no check if the current URL
        /// is the callback URL the client is waiting for.</param>
        /// <returns>Null if the input URL is not the callback URL (yet). Otherwise the arguments containing
        /// the result of the authorization flow.</returns>
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

        /// <summary>
        /// Adds a query parameter to the input URL.
        /// </summary>
        /// <param name="url">The URL to append to</param>
        /// <param name="key">The key of the query parameter</param>
        /// <param name="value">The value of the query parameter</param>
        /// <returns>The URL with the appended query parameter</returns>
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

        /// <summary>
        ///  Internal method for emitting the event from a platform-specific SSO manager at the end of the flow.
        /// </summary>
        /// <param name="args">The arguments containing the result</param>
        protected static void OnAuthorizationFinished(AuthorizationEventArgs args) {
            AuthorizationEventHandler handler = AuthorizationFinished;
            if (handler != null) {
                // Emits the event
                handler(null, args);
            }
        }
    }   
}
