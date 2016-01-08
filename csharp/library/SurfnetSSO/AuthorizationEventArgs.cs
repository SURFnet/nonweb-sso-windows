using System;

namespace SurfnetSSO {
    public class AuthorizationEventArgs : EventArgs {

        public bool IsSuccessful { get; set; }
        public string Token { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public static AuthorizationEventArgs Success(string authToken) {
            AuthorizationEventArgs args = new AuthorizationEventArgs();
            args.Token = authToken;
            args.IsSuccessful = true;
            return args;
        }

        public static AuthorizationEventArgs Error(string errorType, string errorMessage) {
            AuthorizationEventArgs args = new AuthorizationEventArgs();
            args.ErrorType = errorType;
            args.ErrorMessage = errorMessage;
            args.IsSuccessful = false;
            return args;
        }
    }
}
