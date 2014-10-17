// Aby obejrzeć wprowadzenie do szablonu stałego układu, zobacz następującą dokumentację:
// http://go.microsoft.com/fwlink/?LinkId=232508
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: Ta aplikacja została niedawno uruchomiona. Zainicjuj
                // aplikację tutaj.
            } else {
                // TODO: Ta aplikacja została reaktywowana z zawieszenia.
                // Przywróć tutaj stan aplikacji.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: Ta aplikacja zostanie zawieszona. Zapisz tutaj stan,
        // który musi być zachowany po zawieszeniu. Można użyć
        // obiektu WinJS.Application.sessionState, który jest automatycznie
        // zapisywany i przywracany po zawieszeniu. Jeśli musisz wykonać
        // asynchroniczną operację zanim aplikacja zostanie zawieszona, wywołaj
        // args.setPromise().
    };

    app.start();
})();
