using System.Collections.Generic;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using ICSharpCode.SharpDevelop.Dom.VBNet;

namespace ChpokkWeb.Features.Compilation {
	public class Compiler {
		public ICompilationUnit Compile(IProjectContent projectContent, TextReader textReader, SupportedLanguage language) {
			var compilationUnit = ParseCode(projectContent, textReader, language);
			projectContent.UpdateCompilationUnit(null, compilationUnit, null);
			return compilationUnit;
		}

		public ICompilationUnit ParseCode(IProjectContent projectContent, TextReader textReader, SupportedLanguage language) {
			ICompilationUnit compilationUnit;
			using (var parser = ParserFactory.CreateParser(language, textReader)) {
				parser.ParseMethodBodies = false;
				parser.Parse();
				compilationUnit = ConvertCompilationUnit(parser.CompilationUnit, projectContent, language);
			}
			return compilationUnit;
		}


		public void CompileAll(IProjectContent projectContent, IEnumerable<TextReader> sources, SupportedLanguage language) {
			foreach (var source in sources) {
				this.Compile(projectContent, source, language);
			}
		}

		public static ExpressionResult FindExpression(string text, int offset, ParseInformation parseInformation, SupportedLanguage language) {
			IExpressionFinder finder = (language == SupportedLanguage.CSharp)? (IExpressionFinder) new CSharpExpressionFinder(parseInformation): new VBNetExpressionFinder(parseInformation);
			var expression = finder.FindExpression(text, offset);
			return expression;
		}

		public static ICompilationUnit ConvertCompilationUnit(ICSharpCode.NRefactory.Ast.CompilationUnit cu, IProjectContent projectContent, SupportedLanguage language) {
			var converter = new NRefactoryASTConvertVisitor(projectContent, language);
			cu.AcceptVisitor(converter, null);
			return converter.Cu;
		}
	}
}