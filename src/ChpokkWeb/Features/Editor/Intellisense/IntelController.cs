using System;
using System.IO;
using System.Linq;
using FubuMVC.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntelController {
		[JsonEndpoint]
		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			if (input.Text == null) return null;
			var resolver = new NRefactoryResolver(LanguageProperties.CSharp);




			var projectContent = DefaultProjectContent;
			TextReader textReader = new StringReader(input.Text);
			var compilationUnit = Compile(projectContent, textReader);

			var classContent = @"public class A {
									void B(){
									}
								}";
			Compile(projectContent, new StringReader(classContent));

			var text = input.Text;//.Insert(input.Position, input.NewChar.ToString());
			var parseInformation =  new ParseInformation(compilationUnit);
			var expression = FindExpression(text, input.Position, parseInformation);
			var resolveResult = resolver.Resolve(expression, parseInformation, text);
			Console.WriteLine(input.Text);
			Console.WriteLine(compilationUnit);
			Console.WriteLine(expression);
			Console.WriteLine(resolveResult);
			if (resolveResult == null) {
				return new IntelOutputModel{Message = "ResolveResult is null"};
			}
			var completionData = resolveResult.GetCompletionData(projectContent);
			if (completionData == null) {
				return new IntelOutputModel{Message = "Completion Data is null"};
			}

			var items = from entry in completionData.OfType<IMember>() select new IntelOutputModel.IntelModelItem {Text = entry.Name, EntityType = entry.EntityType};
			var model = new IntelOutputModel {Message = input.Message, Items = items.Distinct().ToArray()};
			return model;
		}

		private ICompilationUnit Compile(DefaultProjectContent projectContent, TextReader textReader) {
			ICompilationUnit compilationUnit;
			using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, textReader)) {
				parser.ParseMethodBodies = false;
				parser.Parse();
				compilationUnit = this.ConvertCompilationUnit(parser.CompilationUnit, projectContent);
			}
			projectContent.UpdateCompilationUnit(null, compilationUnit, "fakefile.cs");
			return compilationUnit;
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