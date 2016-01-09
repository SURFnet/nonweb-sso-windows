(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;
    var sched = WinJS.Utilities.Scheduler;
    var ui = WinJS.UI;


    app.onactivated = function (args) {
        // This was added to catch the app activation on phone
        if (Surfnet.Nonweb.Sso.SSOManager._getInstance().onAppActivated(args, activation)) {
            return;
        }
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application was suspended and then terminated.
                // To create a smooth user experience, restore application state here so that it looks like the app never stopped running.
            }
            args.setPromise(WinJS.UI.processAll());
        }
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            nav.history = app.sessionState.history || {};
            nav.history.current.initialPlaceholder = true;

            // Optimize the load of the application and while the splash screen is shown, execute high priority scheduled work.
            ui.disableAnimations();
            var p = ui.processAll().then(function () {
                return nav.navigate(nav.location || Application.navigator.home, nav.state);
            }).then(function () {
                return sched.requestDrain(sched.Priority.aboveNormal + 1);
            }).then(function () {
                ui.enableAnimations();
            });

            args.setPromise(p);
        }

    };

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
    };

    /** If you wish to catch errors and display them in a dialog, uncomment this
    app.onerror = function (e) {
        var dialog = new Windows.UI.Popups.MessageDialog(e.detail.stack, e.detail.message);
        dialog.showAsync().done();
        return true;
    };
    **/

    app.start();
})();