using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IParser = ICSharpCode.SharpDevelop.Project.IParser;

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
			//var parseInformation = new ParseInformation(compilationUnit);
			//Console.WriteLine(parseInformation.CompilationUnit.Classes.Count);
			var source2 = @"public class C {
								void D(){
								}
							}";

			using (ICSharpCode.NRefactory.IParser p = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(source2))) {
				p.ParseMethodBodies = false;

				p.Parse();
				compilationUnit = this.ConvertCompilationUnit(p.CompilationUnit, projectContent);
			}

			projectContent.UpdateCompilationUnit(null, compilationUnit, "fakefile2.cs");
			

			Assert.AreEqual(2, projectContent.Classes.Count);
		}

		ICompilationUnit ConvertCompilationUnit(ICSharpCode.NRefactory.Ast.CompilationUnit cu, IProjectContent projectContent) {
			var converter = new NRefactoryASTConvertVisitor(projectContent, SupportedLanguage.CSharp);
			cu.AcceptVisitor(converter, null);
			return converter.Cu;
		}
	}
}
