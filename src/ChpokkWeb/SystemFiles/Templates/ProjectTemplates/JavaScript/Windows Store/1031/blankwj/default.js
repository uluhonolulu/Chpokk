// Eine Einführung zur leeren Vorlage finden Sie in der folgenden Dokumentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: Diese Anwendung wurde neu eingeführt. Die Anwendung
                // hier initialisieren.
            } else {
                // TODO: Diese Anwendung war angehalten und wurde reaktiviert.
                // Anwendungszustand hier wiederherstellen.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: Diese Anwendung wird gleich angehalten. Jeden Zustand,
        // der über Anhaltevorgänge hinweg beibehalten muss, hier speichern. Dazu kann das
        // WinJS.Application.sessionState-Objekt verwendet werden, das automatisch
        // über ein Anhalten hinweg gespeichert und wiederhergestellt wird. Wenn ein asynchroner
        // Vorgang vor dem Anhalten der Anwendung abgeschlossen werden muss,
        // args.setPromise() aufrufen.
    };

    app.start();
})();
