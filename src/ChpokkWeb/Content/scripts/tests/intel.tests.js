describe("On pressing the period key", function () {
    var manager, editor, container;
    beforeEach(function () {
        if ($('#fixture').length === 0) {
            $('<div id = "fixture"/>').appendTo('body');
        }
        editor = $('<div/>').appendTo('#fixture');
        container = $('<div/>').hide().appendTo('#fixture');
        manager = new IntelManager(editor, container, {});
        sinon.stub(manager, 'getSelectedRange', function () {
            var range = document.createRange();
            range.setStart(editor[0], 0);
            return range;
        });

        //IntelManager.prototype.getSelectedRange = ;
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

describe("When intellisense is open", function () {
    var manager, editor, container, items = [{ "Text": "sample"}];
    beforeEach(function () {
        if ($('#fixture').length === 0) {
            $('<div id = "fixture"/>').appendTo('body');
        }
        editor = $('<div/>').appendTo('#fixture');
        container = $('<div/>').hide().appendTo('#fixture');
        manager = new IntelManager(editor, container, {});
        manager.showItems(items);
    });

    it("Should select the first item", function () {
        expect(manager.selectedItem).toBe(items[0]);
    });
});

describe("If the returned list is empty", function () {
    var manager, editor, container;
    beforeEach(function () {
        if ($('#fixture').length === 0) {
            $('<div id = "fixture"/>').appendTo('body');
        }
        editor = $('<div/>').appendTo('#fixture');
        container = $('<div/>').hide().appendTo('#fixture');
        manager = new IntelManager(editor, container, {});
        manager.showItems([]);
    });

    it("The container shouldn't be visible", function () {
        expect(container).toBeHidden();
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
//    describe("When selecting the very start", function () {
//        it("The position should be zero", function () {
//            range.setStart(editor[0], 0);
//            range.collapse(true);
//            var position = getCaretPosition(range);
//            expect(position).toBe(0);
//        });
//    });

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

    describe("When there's a text after span node", function () {
        it("The position should be the count of symbols since the start plus the length of the span", function () {
            editor.html("<span>some</span>other");
            range.setStart(editor[0].childNodes[1], 2);
            range.collapse(true);
            var position = getCaretPosition(range);
            expect(position).toBe(6);
        });
    });

    describe("When there's a text after several nodes", function () {
        it("The position should be the count of symbols since the start plus the combined length of the previous nodes", function () {
            editor.html("<span>some</span>other<span>one</span>more");
            range.setStart(editor[0].childNodes[3], 2);
            range.collapse(true);
            var position = getCaretPosition(range);
            expect(position).toBe(14); //some + other + one + mo -> 4 + 5 + 3 + 2 = 14
        });
    });

    afterEach(function () {
        $('#fixture').empty();
    });
});