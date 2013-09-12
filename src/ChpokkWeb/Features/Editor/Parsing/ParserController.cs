using System.IO;
using ChpokkWeb.Features.LanguageSupport;
using FubuMVC.Core.Ajax;
using ICSharpCode.NRefactory;

// use ICSharpCode.NRefactory.ParserFactory to create a parser based on filename

namespace ChpokkWeb.Features.Editor.Parsing {
	public class ParserEndpoint {
		private readonly LanguageDetector _languageDetector;
		public ParserEndpoint(LanguageDetector languageDetector) {
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