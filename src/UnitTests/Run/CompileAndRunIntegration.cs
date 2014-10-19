using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Compilation;
using FubuCore;
using FubuMVC.Core.Ajax;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Run {
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
