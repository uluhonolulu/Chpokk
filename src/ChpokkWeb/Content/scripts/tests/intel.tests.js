describe("On pressing the period key", function () {
    var manager, editor, container;
    beforeEach(function () {
        if ($('#fixture').length === 0) {
            $('<div id = "fixture"/>').appendTo('body');
        }
        editor = $('<div/>').appendTo('#fixture');
        container = $('<div/>').hide().appendTo('#fixture');
        manager = new IntelManager(editor, container, {});

    });

    describe("If the returned list is not empty", function () {
        beforeEach(function () {
            // Arrange
            Server.stubContinuation({
                Items: [{ 'Text': 'sample'}]
            });

            //Act
            manager.showData();

            Server.respond();

        });
        it("Intellisense list should show up", function () {
            expect(container).toBeVisible();
        });
        it("The list should contain the same number if items as the returned data", function () {
            expect(container.find('li').length).toBe(1);
        });

        afterEach(function () {
            $('#fixture').empty();
        });
    });

    it("Should send data to server", function () {
        // Arrange
        Server.stubContinuation({});
        // Spy on jQuery's ajax method
        var spy = sinon.spy(jQuery, 'ajax');

        //Act
        manager.showData();

        //Server.respond();
        expect(spy.called).toBeTruthy();
        expect(spy.args[0][0].url).toBe('url::ChpokkWeb.Features.Editor.Intellisense.IntelInputModel');

    });
});

describe("Selection suite", function () {
    var editor;
    var range = document.createRange();
    beforeEach(function () {
        if ($('#fixture').length === 0) {
            $('<div id = "fixture"/>').appendTo('body');
        }
        editor = $('<div/>').appendTo('#fixture')[0];
    });
    describe("When selecting the very start", function () {
        it("The position should be zero", function () {
            range.setStart(editor, 0);
            range.collapse(true);
            var position = getCaretPosition(range);
            expect(position).toBe(0);
        });
    });

    function getCaretPosition(range) {
        return 0;
    }

    afterEach(function () {
        $('#fixture').empty();
    });
});