// 如需固定配置範本的簡介，請參閱下列文件:
// http://go.microsoft.com/fwlink/?LinkId=232508
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: 這個應用程式剛啟動。請在這裡初始化
                // 您的應用程式。
            } else {
                // TODO: 這個應用程式已經從擱置重新啟用。
                // 請在這裡還原應用程式狀態。
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: 這個應用程式即將暫停。請在這裡儲存任何
        // 需要在擱置間保存的狀態。您可以使用
        // WinJS.Application.sessionState 物件，這個物件會自動
        // 在擱置間儲存並還原。如果您需要在
        // 應用程式暫停之前完成非同步作業，請呼叫
        // args.setPromise()。
    };

    app.start();
})();
