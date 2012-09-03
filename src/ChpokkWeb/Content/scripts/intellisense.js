function IntelManager(editor, container, model) {
    this.editor = editor;
    this.container = container;
    this.listItemTemplate = $.template(null, "<li class='ui-menu-item'><a class='ui-corner-all' nobr><img src=\"/_content/images/Intellisense/Icons.16x16.${EntityType}.png\" />&nbsp;${Text}</a></li>");
    this.model = model;
}

IntelManager.prototype.showData = function () {
    var intelUrl = 'url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel';
    var self = this;
    var text = this.editor.text();
    var range = this.getSelectedRange();
    var position = getCaretPosition(range) - 1; // we need the position just before the typed char
    $.post(intelUrl, { Text: text, Position: position, NewChar: '.', RepositoryName: this.model.RepositoryName, ProjectPath: this.model.ProjectPath }, function (intelData) {
        self.showItems(intelData.Items);
    });
};

IntelManager.prototype.showItems = function (items) {
    this.items = items;
    $.tmpl(this.listItemTemplate, items).appendTo(this.container);
    if (items && items.length > 0) {
        this.container.show();
        this.selectItem(0);
    }
};

IntelManager.prototype.selectItem = function(index) {
    this.selectedItem = this.items[index];
};

IntelManager.prototype.getSelectedRange = function() {
    var selection = window.getSelection();
    return selection.getRangeAt(0);
};


function getCaretPosition(range) {
    var index = $.inArray(range.startContainer, range.startContainer.parentNode.childNodes);
    var lengthOfPreviousNodes = 0;
    for (var i = 0; i < index; i++) {
        lengthOfPreviousNodes += range.startContainer.parentNode.childNodes[i].textContent.length;
    }
    return lengthOfPreviousNodes + range.startOffset;
}