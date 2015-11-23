(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/main.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var startButton = element.querySelector("#startButton");
            startButton.addEventListener("click", function () {
                Surfnet.Nonweb.Sso.SSOManager._getInstance().authenticate("5dcbbc877e9955e3b29d7ca0baa4c7c7", "5dcbbc877e9955e3b29d7ca0baa4c7c6", "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php", function (isError, message) {
                    console.log("IsError: " + isError);
                    console.log("Message: " + message);
                });
                
            });
        },

    });
})();