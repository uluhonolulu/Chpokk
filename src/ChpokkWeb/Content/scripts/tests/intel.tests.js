﻿describe("On requesting the intel data", function () {
	var manager, container;
	beforeEach(function () {
		manager = createManager();
		container = manager.container;
		sinon.stub(manager, 'getSelectedRange', function () {
			var range = document.createRange();
			range.setStart(manager.editorElement[0], 0);
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
    var manager, items = [{ "Text": "sample"}];
    beforeEach(function () {
        manager = createManager();
        manager.showItems(items);
    });

    it("Should select the first item", function () {
        expect(manager.selectedItem).toBe(items[0]);
    });
});

describe("On pressing the period key", function () {
	var editor, manager;
	beforeEach(function () {
	    manager = createManager();
        editor = manager.editorElement;
		Server.stubContinuation({});
	});
	describe("Should surround the period with a span", function () {
		it("when the editor is empty", function () {
			editor.html('');
			//typeDot();
			editor.sendkeys('.');
            manager.htmlEditor.wrapTheDot();

			expect(editor.html()).toBe('<span id="wrapper">.</span>');
		});
		it("when the editor is simple text", function () {
			editor.html('stuffhere');
			setPosition(5);
			editor.sendkeys('.');
			//typeDot();
			manager.htmlEditor.wrapTheDot();
			expect(editor.html()).toBe('stuff<span id="wrapper">.</span>here');
		});

		it("when there's another dot already", function () {
			editor.html('stuff.here');
			setPosition(10);
			editor.sendkeys('.');

			manager.htmlEditor.wrapTheDot();
			expect(editor.html()).toBe('stuff.here<span id="wrapper">.</span>');
		});

		it("when there is an inner node before", function () {
			editor.html('<span>stuff</span> here');
			setPosition(10);
			editor.sendkeys('.');

			manager.htmlEditor.wrapTheDot();
			expect(editor.html()).toBe('<span>stuff</span> here<span id="wrapper">.</span>');

		});


		it("in the real life scenario", function () {
			editor.html('<span class="code0">using</span>&nbsp;System;\r\n\r\n');
			setPosition(13);
			editor.sendkeys('.');
			manager.htmlEditor.wrapTheDot();
			var html = editor.html();
			var expected = '<span class="code0">using</span>&nbsp;System;<span id="wrapper">.</span>\n\n'; // note: \r disappears
			var length = (html.length >= expected.length) ? html.length : expected.length;
			for (var i = 0; i < length; i++) {
				expect(html.substr(0, i) + i).toBe(expected.substr(0, i) + i);
			}
			expect(editor.html()).toBe(expected);

		});

		it("when there's another *wrapped* dot, should remove the wrapper", function () {
			editor.html('stuff<span id="wrapper">.</span>here');
			setPosition(36);
			editor.sendkeys('.');

			manager.htmlEditor.wrapTheDot();
			expect(editor.html()).toBe('stuff.here<span id="wrapper">.</span>');

		});

	});


	function setPosition(position) {
		bililiteRange(editor.get(0)).bounds('selection').bounds([position, position]).select();
	}

	function typeDot() {
		var code = '.'.charCodeAt(0);
		editor.trigger({ type: 'keypress', keyCode: code, which: code, charCode: code });
	}
});

describe("Mouseover", function () {
    var manager, items = [{Name: "1"}, {Name: "2"}];
    beforeEach(function () {
        manager = createManager();
        manager.showItems(items);

        var li = manager.container.find('li')[1];
        $(li).mouseover();
    });
    it("Selects the corresponding item", function () {
        expect(manager.selectedItem).toBe(items[1]);
    });
});

describe("Selecting an item", function () {
    var manager, items = [{ Name: "1" }, { Name: "2"}];
    beforeEach(function () {
        manager = createManager();
        manager.showItems(items);
        manager.selectItem(1);
    });
    it("Adds the 'ui-state-hover' class to the link", function () {
        var a = manager.container.find('li a')[1];
        expect(a).toHaveClass('ui-state-hover');
    });
    it("Removes the 'ui-state-hover' class from everything else", function() {
        var a = manager.container.find('li a')[0];
        expect(a).not.toHaveClass('ui-state-hover');        
    });
    afterEach(function () {
        $('#fixture').empty();
    });
});

describe("Using the selected item", function () {
    var manager, items = [{ Name: "1" }, { Name: "2"}];
    beforeEach(function () {
        manager = createManager();
        spyOn(manager.htmlEditor, 'wrapTheDot');
        spyOn(window, 'getDotOffset').andReturn({top:0, left:0});
        manager.showItems(items);
        manager.selectItem(1);
    });
    it("Adds the selected text to the editor", function () { //TODO: fix it by mocking the wrapthedot method
        manager.useSelected(1); //act

        expect(manager.editorElement.html()).toEqual(items[1].Name);
    });
    it("Inserts the selected text at the caret position", function () {
        var editor = manager.editorElement;
        editor.text("some");
        //setCaretPosition(editor, 2);

        manager.useSelected(2); //Act
        expect(editor.html()).toEqual("so2me");

    });
});

describe("If the returned list is empty", function () {
    var manager, editor, container;
    beforeEach(function () {
        editor = createEditor();
        container = createContainer();
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
		editor = createEditor();
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

			// let's test that new plugin
			var s = window.getSelection();
			if (s.rangeCount > 0)
				s.removeAllRanges();
			s.addRange(range);
			var brange = bililiteRange(editor.get(0)).bounds('selection');
			var bosition = brange.bounds()[0];
			expect(bosition).toBe(14);
		});


	});

});

describe("When code contains angular brackets", function () {
	it("keeps them after wrapping the dot", function () {
		var fragment = document.createDocumentFragment();
		fragment.appendChild(document.createTextNode("<..>"));
		var result = getFragmentSource(fragment);
		expect(result).toBe("&lt;..&gt;");
	});
});

describe("all keyz hooks", function () {
	it("doesn't throw", function () {
		var editor = createEditor();
		var codeEditor = new CodeEditor(editor, {});
		var handlers = codeEditor.getKeyHandlers();
		handlers.space();
		expect(true).toBeTruthy();
	});
});

function ensureFixture() {
    if ($('#fixture').length === 0) {
        $('<div id = "fixture"/>').appendTo('body');
    }
}

afterEach(function () {
    $('#fixture').empty();
});


function createEditor() {
    ensureFixture();
    return $('<div/>').appendTo('#fixture');
}

function createContainer() {
    ensureFixture();
    return $('<div/>').hide().appendTo('#fixture');
}

function createManager() {
    var editor = createEditor();
    var container = createContainer();
    var htmlEditor = new HtmlEditor(editor); 
    return new IntelManager(editor, container, htmlEditor, {});
}