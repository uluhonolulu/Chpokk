using System;
using System.IO;
using System.Linq;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb;
using ChpokkWeb.Features.Editor.Intellisense;
using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
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
			Assert.Contains<string>(members.ToArray(), "ToString");
			var toStrings = from member in members where member == "ToString" select member;
			Assert.AreEqual(1, toStrings.Count());
		}

		IUrlRegistry Registry {
			get { return Context.Container.Get<IUrlRegistry>(); }
		}

		public override IntelOutputModel Act() {
			const string text = "using System;\r\nclass AClass\r\n{\r\n void B()\r\n {\r\n  string x;\r\n  x.\r\n }\r\n}\r\n";
			var session = new TestSession();
			var projectPathRelativeToRepositoryRoot = Path.Combine(Context.SOLUTION_FOLDER, Context.PROJECT_PATH);
			var position = text.IndexOf('.');
			var inputModel = new IntelInputModel {Text = text, Position = position, NewChar = '.', ProjectPath = projectPathRelativeToRepositoryRoot, RepositoryName = Context.REPO_NAME};
			var url = Registry.UrlFor<IntelInputModel>();
			return session.PostJson<IntelOutputModel>(url, inputModel, encodeRequest:true);
		}
	}
}
