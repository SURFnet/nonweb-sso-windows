using System;

namespace SurfnetSSO {
    public class AuthorizationEventArgs : EventArgs {

        public bool IsSuccessful { get; set; }
        public String Token { get; set; }
        public String ErrorMessage { get; set; }

        public static AuthorizationEventArgs Success(String authToken) {
            AuthorizationEventArgs args = new AuthorizationEventArgs();
            args.Token = authToken;
            args.IsSuccessful = true;
            return args;
        }

        public static AuthorizationEventArgs Error(String errorMessage) {
            AuthorizationEventArgs args = new AuthorizationEventArgs();
            args.ErrorMessage = errorMessage;
            args.IsSuccessful = false;
            return args;
        }
    }
}
