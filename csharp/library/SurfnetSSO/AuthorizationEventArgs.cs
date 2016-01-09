using System;

namespace SurfnetSSO {
    /// <summary>
    /// The authorization event arguments containing the result of the authorization flow.
    /// </summary>
    public class AuthorizationEventArgs : EventArgs {

        /// <summary>
        /// True if we have a token, false if there was an error
        /// </summary>
        public bool IsSuccessful { get; set; }
        /// <summary>
        /// The OAuth2 access token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The error type
        /// </summary>
        public string ErrorType { get; set; }
        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Factory method for creating an object for a successful flow.
        /// </summary>
        /// <param name="authToken">The OAuth2 access token</param>
        /// <returns>The instance containing the access token</returns>
        public static AuthorizationEventArgs Success(string authToken) {
            AuthorizationEventArgs args = new AuthorizationEventArgs();
            args.Token = authToken;
            args.IsSuccessful = true;
            return args;
        }

        /// <summary>
        /// /// Factory method for creating an object for a failed flow.
        /// </summary>
        /// <param name="errorType">The type of error</param>
        /// <param name="errorMessage">The error details</param>
        /// <returns>The instance containing the error type and details</returns>
        public static AuthorizationEventArgs Error(string errorType, string errorMessage) {
            AuthorizationEventArgs args = new AuthorizationEventArgs();
            args.ErrorType = errorType;
            args.ErrorMessage = errorMessage;
            args.IsSuccessful = false;
            return args;
        }
    }
}
