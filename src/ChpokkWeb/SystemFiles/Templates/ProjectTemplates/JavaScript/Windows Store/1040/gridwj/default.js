// Per un'introduzione a modello griglia, vedere la seguente documentazione:
// http://go.microsoft.com/fwlink/?LinkID=232446
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;

    app.addEventListener("activated", function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: questa applicazione è stata appena avviata. Inizializzare
                // l'applicazione qui.
            } else {
                // TODO: questa applicazione è stata riattivata dalla sospensione.
                // Ripristinare lo stato dell'applicazione qui.
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
        // TODO: questa applicazione sta per essere sospesa. Salvare qui eventuali stati
        // che devono persistere attraverso le sospensioni. Se è necessario 
        // completare un'operazione asincrona prima che l'applicazione 
        // venga sospesa, chiamare args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();
