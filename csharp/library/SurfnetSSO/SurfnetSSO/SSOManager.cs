using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SurfnetSSO {
    public class SSOManager {

        /// <summary>
        /// Starts the authorization flow against the server.
        /// </summary>
        /// <param name="consumerId">The consumer ID of this application</param>
        /// <param name="endpoint">The URL of the server</param>
        /// <param name="endpoint">The path relative to the URL which points to the authentication page</param>
        /// <param name="scheme">The callback scheme which will be invoked by the server at the end of the flow</param>
        public static void authorize(String consumerId, String endpoint, String authPath, String scheme) {
            if (String.IsNullOrEmpty(consumerId)) {
                throw new ArgumentNullException("Consumer ID can not be empty!");
            }
            if (String.IsNullOrEmpty(endpoint)) {
                throw new ArgumentNullException("Endpoint can not be empty!");
            }
            if (String.IsNullOrEmpty(authPath)) {
                throw new ArgumentNullException("Authentication path can not be empty!");
            }
            if (String.IsNullOrEmpty(scheme)) {
                throw new ArgumentNullException("Scheme can not be empty!");
            }
            var client = new RestClient(new Uri(endpoint)); // For example: "https://accounts.google.com"
            var authRequest = new RestRequest(authPath, Method.POST);  // Example: "/o/oauth2/device/code"
            authRequest.AddParameter("client_id", consumerId);
            authRequest.AddParameter("response_type", "token");
            authRequest.AddParameter("state", "surfnet");
            authRequest.AddParameter("scope", "authorize");

            // TODO Call web launcher, catch callback.
        }
    }

    
}
