// Pro úvod do šablony Mřížky, viz následující dokumentace:
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
                // TODO: Tato aplikace byla nově spuštěna. Inicializovat
                // zde aplikaci.
            } else {
                // TODO: Tato aplikace byla znovu aktivována z pozastavení.
                // Obnovit zde stav aplikace.
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
        // TODO: Tato aplikace bude pozastavena. Uložit všechny stavy,
        // které jsou potřeba k přetrvání pozastavení. Pokud potřebujete 
        // dokončit asynchronní operaci předtím, než je aplikace 
        // pozastavena, zavolejte args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();
