describe("WhiteSpace function", function () {
	var spaces = '  \t  ';
	var source = spaces + 'hmhm';
	it("When string starts with whitespaces, whiteSpace function should return these", function () {
		expect(whiteSpaces(source)).toBe(spaces);
	});
	it("When string starts with non-whitespace, let's return an empty space.", function() {
		expect(whiteSpaces('hmhm')).toBe('');
	});

	afterEach(function () {
	});
});

function whiteSpaces(source) {
	var tester = /^\s*/;
	var result = tester.exec(source);
	return result[0];
}