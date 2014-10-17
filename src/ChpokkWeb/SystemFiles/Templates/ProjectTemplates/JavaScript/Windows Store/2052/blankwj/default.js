// 有关“空白”模板的简介，请参阅以下文档:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: 此应用程序刚刚启动。在此处初始化
                //您的应用程序。
            } else {
                // TODO: 此应用程序已从挂起状态重新激活。
                // 在此处恢复应用程序状态。
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: 即将挂起此应用程序。在此处保存
        //需要在挂起中保留的任何状态。您可以使用
        // WinJS.Application.sessionState 对象，该对象将在
        //挂起中自动保存和恢复。如果您需要在
        //挂起应用程序之前完成异步操作，请调用
        // args.setPromise()。
    };

    app.start();
})();
