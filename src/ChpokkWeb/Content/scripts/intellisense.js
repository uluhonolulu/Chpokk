function IntelManager(editor, container) {
    this.editor = editor;
    this.container = container;
    this.listItemTemplate = $.template(null, "<li class='ui-menu-item'><a class='ui-corner-all' nobr><img src=\"/_content/images/Intellisense/Icons.16x16.${EntityName}.png\" />&nbsp;${Text}</a></li>");

}

IntelManager.prototype.showData = function () {
    var intelUrl = 'url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel';
    var self = this;
    var text = this.editor.text();
    var position = text.indexOf('""') + 2;
    $.post(intelUrl, { Text: text, Position: position, NewChar: '.', RepositoryName: 'Chpokk-SampleSol', ProjectPath: 'src\\ConsoleApplication1\\ConsoleApplication1.csproj' }, function (intelData) {
        $.tmpl(self.listItemTemplate, intelData.Items).appendTo(self.container);
        self.container.show();
    });
};

//IntelManager.prototype.showItems