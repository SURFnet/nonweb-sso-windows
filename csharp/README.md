SURFnet SSO Authentication library for Windows (C#)
------------------------------------------------------

### About the library

Lately there have been a lot of different API sets if you wanted to develop for multiple Windows operating systems.
Luckily Microsoft is moving slowly into UWP - Universal Windows Platform, but there is still a huge part of users which are still on older APIs - .NET or Windows Phone.
This library is an attempt to make an OAuth2 client for all these platforms. Since the APIs differ, we can't make one library for all, but we can share some common code in a PCL, and write the platform-dependent code in an extension.
The samples in this repository cover the usage of the library, but here's also some step-by-step documentation for each platform.

### Adding the library
Select the `library/SurfnetSSO` directory and the `library/SurfnetSSO.Platform.<YOUR PLATFORM>`. Copy these into your source control. Now right-click on your project in Solution Explorer, Add, Reference... and select the two projects you just copied. Now you can reference the SSO manager of your platform.
For all platforms: Before you call the `Authorize` method, subscribe to the `AuthorizationFinished` event of the `SSOManager` to receive the result:
    
```cs
...
SSOManager.AuthorizationFinished += SSOManager_AuthorizationFinished;
// Start the auth flow now - see platform dependent code
...

private void SSOManager_AuthorizationFinished(object sender, AuthorizationEventArgs e) {
    if (e.IsSuccessful) {
        // Do something on success
    } else {
        // Do someting else on failure
    }
}
```

### .NET 4.5 - Windows Forms
Since Windows can not intercept URLs in the (default) browser, you need to add a `WebBrowser` control to your app. To start the OAuth2 flow, call:
    
```cs
WinFormsSSOManager.Authorize(webBrowser, consumerId, endpoint, callbackUrl);
```
Note that you can't use a callback URL which does NOT exist - for more information, see [this blog post](http://davidquail.com/2010/01/10/c-webbrowser-swallows-redirect-uri/).


### Universal Windows Platform
To start the OAuth2 flow:
    
```cs
UWPSSOManager.Authorize(consumerId, endpoint, callbackUrl);
```
The `AuthorizationFinished` event will be sent when the flow is finished.

### Windows Phone 8
Almost the same as UWP, with an extra addition.
To start the OAuth2 flow:
    
```cs
WP8SSOManager.Authorize(consumerId, endpoint, callbackUrl);
```
In `App.xaml.cs`, add the following method:
    
```cs
protected override void OnActivated(IActivatedEventArgs args) {
    base.OnActivated(args);
    var authorizationArgs = WP8SSOManager.OnAppActivated(args);
    if (authorizationArgs == null) {
        // Handle other activation events here, if you have any
    }
}
```
This will handle the continuation of the flow for you.
The AuthorizationFinished event will be sent when the flow is finished.
