// SSO Manager
// Base class for handling SSO authentication
(function () {
    "use strict";

    WinJS.Namespace.define("Surfnet.Nonweb.Sso", {
        SSOManager: WinJS.Class.define(
            function init() {
            },
            {
                authenticate: function (clientIdPhone, clientIdTabletAndDesktop, endpoint, callbackFunction) {
                    // Phone and tablet need to have a different client_id
                    var clientId = (WinJS.Utilities.isPhone) ? clientIdPhone : clientIdTabletAndDesktop;
                    var urlString = constants.AUTH_URL + "?client_id=" + clientId  + "&response_type=" + constants.RESPONSE_TYPE + "&state=" + constants.STATE + "&scope=" + constants.SCOPE;
                    var startURI = new Windows.Foundation.Uri(urlString);
                    try {
                        // Windows 8/8.1
                        Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAsync(
                            Windows.Security.Authentication.Web.WebAuthenticationOptions.none, startURI)
                            .done(function (result) {
                                var token = "";
                                var responseData = result.responseData.replace("#access_token", "?access_token");
                                var URI = new Windows.Foundation.Uri(responseData);
                                var queryParams = URI.queryParsed;
                                if (queryParams.length > 0) {
                                    token = queryParams[0].value;
                                }
                                if (result.responseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.errorHttp) {
                                    callbackFunction.err(result.responseErrorDetail);
                                } else {
                                    callbackFunction.success(token);
                                }
                            }, function (err) {
                                callbackFunction.err(err);
                            });
                    }
                    catch (err) {
                        // Windows Phone 8.1 will throw a Not Implemented exception when you call authenticateAsync. Use authenticateAndContinue instead
                        // continuation is handled in the activation handler
                        SSOManager.callbackFunction = callbackFunction;
                        var endURI = new Windows.Foundation.Uri(Windows.Security.Authentication.Web.WebAuthenticationBroker.getCurrentApplicationCallbackUri().absoluteUri);
                        Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAndContinue(startURI);
                    }
                },
                onAppActivated: function(app, activation, navigation) {
                    if (args.detail.kind == activation.ActivationKind.webAuthenticationBrokerContinuation) {
                        //take oauth response and continue login process on Windows Phone 8.1
                        var token = "";
                        var responseData = args.detail.webAuthenticationResult.responseData.replace("#access_token", "?access_token");
                        if (responseData.length > 0) {
                            var URI = new Windows.Foundation.Uri(responseData);
                            var queryParams = URI.queryParsed;
                            // FIXME: this is actually an ugly way to retrieve the access token, and not safe at all.
                            // best way would be to iterate over the params to retrieve the correct token, but since this is
                            // for demo purposes only, we use this way.
                            if (queryParams.length > 0) {
                                token = queryParams[0].value;
                            }
                        }

                        if (args.detail.webAuthenticationResult.responseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.errorHttp) {
                            SSOManager.callbackFunction.err(args.detail.webAuthenticationResult.responseErrorDetail);
                        } else {
                            SSOManager.callbackFunction.success(token);
                        }
                        return true;
                    }
                    return false;
                }
            },
            {
                _instance = null,
                _getInstance: function() {
                    if (_instance == null) {
                        _instance = new SSOManager();
                    }
                    return _instance;
                }
            },
        );
    });
})();