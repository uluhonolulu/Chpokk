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
        editor = $('<div/>').appendTo('#fixture');
    });
    describe("When selecting the very start", function () {
        it("The position should be zero", function () {
            range.setStart(editor[0], 0);
            range.collapse(true);
            var position = getCaretPosition(range);
            expect(position).toBe(0);
        });
    });

    describe("When there's a root text node", function () {
        it("The position should be the count of symbols since the start", function () {
            editor.text("some");
            range.setStart(editor[0].childNodes[0], 2);
            range.collapse(true);
            var position = getCaretPosition(range);
            expect(position).toBe(2);
        });
    });

    describe("When there's a root span node", function () {
        it("The position should be the count of symbols since the start", function () {
            editor.html("<span>some</span>");
            range.setStart(editor[0].childNodes[0].childNodes[0], 2);
            range.collapse(true);
            var position = getCaretPosition(range);
            expect(position).toBe(2);
        });
    });
    
//    describe("When there's a text after span node", function () {
//        it("The position should be the count of symbols since the start plus the length of the span", function () {
//            editor.html("<span>some</span>other");
//            range.setStart(editor[0].childNodes[1], 2);
//            range.collapse(true);
//            var position = getCaretPosition(range);
//            expect(position).toBe(6);
//        });
//    });
    
    function getCaretPosition(range) {
        return range.startOffset;
    }

    afterEach(function () {
        $('#fixture').empty();
    });
});