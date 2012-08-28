function IntelManager(editor, container) {
    this.editor = editor;
    this.container = container;
}

IntelManager.prototype.showData = function () {
    this.container.show();
};

describe("On pressing the period key", function () {
    it("Intellisense list should show up", function () {
        // Arrange
        var editor = $('<div/>').appendTo('body');
        var container = $('<div/>').hide().appendTo('body');
        var manager = new IntelManager(editor, container);
        Server.stubContinuation({
            success: true
        });

        //Act
        manager.showData();

        //Assert
        expect(container).toBeVisible();
    });
});