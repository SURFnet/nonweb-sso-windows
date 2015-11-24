// SSO Manager
// Base class for handling SSO authentication
(function () {
    "use strict";

    WinJS.Namespace.define("Surfnet.Nonweb.Sso", {
        SSOManager: WinJS.Class.define(
            function init() {
            },
            {
                getCurrentApplicationCallbackUri: function() {
                    return Windows.Security.Authentication.Web.WebAuthenticationBroker.getCurrentApplicationCallbackUri().absoluteUri;
                },
                authenticate: function (clientIdPhone, clientIdTabletAndDesktop, endpoint, callbackFunction) {
                    // Phone and tablet need to have a different client_id
                    var clientId = (WinJS.Utilities.isPhone) ? clientIdPhone : clientIdTabletAndDesktop;
                    var urlString = endpoint + "?client_id=" + clientId  + "&response_type=token&state=demo&scope=authorize";
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
                                    callbackFunction(true, result.responseErrorDetail);
                                } else {
                                    callbackFunction(false, token);
                                }
                            }, function (err) {
                                callbackFunction(true, err);
                            });
                    }
                    catch (err) {
                        // Windows Phone 8.1 will throw a Not Implemented exception when you call authenticateAsync. Use authenticateAndContinue instead
                        // continuation is handled in the activation handler
                        Surfnet.Nonweb.Sso.SSOManager._callbackFunction = callbackFunction;
                        var endURI = new Windows.Foundation.Uri(Windows.Security.Authentication.Web.WebAuthenticationBroker.getCurrentApplicationCallbackUri().absoluteUri);
                        Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAndContinue(startURI);
                    }
                },
                onAppActivated: function (args, activation) {
                    if (args.detail.kind == activation.ActivationKind.webAuthenticationBrokerContinuation) {
                        //take oauth response and continue login process on Windows Phone 8.1
                        var token = "";
                        var responseData = args.detail.webAuthenticationResult.responseData.replace("#access_token", "?access_token");
                        console.log(args.detail.webAuthenticationResult.responseStatus);
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
                        if (args.detail.webAuthenticationResult.responseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.success) {
                            Surfnet.Nonweb.Sso.SSOManager._callbackFunction(false, token);
                        } else {
                            Surfnet.Nonweb.Sso.SSOManager._callbackFunction(true, args.detail.webAuthenticationResult.responseErrorDetail);
                        }
                        return true;
                    }
                    return false;
                }
            },
            {
                _instance: null,
                _callbackFunction: null,
                _getInstance: function() {
                    if (Surfnet.Nonweb.Sso.SSOManager._instance == null) {
                        Surfnet.Nonweb.Sso.SSOManager._instance = new Surfnet.Nonweb.Sso.SSOManager();
                    }
                    return Surfnet.Nonweb.Sso.SSOManager._instance;
                }
            }
        )
    });
})();