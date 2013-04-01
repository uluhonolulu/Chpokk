using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Ajax;
using ICSharpCode.NRefactory.CSharp;

namespace ChpokkWeb.Features.Editor.Compilation {
	public class ParserController {
		private CSharpParser _parser;
		public ParserController(CSharpParser parser) {
			_parser = parser;
		}

		public AjaxContinuation Parse(ParserInputModel input) {
			var compilationUnit = _parser.Parse(input.Content, string.Empty);
			if (compilationUnit.Errors.Count == 0) {
				return AjaxContinuation.Successful();
			}
			else {
				var continuation = new AjaxContinuation();
				foreach (var error in compilationUnit.Errors) {
					continuation.Errors.Add(new AjaxError {message = error.Message});
				}
				return continuation;
			}
		}
	}
}