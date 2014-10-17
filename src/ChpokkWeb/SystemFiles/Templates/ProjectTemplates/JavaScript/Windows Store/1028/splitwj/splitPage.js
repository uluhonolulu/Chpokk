(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    ui.Pages.define("/pages/split/split.html", {

        /// <field type="WinJS.Binding.List" />
        _items: null,
        _group: null,
        _itemSelectionIndex: -1,

        // 每當使用者巡覽至此頁面時，就會呼叫這個函式。它
        // 會將應用程式的資料填入頁面項目。
        ready: function (element, options) {
            var listView = element.querySelector(".itemlist").winControl;

            // 儲存此頁面將顯示之群組和選取範圍的
            // 相關資訊。
            this._group = (options && options.groupKey) ? Data.resolveGroupReference(options.groupKey) : Data.groups.getAt(0);
            this._items = Data.getItemsFromGroup(this._group);
            this._itemSelectionIndex = (options && "selectedIndex" in options) ? options.selectedIndex : -1;

            element.querySelector("header[role=banner] .pagetitle").textContent = this._group.title;

            // 設定 ListView。
            listView.itemDataSource = this._items.dataSource;
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.onselectionchanged = this._selectionChanged.bind(this);
            listView.layout = new ui.ListLayout();

            this._updateVisibility();
            if (this._isSingleColumn()) {
                if (this._itemSelectionIndex >= 0) {
                    // 如需單一資料行詳細資料檢視，請載入此發行項。
                    binding.processAll(element.querySelector(".articlesection"), this._items.getAt(this._itemSelectionIndex));
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/split/split.html") {
                    // 清除上一頁堆疊以處理貼齊、巡覽
                    // 離開、取消貼齊，然後返回此頁面的使用者。
                    nav.history.backStack.pop();
                }
                // 如果此頁面具有 selectionIndex，請確定選取範圍
                // 顯示在 ListView 中。
                listView.selection.set(Math.max(this._itemSelectionIndex, 0));
            }
        },

        unload: function () {
            this._items.dispose();
        },

        // 此函式會更新頁面配置以回應 viewState 變更。
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            var listView = element.querySelector(".itemlist").winControl;
            var firstVisible = listView.indexOfFirstVisible;
            this._updateVisibility();

            var handler = function (e) {
                listView.removeEventListener("contentanimating", handler, false);
                e.preventDefault();
            }

            if (this._isSingleColumn()) {
                listView.selection.clear();
                if (this._itemSelectionIndex >= 0) {
                    // 如果應用程式已經貼齊至單一資料行詳細資料檢視中，
                    // 請將單一資料行清單檢視加入至上一頁堆疊。
                    nav.history.current.state = {
                        groupKey: this._group.key,
                        selectedIndex: this._itemSelectionIndex
                    };
                    nav.history.backStack.push({
                        location: "/pages/split/split.html",
                        state: { groupKey: this._group.key }
                    });
                    element.querySelector(".articlesection").focus();
                } else {
                    listView.addEventListener("contentanimating", handler, false);
                    if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                        listView.indexOfFirstVisible = firstVisible;
                    }
                    listView.forceLayout();
                }
            } else {
                // 如果應用程式已經取消貼齊至兩個資料行檢視中，請移除
                // 已加入至上一頁堆疊的任何 splitPage 執行個體。
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/split/split.html") {
                    nav.history.backStack.pop();
                }
                if (viewState !== lastViewState) {
                    listView.addEventListener("contentanimating", handler, false);
                    if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                        listView.indexOfFirstVisible = firstVisible;
                    }
                    listView.forceLayout();
                }

                listView.selection.set(this._itemSelectionIndex >= 0 ? this._itemSelectionIndex : Math.max(firstVisible, 0));
            }
        },

        // 此函式會檢查清單和詳細資料資料行是否應該顯示
        // 在個別頁面上，而非並存顯示。
        _isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        _selectionChanged: function (args) {
            var listView = document.body.querySelector(".itemlist").winControl;
            var details;
            // 根據預設，選取範圍限制為單一項目。
            listView.selection.getItems().done(function updateDetails(items) {
                if (items.length > 0) {
                    this._itemSelectionIndex = items[0].index;
                    if (this._isSingleColumn()) {
                        // 如果為貼齊或縱向，請巡覽至包含選取項目之詳細資料的
                        // 新頁面。
                        nav.navigate("/pages/split/split.html", { groupKey: this._group.key, selectedIndex: this._itemSelectionIndex });
                    } else {
                        // 如果為全螢幕或填滿，請使用新資料來更新詳細資料資料行。
                        details = document.querySelector(".articlesection");
                        binding.processAll(details, items[0].data);
                        details.scrollTop = 0;
                    }
                }
            }.bind(this));
        },

        // 此函式會根據目前的檢視狀態和項目選取範圍，切換
        // 兩個資料行的可視性。
        _updateVisibility: function () {
            var oldPrimary = document.querySelector(".primarycolumn");
            if (oldPrimary) {
                utils.removeClass(oldPrimary, "primarycolumn");
            }
            if (this._isSingleColumn()) {
                if (this._itemSelectionIndex >= 0) {
                    utils.addClass(document.querySelector(".articlesection"), "primarycolumn");
                    document.querySelector(".articlesection").focus();
                } else {
                    utils.addClass(document.querySelector(".itemlistsection"), "primarycolumn");
                    document.querySelector(".itemlist").focus();
                }
            } else {
                document.querySelector(".itemlist").focus();
            }
        }
    });
})();
