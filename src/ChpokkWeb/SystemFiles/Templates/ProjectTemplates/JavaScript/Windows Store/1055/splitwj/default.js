// Bölünmüş şablona giriş için aşağıdaki belgelere bakın:
// http://go.microsoft.com/fwlink/?LinkID=232447
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;

    app.addEventListener("activated", function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: Bu uygulama yeni başlatıldı. Başlat
                // uygulamanız burada.
            } else {
                // TODO: Bu uygulama askı durumundan etkinleştirildi.
                // geri yükleme uygulama durumu burada.
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
        // TODO: Bu uygulama askıya alınma hakkında. Herhangi bir durumda kaydet
        // burada askıya alınmalar arasında devam etmesi gerekiyor. Uygulamanız 
        // askıya alınmadan önce zaman uyumsuz bir işlemi tamamlamanız 
        // gerekiyorsa, args.setPromise() değerini çağırın.
        app.sessionState.history = nav.history;
    };

    app.start();
})();
