/* ***** BEGIN LICENSE BLOCK *****
 * Distributed under the BSD license:
 *
 * Copyright (c) 2012, Ajax.org B.V.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of Ajax.org B.V. nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL AJAX.ORG B.V. BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *
 * Contributor(s):
 * 
 *
 *
 * ***** END LICENSE BLOCK ***** */

ace.define('ace/mode/vbnet', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/text', 'ace/tokenizer', 'ace/mode/vbnet_highlight_rules'], function (require, exports, module) {


var oop = require("../lib/oop");
var TextMode = require("./text").Mode;
var Tokenizer = require("../tokenizer").Tokenizer;
var VBNetHighlightRules = require("./vbnet_highlight_rules").VBNetHighlightRules;

var Mode = function() {
	var highlighter = new VBNetHighlightRules();
	
	this.$tokenizer = new Tokenizer(highlighter.getRules());
};
oop.inherits(Mode, TextMode);

(function() {
	   
	this.lineCommentStart = ["'", "REM"];
	
}).call(Mode.prototype);

exports.Mode = Mode;
});


ace.define('ace/mode/vbnet_highlight_rules', ['require', 'exports', 'module' , 'ace/lib/oop', 'ace/mode/text_highlight_rules'], function(require, exports, module) {


var oop = require("../lib/oop");
var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

	//

var VBNetHighlightRules = function() {
	var keywordMapper = this.createKeywordMapper({
		"keyword": "AddHandler|AddressOf|Aggregate|Alias|All|And|AndAlso|Ansi|As|Ascending|Assembly|Auto|Binary|Boolean|By|ByRef|Byte|ByVal|Call|Case|Catch|CBool|CByte|CChar|CDate|CDbl|CDec|Char|CInt|Class|CLng|CObj|Compare|Const|Continue|CSByte|CShort|CSng|CStr|CType|CUInt|CULng|CUShort|Custom|Date|Decimal|Declare|Default|Delegate|Descending|Dim|DirectCast|Distinct|Do|Double|Each|Else|ElseIf|End|EndIf|Enum|Equals|Erase|Error|Event|Exit|Explicit|ExternalChecksum|ExternalSource|False|Finally|For|Friend|From|Function|Get|GetType|GetXmlNamespace|Global|Gosub|GoTo|Group|Handles|If|Implements|Imports|In|Infer|Inherits|Integer|Interface|Into|Is|IsFalse|IsNot|IsTrue|Join|Key|Let|Lib|Like|Long|Loop|Me|Mid|Mod|Module|MustInherit|MustOverride|MyBase|MyClass|Namespace|Narrowing|New|Next|Not|Nothing|NotInheritable|NotOverridable|Object|Of|Off|On|Operator|Option|Optional|Or|Order|OrElse|Out|Overloads|Overridable|Overrides|ParamArray|Partial|Preserve|Private|Property|Protected|Public|RaiseEvent|ReadOnly|ReDim|Reference|Region|REM|RemoveHandler|Resume|Return|SByte|Select|Set|Shadows|Shared|Short|Single|Skip|Static|Step|Stop|Strict|String|Structure|Sub|SyncLock|Take|Text|Then|Throw|To|True|Try|TryCast|Type|TypeOf|UInteger|ULong|Unicode|Until|UShort|Using|Variant|Wend|When|Where|While|Widening|With|WithEvents|WriteOnly|Xml|Xor",
		"variable.language": "Me",
		"constant.language": "Nothing|True|False"
	}, "identifier", true);

	this.$rules = {
	"start": [
		{
			token: [
				"meta.ending-space"
			],
			regex: "$"
		},
		{
			token: [
				null
			],
			regex: "^(?=\\t)",
			next: "state_3"
		},
		{
			token: [null],
			regex: "^(?= )",
			next: "state_4"
		},
		{
			token: [
				"storage.type.function.vbnet",
				"text",
				"entity.name.function.vbnet",
				"text",
				"punctuation.definition.parameters.vbnet",
				"variable.parameter.function.vbnet",
				"punctuation.definition.parameters.vbnet"
			],
			regex: "^\\s*((?:Function|Sub))(\\s*)([a-zA-Z_]\\w*)(\\s*)(\\()([^)]*)(\\)).*\\n?",
			caseInsensitive: true
		},
		{
			token: "punctuation.definition.comment.vbnet",
			regex: "'|REM",
			next: "comment"
		},
		{
			token: [
				"keyword.control.vbnet"
			],
			regex: "(?:\\b(If|Then|Else|ElseIf|Else If|End If|While|Wend|For|To|Each|Case|Select|End Select|Return|Continue|Do|Until|Loop|Next|With|Exit Do|Exit For|Exit Function|Exit Property|Exit Sub|IIf|Try|Catch|Finally)\\b)",
			caseInsensitive: true
		},
		{
			token: [
				"keyword.operator.vbnet"
			],
			regex: "(?:\\b(Mod|And|AndAlso|Not|Or|OrElse|Xor)\\b)",
			caseInsensitive: true
		},
		{
			token: [
				"storage.type.vbnet"
			],
			regex: "Dim|As|Call|Class|Const|Dim|Redim|Function|Sub|Private Sub|Public Sub|End Sub|End Function|Set|Get|New|Randomize|Option Explicit|On Error Resume Next|On Error GoTo",
			caseInsensitive: true
		},
		{
			token: [
				"storage.modifier.vbnet"
			],
			regex: "(?:\\b(Private|Public|Protected|Friend|Shared|Overridable|Default)\\b)",
			caseInsensitive: true
		},
		{
			token: [
				"constant.language.vbnet"
			],
			regex: "(?:\\s*\\b(Empty|False|Nothing|Null|True)\\b)",
			caseInsensitive: true
		},
		{
			token: [
				"punctuation.definition.string.begin.vbnet"
			],
			regex: '"',
			next: "string"
		},
		{
			token: [
				"punctuation.definition.variable.vbnet"
			],
			regex: "(\\$)[a-zA-Z_x7f-xff][a-zA-Z0-9_x7f-xff]*?\\b\\s*"
		},
		{
			token: [
				"support.function.event.vbnet"
			],
			regex: "(?:\\b(Application_OnEnd|Application_OnStart|OnTransactionAbort|OnTransactionCommit|Session_OnEnd|Session_OnStart)\\b)"
		},
		{
			token: [
				"support.function.vb.vbnet"
			],
			regex: "(?:\\b(Array|Add|Asc|Atn|CBool|CByte|CCur|CDate|CDbl|Chr|CInt|CLng|Conversions|Cos|CreateObject|CSng|CStr|Date|DateAdd|DateDiff|DatePart|DateSerial|DateValue|Day|Derived|Math|Escape|Eval|Exists|Exp|Filter|FormatCurrency|FormatDateTime|FormatNumber|FormatPercent|GetLocale|GetObject|GetRef|Hex|Hour|InputBox|InStr|InStrRev|Int|Fix|IsArray|IsDate|IsEmpty|IsNull|IsNumeric|IsObject|Item|Items|Join|Keys|LBound|LCase|Left|Len|LoadPicture|Log|LTrim|RTrim|Trim|Maths|Mid|Minute|Month|MonthName|MsgBox|Now|Oct|Remove|RemoveAll|Replace|RGB|Right|Rnd|Round|ScriptEngine|ScriptEngineBuildVersion|ScriptEngineMajorVersion|ScriptEngineMinorVersion|Second|SetLocale|Sgn|Sin|Space|Split|Sqr|StrComp|String|StrReverse|Tan|Time|Timer|TimeSerial|TimeValue|TypeName|UBound|UCase|Unescape|VarType|Weekday|WeekdayName|Year)\\b)",
			caseInsensitive: true
		},
		{
			token: [
				"constant.numeric.vbnet"
			],
			regex: "-?\\b(?:(?:0(?:x|X)[0-9a-fA-F]*)|(?:(?:[0-9]+\\.?[0-9]*)|(?:\\.[0-9]+))(?:(?:e|E)(?:\\+|-)?[0-9]+)?)(?:L|l|UL|ul|u|U|F|f)?\\b"
		},
		{
			token: [
				"support.type.vb.vbnet"
			],
			regex: "(?:\\b(vbtrue|vbfalse|vbcr|vbcrlf|vbformfeed|vblf|vbnewline|vbnullchar|vbnullstring|int32|vbtab|vbverticaltab|vbbinarycompare|vbtextcomparevbsunday|vbmonday|vbtuesday|vbwednesday|vbthursday|vbfriday|vbsaturday|vbusesystemdayofweek|vbfirstjan1|vbfirstfourdays|vbfirstfullweek|vbgeneraldate|vblongdate|vbshortdate|vblongtime|vbshorttime|vbobjecterror|vbEmpty|vbNull|vbInteger|vbLong|vbSingle|vbDouble|vbCurrency|vbDate|vbString|vbObject|vbError|vbBoolean|vbVariant|vbDataObject|vbDecimal|vbByte|vbArray)\\b)",
			caseInsensitive: true
		},
		{
			token: [
				"entity.name.function.vbnet"
			],
			regex: "(?:(\\b[a-zA-Z_x7f-xff][a-zA-Z0-9_x7f-xff]*?\\b)(?=\\(\\)?))"
		},
		{
			token: [
				"keyword.operator.vbnet"
			],
			regex: "\\-|\\+|\\*\\\/|\\>|\\<|\\=|\\&"
		}, {
			token: keywordMapper,
			regex: "[a-zA-Z_$][a-zA-Z0-9_$]*\\b"
		}
	],
	"state_3": [
		{
			token: [
				"meta.odd-tab.tabs",
				"meta.even-tab.tabs"
			],
			regex: "(\\t)(\\t)?"
		},
		{
			token: "meta.leading-space",
			regex: "(?=[^\\t])",
			next: "start"
		},
		{
			token: "meta.leading-space",
			regex: ".",
			next: "state_3"
		}
	],
	"state_4": [
		{
			token: [
				"meta.odd-tab.spaces",
				"meta.even-tab.spaces"
			],
			regex: "(  )(  )?"
		},
		{
			token: "meta.leading-space",
			regex: "(?=[^ ])",
			next: "start"
		},
		{
			token: "meta.leading-space",
			regex: ".",
			next: "state_4"
		}
	],
	"comment": [
		{
			token: "comment.line.apostrophe.vbnet",
			regex: "$|(?=(?:%>))",
			next: "start"
		},
		{
			token: "comment.line.apostrophe.vbnet",
			regex: "."
		}
	],
	"string": [
		{
			token: "constant.character.escape.apostrophe.vbnet",
			regex: '""'
		},
		{
			token: "string.quoted.double.vbnet",
			regex: '"',
			next: "start"
		},
		{
			token: "string.quoted.double.vbnet",
			regex: "."
		}
	]
}

};

oop.inherits(VBNetHighlightRules, TextHighlightRules);

exports.VBNetHighlightRules = VBNetHighlightRules;
});