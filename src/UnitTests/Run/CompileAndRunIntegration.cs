using Arractas;
using Chpokk.Tests.Run;
using ChpokkWeb.Features.Compilation;
using FubuCore;
using FubuMVC.Core.Ajax;
using MbUnit.Framework;
using Shouldly;

namespace UnitTests.Run {
	[TestFixture]
	public class CompileAndRunIntegration : BaseQueryTest<BuildableProjectWithExeOutput, AjaxContinuation> {
		[Test]
		public void ShouldReturnASuccessMessage() {
			Result.Success.ShouldBe(true);
		}

		[Test, Ignore("Covered by unit tests")]
		public void ShouldReturnTheOutput() {
			var logger = Context.Container.Get<ChpokkLogger>();
			Result.Message.ShouldStartWith("message");
		}

		public override AjaxContinuation Act() {
			var endpoint = Context.Container.Get<CompilerEndpoint>();
			return endpoint.CompileAndRun(new CompileAndRunInputModel {ProjectPath = Context.ProjectPath.PathRelativeTo(Context.RepositoryRoot), RepositoryName = Context.REPO_NAME, ConnectionId = "fakeId"});
		}
	}
}
