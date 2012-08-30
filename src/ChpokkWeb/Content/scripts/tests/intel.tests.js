function IntelManager(editor, container) {
    this.editor = editor;
    this.container = container;
}

IntelManager.prototype.showData = function () {
    var intelUrl = 'url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel';
    $.post(intelUrl);
    this.container.show();
};

describe("On pressing the period key", function () {
    var manager, editor, container;
    beforeEach(function() {
        editor = $('<div/>').appendTo('body');
        container = $('<div/>').hide().appendTo('body');
        manager = new IntelManager(editor, container);
        
    });

//    describe("If the returned list is not empty", function () {
//    });
    it("Intellisense list should show up", function () {
        // Arrange
        Server.stubContinuation({
            Items: [{ Text: 'sample'}]
        });

        //Act
        manager.showData();

        //Assert
        expect(container).toBeVisible();
    });

    it("Should send data to server", function () {
        // Arrange
        Server.stubContinuation({
            Items: [{ Text: 'sample'}]
        });
        // Spy on jQuery's ajax method
        var spy = sinon.spy(jQuery, 'ajax');

        //Act
        manager.showData();

        //Server.respond();
//        expect(spy.called).toBeTruthy();
//        expect(spy.args[0][0].url).toBe('url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel');

    });
});

