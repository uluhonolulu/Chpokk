$(function () {
    $.fn.keyz.keymap.space = $.ui.keyCode.SPACE;
});

function CodeEditor(element, model) {
    this.element = element;
    this.intelContainer = $('#' + element.data('intelResults'));
    var intelManager = new IntelManager($('#code'), $('#intel_results'));
    $('#code').keyz(null, null, {
        'space': function () {
            colorize(this);
            return true;
        },

        '.': function () {
            //displayIntellisense(this);
            intelManager.showData();
            return true;
            function displayIntellisense(editor) {
                var intelData = {
                    container: $('#intel_results'),
                    editor: $('#code'),
                    show: function () {
                        //container.hide();
                        var text = this.editor.text();
                        //var position = this.editor.caret().start;
                        var position = text.indexOf('""') + 2;
                        var intelUrl = 'url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel';
                        $.post(intelUrl, { Text: text, Position: position, NewChar: '.', RepositoryName: 'Chpokk-SampleSol', ProjectPath: 'src\\ConsoleApplication1\\ConsoleApplication1.csproj' }, function (inteldata) {
                            if (inteldata != null && inteldata.Items != null && inteldata.Items.length > 0) {
                                alert("it works!");
                            }

                        });

                    }
                };
                intelData.show();
            }

        }
    });
}