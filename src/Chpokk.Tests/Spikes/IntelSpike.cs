using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IParser = ICSharpCode.SharpDevelop.Project.IParser;
using FubuCore;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class IntelSpike {
		[Test]
		public void Test() {
			var pcRegistry = new ProjectContentRegistry();

			var projectContent = new DefaultProjectContent() { Language = LanguageProperties.CSharp };
			projectContent.AddReferencedContent(pcRegistry.Mscorlib);
			var source1 = @"public class A {
								void B(){
								}
							}";
			//var resolver = new NRefactoryResolver(LanguageProperties.CSharp);

			ICompilationUnit compilationUnit;

			using (ICSharpCode.NRefactory.IParser p = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(source1))) {
				p.ParseMethodBodies = false;

				p.Parse();
				compilationUnit = this.ConvertCompilationUnit(p.CompilationUnit, projectContent);
			}

			projectContent.UpdateCompilationUnit(null, compilationUnit, "fakefile1.cs");

	
			var source2 = @"public class C {
								void D(){
									new A().B();
								}
							}";

			using (ICSharpCode.NRefactory.IParser p = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(source2))) {
				p.ParseMethodBodies = false;

				p.Parse();
				compilationUnit = this.ConvertCompilationUnit(p.CompilationUnit, projectContent);
			}

			projectContent.UpdateCompilationUnit(null, compilationUnit, "fakefile2.cs");
			

			Assert.AreEqual(2, projectContent.Classes.Count);

			var parseInformation = new ParseInformation(compilationUnit);
			var text = @"public class C {
								void D(){
									new A().
								}
							}";

			var expression = FindExpression(text, text.IndexOf('.'), parseInformation);
			var resolver = new NRefactoryResolver(LanguageProperties.CSharp);
			var result = resolver.Resolve(expression, parseInformation, text);
			Assert.IsNotNull(result);
			Assert.AreEqual("A", result.ResolvedType.Name);
			Console.WriteLine(result.ResolvedType.Name);
			var completionData = result.GetCompletionData(projectContent);
			completionData.Cast<IMember>().Each(member => Console.WriteLine(member.Name));
			Console.WriteLine(result);
		}

		ICompilationUnit ConvertCompilationUnit(ICSharpCode.NRefactory.Ast.CompilationUnit cu, IProjectContent projectContent) {
			var converter = new NRefactoryASTConvertVisitor(projectContent, SupportedLanguage.CSharp);
			cu.AcceptVisitor(converter, null);
			return converter.Cu;
		}

		private ExpressionResult FindExpression(string text, int offset, ParseInformation parseInformation) {
			var finder = new CSharpExpressionFinder(parseInformation);
			var expression = finder.FindExpression(text, offset);
			return expression;
		}

		[Test]
		public void whatsupwiththecompilation() {
			var pcRegistry = new ProjectContentRegistry();

			var projectContent = new DefaultProjectContent() { Language = LanguageProperties.CSharp };
			projectContent.AddReferencedContent(pcRegistry.Mscorlib);

			var source = "public class X {public void Y(){}}";
			var unit = Compile(projectContent, new StringReader(source));

			Assert.AreEqual(1, unit.Classes.Count);
		}

		private ICompilationUnit Compile(DefaultProjectContent projectContent, TextReader textReader) {
			ICompilationUnit compilationUnit;
			using (ICSharpCode.NRefactory.IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, textReader)) {
				parser.ParseMethodBodies = false;
				parser.Parse();
				compilationUnit = this.ConvertCompilationUnit(parser.CompilationUnit, projectContent);
			}
			projectContent.UpdateCompilationUnit(null, compilationUnit, "fakefile.cs");
			return compilationUnit;
		}
	}
}
