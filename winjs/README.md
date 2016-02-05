SURFnet SSO Authentication library for Windows (WinJS)
------------------------------------------------------

### Integration in a WinJS app

1. Copy the file [SSOManager.js](library/js/sso/SSOManager.js) inside your WinJS project (preferably inside the *Shared* project, if you are targeting multiple platforms, and intend to use SSO on all of them).
2. Don't forget to add the file inside your Visual Studio project after copying the file in the folder. Right-click on the project, `Add -> Existing item...` and navigate to where you added the file and select it. It should appear in *the Solution Explorer* now.
3. Now you have to register your application as an SSO client at the identity provider configuration. This will give you the ClientID's you need in step 5. For this you need the callback URL of your application, which you can find out by checking the return value of `Surfnet.Nonweb.Sso.SSOManager._getInstance().getCurrentApplicationCallbackUri()` (this can be done using the Javascript Console when running the app). Note that the phone and tablet/desktop application are separate applications, and have a different URL, so they need different client IDs too (so you have to register 2 clients). the URL should look something like `ms-app://s-1-15-2-1668544952-1132098162-3287153653-926875653-937996570-661899771-2544785182/`
4. If the registration is done, let's finish with integrating the SDK. In your `default.js`, find the clause `app.onactivated = function (args) {...}` and add a few lines of code so it looks like this:
   
    ```javascript
    app.onactivated = function (args) {
        // This was added to catch the app activation on phone
        if (Surfnet.Nonweb.Sso.SSOManager._getInstance().onAppActivated(args, activation)) {
            return;
        }
        // ... other checks
    }
    ```
    This is only needed when you are also targeting Windows Phone.

    Secondly, in your `default.html` include the `SSOManager.js` file, for example like:

        <script src="/js/SSOManager.js"></script>
    

5. Last thing remaining is adding the call for starting the SSO authentication. For this you need to use:
    
    ```javascript
    Surfnet.Nonweb.Sso.SSOManager._getInstance().authenticate(clientIdPhone, clientIdTabletAndDesktop, endpoint, callbackFunction)
    ```
    So for example:
    ```javascript
    startAuthentication: function() {
        Surfnet.Nonweb.Sso.SSOManager._getInstance().authenticate("5dcbbc877e9955e3b29d7ca0baa4c7c7", "5dcbbc877e9955e3b29d7ca0baa4c7c6", "https://nonweb.demo.surfconext.nl/php-oauth-as/authorize.php", function (isError, message) {
            console.log("IsError: " + isError);
            console.log("Message: " + message);
        });
}
    ```
    The callback function will be executed when the authentication flow has come to an end.
    If the parameter `isError` is true, then the `message` parameter contains the error message.
    If it is false, the `message` parameter will contain the OAuth2.0 authentication token.
