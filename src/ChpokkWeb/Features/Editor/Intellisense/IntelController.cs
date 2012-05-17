using System.Diagnostics;
using System.IO;
using System.Linq;
using FubuMVC.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

namespace ChpokkWeb.Editor.Intellisense {
	public class IntelController {
		[JsonEndpoint]
		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			if (input.Text == null) return null;
			var resolver = new NRefactoryResolver(LanguageProperties.CSharp);
			//Debug.Assert(input.Text == "using System;\r\nclass A\r\n{\r\n void B()\r\n {\r\n  string x;\r\n  \r\n }\r\n}\r\n");

			TextReader textReader = new StringReader(input.Text);
			ICompilationUnit compilationUnit;
			var projectContent = DefaultProjectContent;

			using (IParser p = ParserFactory.CreateParser(SupportedLanguage.CSharp, textReader)) {
				// we only need to parse types and method definitions, no method bodies
				// so speed up the parser and make it more resistent to syntax
				// errors in methods
				p.ParseMethodBodies = false;

				p.Parse();
				compilationUnit = this.ConvertCompilationUnit(p.CompilationUnit, projectContent);
			}


			// Remove information from lastCompilationUnit and add information from newCompilationUnit.
			projectContent.UpdateCompilationUnit(null, compilationUnit, "fakefile.cs");
			var parseInformation =  new ParseInformation(compilationUnit);
			//_parseInformation = new ParseInformation(new DefaultCompilationUnit(projectContent));
			var text = input.Text.Insert(input.Position, input.NewChar.ToString());
			//Debug.Assert(text == "using System;\r\nclass A\r\n{\r\n void B()\r\n {\r\n  string x;\r\n  \r\n }\r\n}\r\n");
			var expression = FindExpression(text, input.Position, parseInformation);
			var rr = resolver.Resolve(expression,
													parseInformation,
													text);
			if (rr == null) {
				return null;
			}
			var completionData = rr.GetCompletionData(projectContent);
			if (completionData == null) {
				return null;
			}
			var items = from entry in completionData.OfType<IMember>() select new IntelOutputModel.IntelModelItem {Text = entry.Name, EntityType = entry.EntityType};
			var model = new IntelOutputModel {Message = input.Message, Items = items.Distinct().ToArray()};
			return model;
		}

		private static DefaultProjectContent _projectContent;
		private static DefaultProjectContent DefaultProjectContent {
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
			var x = IntelController.DefaultProjectContent;
		}

		private ExpressionResult FindExpression(string text, int offset, ParseInformation parseInformation) {
			var finder = new CSharpExpressionFinder(parseInformation);
			var expression = finder.FindExpression(text, offset);
			return expression;
			return new ExpressionResult();
		}

		ICompilationUnit ConvertCompilationUnit(ICSharpCode.NRefactory.Ast.CompilationUnit cu, IProjectContent projectContent) {
			var converter = new NRefactoryASTConvertVisitor(projectContent, SupportedLanguage.CSharp);
			cu.AcceptVisitor(converter, null);
			return converter.Cu;
		}
	}
}