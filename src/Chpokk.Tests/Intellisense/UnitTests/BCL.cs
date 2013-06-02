using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.ProjectManagement;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

//looks like this is actually a spike
namespace Chpokk.Tests.Intellisense.UnitTests {
	[TestFixture]
	public class CSharpBclReference : BaseQueryTest<SimpleConfiguredContext, ResolveResult> {
		[Test]
		public void ShouldDisplayIntellisenseForStrings() {
			Assert.IsTrue(Result.IsValid);
		}

		public override ResolveResult Act() {
			const string text = "class ABCClass {  void BCD()  {   string x; x. } }\r\n"; //important to end it with newline
			//const string text = "using System; class AClass {  void B()  {   BClass x; x. } }";
			var textReader = new StringReader(text);
			var compiler = Context.Container.Get<Compiler>();
			var projectContent = new DefaultProjectContent() { Language = LanguageProperties.CSharp };
			var compilationUnit = compiler.ParseCode(projectContent, textReader, SupportedLanguage.CSharp);
			var parseInformation =  new ParseInformation(compilationUnit);
			var _resolver = Context.Container.Get<NRefactoryResolver>();
			var expression = Compiler.FindExpression(text, text.IndexOf('.'), parseInformation, SupportedLanguage.CSharp);
			var resolveResult = _resolver.Resolve(expression, parseInformation, text);
			return resolveResult;
		}
	}

	public class VbNetBclReference : BaseQueryTest<SimpleConfiguredContext, ResolveResult> {
		[Test]
		public void ShouldDisplayIntellisenseForStrings() {
			Assert.IsTrue(Result.IsValid);
		}

		public override ResolveResult Act() {
			const string text = "Class ABCClass \r\n  Sub BCD()  \r\n Dim x = \"\" \r\n x.\r\n End Sub \r\n End Class\r\n"; //important to end it with newline
			//const string text = "using System; class AClass {  void B()  {   BClass x; x. } }";
			var textReader = new StringReader(text);
			var compiler = Context.Container.Get<Compiler>();
			var pcRegistry = new ProjectContentRegistry();

			var projectContent = new DefaultProjectContent() { Language = LanguageProperties.VBNet };
			projectContent.AddReferencedContent(pcRegistry.Mscorlib);
			var compilationUnit = compiler.ParseCode(projectContent, textReader, SupportedLanguage.VBNet);
			var parseInformation =  new ParseInformation(compilationUnit);
			var _resolver = new NRefactoryResolver(LanguageProperties.VBNet);
			var expression = Compiler.FindExpression(text, text.IndexOf("x.") + 1, parseInformation, SupportedLanguage.VBNet);
			var resolveResult = _resolver.Resolve(expression, parseInformation, text);
			return resolveResult;	
		}
	}
}
