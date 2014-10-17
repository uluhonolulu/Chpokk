(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/itemDetail/itemDetail.html", {
        // 每當使用者巡覽至此頁面時，就會呼叫這個函式。它
        // 會將應用程式的資料填入頁面項目。
        ready: function (element, options) {
            var item = options && options.item ? Data.resolveItemReference(options.item) : Data.items.getAt(0);
            element.querySelector(".titlearea .pagetitle").textContent = item.group.title;
            element.querySelector("article .item-title").textContent = item.title;
            element.querySelector("article .item-subtitle").textContent = item.subtitle;
            element.querySelector("article .item-image").src = item.backgroundImage;
            element.querySelector("article .item-image").alt = item.subtitle;
            element.querySelector("article .item-content").innerHTML = item.content;
            element.querySelector(".content").focus();
        }
    });
})();
