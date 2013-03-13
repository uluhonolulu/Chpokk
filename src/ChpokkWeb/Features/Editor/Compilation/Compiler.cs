using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Intellisense;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

namespace ChpokkWeb.Features.Editor.Compilation {
	public class Compiler {
		public ICompilationUnit Compile(DefaultProjectContent projectContent, TextReader textReader) {
			ICompilationUnit compilationUnit;
			using (var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, textReader)) {
				parser.ParseMethodBodies = false;
				parser.Parse();
				compilationUnit = ConvertCompilationUnit(parser.CompilationUnit, projectContent);
			}
			projectContent.UpdateCompilationUnit(null, compilationUnit, null);
			return compilationUnit;
		}

		private static DefaultProjectContent _projectContent;

		public static DefaultProjectContent DefaultProjectContent {
			get {
				if (_projectContent == null) {
					var pcRegistry = new ProjectContentRegistry();

					_projectContent = new DefaultProjectContent() {Language = LanguageProperties.CSharp};
					_projectContent.AddReferencedContent(pcRegistry.Mscorlib);
					
				}
				return _projectContent;
			}
		}

		public static void WarmUp() {
			var x = DefaultProjectContent;
		}

		public static ExpressionResult FindExpression(string text, int offset, ParseInformation parseInformation) {
			var finder = new CSharpExpressionFinder(parseInformation);
			var expression = finder.FindExpression(text, offset);
			return expression;
		}

		public static ICompilationUnit ConvertCompilationUnit(ICSharpCode.NRefactory.Ast.CompilationUnit cu, IProjectContent projectContent) {
			var converter = new NRefactoryASTConvertVisitor(projectContent, SupportedLanguage.CSharp);
			cu.AcceptVisitor(converter, null);
			return converter.Cu;
		}
	}
}