using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Compilation;
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
			if (!Result.Success) {
				Console.WriteLine(Result.Message);
			}
			Result.Success.ShouldBe(true);
			
		}

		[Test]
		public void ShouldReturnTheOutput() {
			Result.Message.ShouldStartWith("message");
		}

		public override AjaxContinuation Act() {
			var endpoint = Context.Container.Get<CompilerEndpoint>();
			return endpoint.CompileAndRun(new CompileAndRunInputModel { PhysicalApplicationPath = Context.AppRoot, ProjectPath = Context.ProjectPath.PathRelativeTo(Context.RepositoryRoot), RepositoryName = Context.REPO_NAME });
		}
	}
}
