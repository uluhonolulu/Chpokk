// Pro úvod do Prázdné šablony, viz následující dokumentace:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: Tato aplikace byla nově spuštěna. Inicializovat
                // zde aplikaci.
            } else {
                // TODO: Tato aplikace byla znovu aktivována z pozastavení.
                // Obnovit zde stav aplikace.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: Tato aplikace bude pozastavena. Uložit všechny stavy,
        // které jsou potřeba k přetrvání pozastavení. Můžete použít
        // objekt WinJS.Application.sessionState, který je automaticky
        // uložen a obnoven při pozastavení. Pokud je nutné provést
        // asynchronní operace předtím, než je aplikace pozastavena, zavolejte
        // args.setPromise().
    };

    app.start();
})();
