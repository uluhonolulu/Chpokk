using System;
using System.IO;
using System.Linq;
using System.Web;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb;
using ChpokkWeb.Features.Editor.Intellisense;
using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
using ICSharpCode.NRefactory.Visitors;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using Ivonna.Framework.Generic;
using Ivonna.Framework;
using MbUnit.Framework;
using StructureMap;

namespace Chpokk.Tests.Intellisense {
	[TestFixture, RunOnWeb]
	public class RequestForIntelData : WebQueryTest<ProjectWithSingleRootFileContext, IntelOutputModel> {


		[Test]
		public void ShouldReturnToStringMethodForStrings() {
			var members = from item in Result.Items select item.Name;
			Assert.Contains(members.ToArray(), "ToString");
			var toStrings = from member in members where member == "ToString" select member;
			Assert.AreEqual(1, toStrings.Count());
		}

		IUrlRegistry Registry {
			get { return Context.Container.Get<IUrlRegistry>(); }
		}

		public override IntelOutputModel Act() {
			const string text = "class ABCClass { void BCD() {  string x;  x. }}\r\n";
			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("Exception")));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is HttpResponse));
			var projectPathRelativeToRepositoryRoot = Path.Combine(Context.SOLUTION_FOLDER, Context.PROJECT_PATH);
			var position = text.IndexOf('.');
			var inputModel = new IntelInputModel {Text = text, Position = position, NewChar = '.', ProjectPath = projectPathRelativeToRepositoryRoot, RepositoryName = Context.REPO_NAME};
			var url = Registry.UrlFor<IntelInputModel>();
			return session.PostJson<IntelOutputModel>(url, inputModel, encodeRequest:false);
		}
	}
}

//Unit testing: 
			//var compilationUnit = _compiler.Compile(projectContent, textReader); 
			//var expression = Compiler.FindExpression(text, input.Position, parseInformation);
//then check that:
//expression	{<x> ([DefaultExpressionContext: IdentifierExpected])}	ICSharpCode.SharpDevelop.Dom.ExpressionResult
