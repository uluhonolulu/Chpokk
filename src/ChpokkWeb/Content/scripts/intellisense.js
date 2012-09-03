function IntelManager(editor, container, model) {
    this.editor = editor;
    this.container = container;
    this.listItemTemplate = $.template(null, "<li class='ui-menu-item'><a class='ui-corner-all' nobr><img src=\"/_content/images/Intellisense/Icons.16x16.${EntityName}.png\" />&nbsp;${Text}</a></li>");
    this.model = model;
}

IntelManager.prototype.showData = function () {
    var intelUrl = 'url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel';
    var self = this;
    var text = this.editor.text();
    var selection = window.getSelection();
    var position = text.indexOf('""') + 2;
    var recentChars = text.substring(position - 5, position);
    trace(recentChars);
    debugger;
    $.post(intelUrl, { Text: text, Position: position, NewChar: '.', RepositoryName: this.model.RepositoryName, ProjectPath: this.model.ProjectPath }, function (intelData) {
        $.tmpl(self.listItemTemplate, intelData.Items).appendTo(self.container);
        self.container.show();
    });
};

//IntelManager.prototype.showItems