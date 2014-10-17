// Pour obtenir une présentation du modèle Navigation, consultez la documentation suivante :
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
                // TODO: cette application vient d'être lancée. Initialisez
                // votre application ici.
            } else {
                // TODO: cette application a été réactivée après avoir été suspendue.
                // Restaurez l'état de l'application ici.
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
        // TODO: cette application est sur le point d'être suspendue. Enregistrez tout état
        // devant être conservé lors des suspensions ici. Si vous devez 
        // effectuer une opération asynchrone avant la suspension de 
        // l'application, appelez args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();
