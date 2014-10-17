// Aby obejrzeć wprowadzenie do szablonu nawigacji, zobacz następującą dokumentację:
// http://go.microsoft.com/fwlink/?LinkId=232506
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;

    app.addEventListener("activated", function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: Ta aplikacja została niedawno uruchomiona. Zainicjuj
                // aplikację tutaj.
            } else {
                // TODO: Ta aplikacja została reaktywowana z zawieszenia.
                // Przywróć tutaj stan aplikacji.
            }

            if (app.sessionState.history) {
                nav.history = app.sessionState.history;
            }
            args.setPromise(WinJS.UI.processAll().then(function () {
                if (nav.location) {
                    nav.history.current.initialPlaceholder = true;
                    return nav.navigate(nav.location, nav.state);
                } else {
                    return nav.navigate(Application.navigator.home);
                }
            }));
        }
    });

    app.oncheckpoint = function (args) {
        // TODO: Ta aplikacja zostanie zawieszona. Zapisz tutaj stan,
        // który musi być zachowany po zawieszeniu. Jeśli musisz 
        // zakończyć operację asynchroniczną, zanim aplikacja zostanie 
        // suspended, call args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();
