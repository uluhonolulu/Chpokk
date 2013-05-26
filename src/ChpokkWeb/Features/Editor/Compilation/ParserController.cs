using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.LanguageSupport;
using FubuMVC.Core.Ajax;
using ICSharpCode.NRefactory;

// use ICSharpCode.NRefactory.ParserFactory to create a parser based on filename

namespace ChpokkWeb.Features.Editor.Compilation {
	public class ParserController {
		private readonly LanguageDetector _languageDetector;
		public ParserController(LanguageDetector languageDetector) {
			_languageDetector = languageDetector;
		}

		public AjaxContinuation Parse(ParserInputModel input) {
			//var filePath = _repositoryManager.GetPhysicalFilePath(input);
			var language = _languageDetector.GetLanguage(input.PathRelativeToRepositoryRoot);
			var parser = ParserFactory.CreateParser(language, new StringReader(input.Content));
			parser.Parse();
			if (parser.Errors.Count == 0) {
				return AjaxContinuation.Successful();
			}
			else {
				var continuation = new AjaxContinuation();
				continuation.Errors.Add(new AjaxError {message = parser.Errors.ErrorOutput});
				return continuation;
			}
		}
	}
}