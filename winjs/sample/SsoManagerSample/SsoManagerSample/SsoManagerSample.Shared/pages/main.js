(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/main.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var startButton = element.querySelector("#startButton");
            startButton.addEventListener("click", function () {
                Surfnet.Nonweb.Sso.SSOManager._getInstance().authenticate("5dcbbc877e9955e3b29d7ca0baa4c7c7", "5dcbbc877e9955e3b29d7ca0baa4c7c6", "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php", function (isError, message) {
                    // Just displaying the values in HTML.
                    // Normally you would persist the token now and authenticate the calls with it.
                    var ssoIsError = element.querySelector("#ssoIsError");
                    var ssoMessage = element.querySelector("#ssoMessage");
                    ssoIsError.textContent = isError;
                    ssoMessage.textContent = message;
                });
                
            });
        },

    });
})();