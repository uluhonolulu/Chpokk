using System;
using System.Web.Hosting;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes;
using FubuMVC.Core.Urls;
using Ivonna.Framework;
using Ivonna.Framework.Generic;
using LibGit2Sharp;
using MbUnit.Framework;
using System.Linq;

namespace Chpokk.Tests.GitHub {
	[TestFixture, RunOnWeb]
	public class SendingTheCloneCommand : BaseQueryTest<SimpleConfiguredContext, Spy<CloneInputModel>> {
		[Test]
		public void Test() {
			Assert.AreEqual(1, Result.Results.Count());
		}

		[FixtureSetUp]
		public override void Arrange() {
			base.Arrange();
		}

		//[TestedMethod]
		public override Spy<CloneInputModel> Act() {
			var spy = new Spy<CloneInputModel>(info => info.TypeName.EndsWith("CloneController") && info.MethodName == "CloneRepository", args => (CloneInputModel) args.ParameterValues[0]);
			CThruEngine.AddAspect(spy);
			CThruEngine.AddAspect(Stub.For<CloneController>("CloneRepository").Return(new RepositoryInfo("", "")));
			var url = Context.Container.Get<IUrlRegistry>().UrlFor<CloneInputModel>();
			new TestSession().Post(url, new CloneInputModel {RepoUrl = "stub"});
			return spy;
		}

	}

	//public class TestedMethodAttribute : Attribute {}
}
