// Per un'introduzione a modello vuoto, vedere la seguente documentazione:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: questa applicazione è stata appena avviata. Inizializzare
                // l'applicazione qui.
            } else {
                // TODO: questa applicazione è stata riattivata dalla sospensione.
                // Ripristinare lo stato dell'applicazione qui.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: questa applicazione sta per essere sospesa. Salvare qui eventuali stati
        // che devono persistere attraverso le sospensioni. È possibile utilizzare l'oggetto
        // WinJS.Application.sessionState, che viene automaticamente
        // salvato e ripristinato in seguito a sospensioni. Se è necessario eseguire
        // un'operazione asincrona prima che l'applicazione venga sospesa, chiamare
        // args.setPromise().
    };

    app.start();
})();
