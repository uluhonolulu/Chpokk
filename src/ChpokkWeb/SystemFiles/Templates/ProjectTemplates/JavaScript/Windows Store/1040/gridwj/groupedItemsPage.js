(function () {
    "use strict";

    var appView = Windows.UI.ViewManagement.ApplicationView;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/groupedItems/groupedItems.html", {
        // Passa a groupHeaderPage. Chiamato da groupHeaders,
        // tasto di scelta rapida e iteminvoked.
        navigateToGroup: function (key) {
            nav.navigate("/pages/groupDetail/groupDetail.html", { groupKey: key });
        },

        // La funzione viene chiamata quando un utente passa a questa pagina. Popola
        // gli elementi della pagina con i dati dell'applicazione.
        ready: function (element, options) {
            var listView = element.querySelector(".groupeditemslist").winControl;
            listView.groupHeaderTemplate = element.querySelector(".headertemplate");
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.oniteminvoked = this._itemInvoked.bind(this);

            // Impostare un tasto di scelta rapida (CTRL + ALT + g) per passare al
            // gruppo corrente non in modalità ancorata.
            listView.addEventListener("keydown", function (e) {
                if (appView.value !== appViewState.snapped && e.ctrlKey && e.keyCode === WinJS.Utilities.Key.g && e.altKey) {
                    var data = listView.itemDataSource.list.getAt(listView.currentItem.index);
                    this.navigateToGroup(data.group.key);
                    e.preventDefault();
                    e.stopImmediatePropagation();
                }
            }.bind(this), true);

            this._initializeLayout(listView, appView.value);
            listView.element.focus();
        },

        // La funzione aggiorna il layout della pagina in risposta alle modifiche di viewState.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            var listView = element.querySelector(".groupeditemslist").winControl;
            if (lastViewState !== viewState) {
                if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    }
                    listView.addEventListener("contentanimating", handler, false);
                    this._initializeLayout(listView, viewState);
                }
            }
        },

        // Questa funzione aggiorna ListView con i nuovi layout
        _initializeLayout: function (listView, viewState) {
            /// <param name="listView" value="WinJS.UI.ListView.prototype" />

            if (viewState === appViewState.snapped) {
                listView.itemDataSource = Data.groups.dataSource;
                listView.groupDataSource = null;
                listView.layout = new ui.ListLayout();
            } else {
                listView.itemDataSource = Data.items.dataSource;
                listView.groupDataSource = Data.groups.dataSource;
                listView.layout = new ui.GridLayout({ groupHeaderPosition: "top" });
            }
        },

        _itemInvoked: function (args) {
            if (appView.value === appViewState.snapped) {
                // Se la pagina è bloccata, l'utente ha chiamato un gruppo.
                var group = Data.groups.getAt(args.detail.itemIndex);
                this.navigateToGroup(group.key);
            } else {
                // Se la pagina non è bloccata, l'utente ha chiamato un elemento.
                var item = Data.items.getAt(args.detail.itemIndex);
                nav.navigate("/pages/itemDetail/itemDetail.html", { item: Data.getItemReference(item) });
            }
        }
    });
})();
