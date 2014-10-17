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

        // 이 함수는 사용자가 이 페이지로 이동할 때마다 호출되어
        // 페이지 요소를 응용 프로그램 데이터로 채웁니다.
        ready: function (element, options) {
            var listView = element.querySelector(".itemlist").winControl;

            // 이 페이지에 표시될 그룹 및 선택 사항에 대한 정보를
            // 저장합니다.
            this._group = (options && options.groupKey) ? Data.resolveGroupReference(options.groupKey) : Data.groups.getAt(0);
            this._items = Data.getItemsFromGroup(this._group);
            this._itemSelectionIndex = (options && "selectedIndex" in options) ? options.selectedIndex : -1;

            element.querySelector("header[role=banner] .pagetitle").textContent = this._group.title;

            // ListView를 설정합니다.
            listView.itemDataSource = this._items.dataSource;
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.onselectionchanged = this._selectionChanged.bind(this);
            listView.layout = new ui.ListLayout();

            this._updateVisibility();
            if (this._isSingleColumn()) {
                if (this._itemSelectionIndex >= 0) {
                    // 단일 열 자세히 보기를 보려면 기사를 로드하십시오.
                    binding.processAll(element.querySelector(".articlesection"), this._items.getAt(this._itemSelectionIndex));
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/split/split.html") {
                    // 사용자 맞추기, 다른 페이지로 이동, 맞추기 해제 후 다시 이 페이지로
                    // 돌아오도록 백스택을 정리합니다.
                    nav.history.backStack.pop();
                }
                // 이 페이지에 selectionIndex가 있는 경우 선택 사항이
                // ListView에 나타나도록 합니다.
                listView.selection.set(Math.max(this._itemSelectionIndex, 0));
            }
        },

        unload: function () {
            this._items.dispose();
        },

        // 이 함수는 viewState 변경 내용에 응답하여 페이지 레이아웃을 업데이트합니다.
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
                    // 응용 프로그램이 단일 열 자세히 보기로 맞춰진 경우
                    // 단일 열 목록 보기를 백스택에 추가합니다.
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
                // 응용 프로그램이 2열 보기로 맞추기가 해제된 경우 백스택에 추가된
                // splitPage 인스턴스를 모두 제거합니다.
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

        // 이 함수는 목록 및 세부 사항 열이 나란히 표시되지 않고
        // 별도의 페이지에 표시되는지 확인합니다.
        _isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        _selectionChanged: function (args) {
            var listView = document.body.querySelector(".itemlist").winControl;
            var details;
            // 기본적으로 선택 사항은 단일 항목으로 제한됩니다.
            listView.selection.getItems().done(function updateDetails(items) {
                if (items.length > 0) {
                    this._itemSelectionIndex = items[0].index;
                    if (this._isSingleColumn()) {
                        // 맞춰지거나 세로 방향일 경우 선택한 항목의 세부 사항이 있는
                        // 새 페이지로 이동합니다.
                        nav.navigate("/pages/split/split.html", { groupKey: this._group.key, selectedIndex: this._itemSelectionIndex });
                    } else {
                        // 전체 화면이거나 채워진 경우 세부 사항 열을 새 데이터로 업데이트합니다.
                        details = document.querySelector(".articlesection");
                        binding.processAll(details, items[0].data);
                        details.scrollTop = 0;
                    }
                }
            }.bind(this));
        },

        // 이 함수는 현재 보기 상태와 항목 선택을 기준으로 두 열의
        // 표시 유형을 전환합니다.
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
