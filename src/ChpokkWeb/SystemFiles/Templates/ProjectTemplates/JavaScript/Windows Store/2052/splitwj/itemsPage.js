(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/items/items.html", {
        // 每当用户导航至此页面时都要调用此功能。它
        // 使用应用程序的数据填充页面元素。
        ready: function (element, options) {
            var listView = element.querySelector(".itemslist").winControl;
            listView.itemDataSource = Data.groups.dataSource;
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.oniteminvoked = this._itemInvoked.bind(this);

            this._initializeLayout(listView, Windows.UI.ViewManagement.ApplicationView.value);
            listView.element.focus();
        },

        // 此功能更新页面布局以响应 viewState 更改。
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            var listView = element.querySelector(".itemslist").winControl;
            if (lastViewState !== viewState) {
                if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    }
                    listView.addEventListener("contentanimating", handler, false);
                    var firstVisible = listView.indexOfFirstVisible;
                    this._initializeLayout(listView, viewState);
                    if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                        listView.indexOfFirstVisible = firstVisible;
                    }
                }
            }
        },

        // 此函数将使用新布局更新 ListView
        _initializeLayout: function (listView, viewState) {
            /// <param name="listView" value="WinJS.UI.ListView.prototype" />

            if (viewState === appViewState.snapped) {
                listView.layout = new ui.ListLayout();
            } else {
                listView.layout = new ui.GridLayout();
            }
        },

        _itemInvoked: function (args) {
            var groupKey = Data.groups.getAt(args.detail.itemIndex).key;
            WinJS.Navigation.navigate("/pages/split/split.html", { groupKey: groupKey });
        }
    });
})();
