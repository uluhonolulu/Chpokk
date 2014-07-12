using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChpokkWeb.Features.LanguageSupport;
using FubuMVC.Core.Ajax;
using ICSharpCode.NRefactory;
using Roslyn.Compilers.Common;

// use ICSharpCode.NRefactory.ParserFactory to create a parser based on filename

namespace ChpokkWeb.Features.Editor.Parsing {
	public class ParserEndpoint {
		private readonly LanguageDetector _languageDetector;
		public ParserEndpoint(LanguageDetector languageDetector) {
			_languageDetector = languageDetector;
		}

		public ParserModel Parse(ParserInputModel input) {
			//var filePath = _repositoryManager.GetPhysicalFilePath(input);
			CommonSyntaxTree tree;
			var language = _languageDetector.GetLanguage(input.PathRelativeToRepositoryRoot);
			tree = language==SupportedLanguage.CSharp? (CommonSyntaxTree) Roslyn.Compilers.CSharp.SyntaxTree.ParseText(input.Content) : Roslyn.Compilers.VisualBasic.SyntaxTree.ParseText(input.Content);
			var diagnostics = tree.GetRoot().GetDiagnostics();
			var errors = from diagnostic in diagnostics
			             select
				             new ErrorInfo()
					             {
						             Message = diagnostic.Info.ToString(),
						             PositionSpan = diagnostic.Location.GetLineSpan(false)
					             };
			return new ParserModel() {Errors = errors};
		}
	}

	public class ParserModel {
		public IEnumerable<ErrorInfo> Errors { get; set; }
	}
}