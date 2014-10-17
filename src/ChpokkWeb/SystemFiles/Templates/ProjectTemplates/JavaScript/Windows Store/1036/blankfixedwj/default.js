// Pour obtenir une présentation du modèle Disposition fixe, consultez la documentation suivante :
// http://go.microsoft.com/fwlink/?LinkId=232508
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: cette application vient d'être lancée. Initialisez
                // votre application ici.
            } else {
                // TODO: cette application a été réactivée après avoir été suspendue.
                // Restaurez l'état de l'application ici.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: cette application est sur le point d'être suspendue. Enregistrez tout état
        // devant être conservé lors des suspensions ici. Vous pouvez utiliser l'objet
        // WinJS.Application.sessionState, qui est automatiquement
        // enregistré et restauré en cas de suspension. Si vous devez effectuer une
        // opération asynchrone avant la suspension de votre application, appelez
        // args.setPromise().
    };

    app.start();
})();
