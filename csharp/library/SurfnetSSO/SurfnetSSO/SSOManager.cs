using System;

namespace SurfnetSSO {
    public class SSOManager {

        public delegate void AuthorizationEventHandler(object sender, AuthorizationEventArgs e);

        public static event AuthorizationEventHandler AuthorizationFinished;

        /// <summary>
        /// Starts the authorization flow against the server.
        /// </summary>
        /// <param name="consumerId">The consumer ID of this application</param>
        /// <param name="endpoint">The URL of the server</param>
        /// <param name="endpoint">The path relative to the URL which points to the authentication page</param>
        /// <param name="scheme">The callback scheme which will be invoked by the server at the end of the flow</param>
        public static Uri authorize(String consumerId, String endpoint, String scheme) {
            if (String.IsNullOrEmpty(consumerId)) {
                throw new ArgumentNullException("Consumer ID can not be empty!");
            }
            if (String.IsNullOrEmpty(endpoint)) {
                throw new ArgumentNullException("Endpoint can not be empty!");
            }
            if (String.IsNullOrEmpty(scheme)) {
                throw new ArgumentNullException("Scheme can not be empty!");
            }
            String url = _addQueryParameter(endpoint, "client_id", consumerId);
            url = _addQueryParameter(url, "response_type", "token");
            url = _addQueryParameter(url, "state", "surfnet");
            url = _addQueryParameter(url, "scope", "authorize");
            return new Uri(url, UriKind.Absolute);
        }

        public static bool isCallbackUrl(Uri uri, String scheme) {
            return uri.Scheme.Equals(scheme);
        }

        private static String _addQueryParameter(String url, String key, String value) {
            String result = url;
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
