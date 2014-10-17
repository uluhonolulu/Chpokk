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

        // この関数は、ユーザーがこのページに移動するたびに呼び出されます。
        // ページ要素にアプリケーションのデータを設定します。
        ready: function (element, options) {
            var listView = element.querySelector(".itemlist").winControl;

            // このページに表示するグループおよび選択に関する情報を格納
            // します。
            this._group = (options && options.groupKey) ? Data.resolveGroupReference(options.groupKey) : Data.groups.getAt(0);
            this._items = Data.getItemsFromGroup(this._group);
            this._itemSelectionIndex = (options && "selectedIndex" in options) ? options.selectedIndex : -1;

            element.querySelector("header[role=banner] .pagetitle").textContent = this._group.title;

            // ListView を設定します。
            listView.itemDataSource = this._items.dataSource;
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.onselectionchanged = this._selectionChanged.bind(this);
            listView.layout = new ui.ListLayout();

            this._updateVisibility();
            if (this._isSingleColumn()) {
                if (this._itemSelectionIndex >= 0) {
                    // 単一列の詳細ビューの場合、アーティクルを読み込みます。
                    binding.processAll(element.querySelector(".articlesection"), this._items.getAt(this._itemSelectionIndex));
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/split/split.html") {
                    // バックスタックをクリーンアップして、ユーザーによる位置合わせ、移動、
                    // 位置合わせ解除、およびこのページに戻る操作を処理します。
                    nav.history.backStack.pop();
                }
                // このページに selectionIndex がある場合、その選択を
                // ListView に表示します。
                listView.selection.set(Math.max(this._itemSelectionIndex, 0));
            }
        },

        unload: function () {
            this._items.dispose();
        },

        // この関数は、viewState の変更に応じてページ レイアウトを更新します。
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
                    // アプリケーションが単一列の詳細ビューに位置合わせされている場合、
                    // 単一列のリスト ビューをバックスタックに追加します。
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
                // アプリケーションが 2 列のビューに位置合わせ解除されている場合、
                // バックスタックに追加された splitPage インスタンスを削除します。
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

        // この関数は、リストおよび詳細列を左右に並べて表示するのではなく、
        // 個別のページに表示するかどうかを確認します。
        _isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        _selectionChanged: function (args) {
            var listView = document.body.querySelector(".itemlist").winControl;
            var details;
            // 既定では、選択は単一項目に制限されます。
            listView.selection.getItems().done(function updateDetails(items) {
                if (items.length > 0) {
                    this._itemSelectionIndex = items[0].index;
                    if (this._isSingleColumn()) {
                        // 位置合わせされているか縦方向の場合、選択した項目の詳細が格納された
                        // 新しいページに移動します。
                        nav.navigate("/pages/split/split.html", { groupKey: this._group.key, selectedIndex: this._itemSelectionIndex });
                    } else {
                        // 全画面表示またはページ幅に合わせた表示の場合、詳細列を新しいデータで更新します。
                        details = document.querySelector(".articlesection");
                        binding.processAll(details, items[0].data);
                        details.scrollTop = 0;
                    }
                }
            }.bind(this));
        },

        // この関数は、現在のビューステートおよび項目選択に基づいて 2 列の
        // 表示を切り替えます。
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
