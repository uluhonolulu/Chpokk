using System;
using System.Web.Hosting;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Remotes;
using FubuMVC.Core.Urls;
using Ivonna.Framework;
using Ivonna.Framework.Generic;
using MbUnit.Framework;

namespace Chpokk.Tests.GitHub {
	[TestFixture, RunOnWeb]
	public class SendingTheCloneCommand : BaseCommandTest<SimpleConfiguredContext> {
		private TestSession _session;
		private string _url;

		[Test]
		public void Test() {
			Assert.DoesNotThrow(() => _session.Post(_url, new CloneInputModel{RepoUrl = "stub"}));
		}

		[FixtureSetUp]
		public override void Arrange() {
			base.Arrange();
		}

		//[TestedMethod]
		public override void Act() {
			var env = HostingEnvironment.IsHosted;
			_session = new TestSession();
			Assert.IsNotNull(Context);
			var registry = Context.Container.Get<IUrlRegistry>();
			_url = registry.UrlFor<CloneInputModel>();
			Console.WriteLine(_url);
			_session.Stub<CloneController>("CloneRepository").Return(new FakeRepository());
			Assert.DoesNotThrow(() => _session.Post(_url, new CloneInputModel{RepoUrl = "stub"}));

		}

		class FakeRepository:IDisposable {
			public void Dispose() {}
		}
	}

	public class TestedMethodAttribute : Attribute {}
}
