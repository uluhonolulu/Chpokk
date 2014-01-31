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
	public class CompileAndRunIntegration : BaseCommandTest<BuildableProjectWithExeOutput> {
		[Test, Ignore("Don't know how to test it")]
		public void ShouldReturnASuccessMessage() {
			//if (!Result.Success) {
			//	Console.WriteLine(Result.Message);
			//}
			//Result.Success.ShouldBe(true);
			
		}

		[Test, Ignore("Don't know how to test it")]
		public void ShouldReturnTheOutput() {
			//Result.Message.ShouldStartWith("message");
		}

		public override void Act() {
			var endpoint = Context.Container.Get<CompilerEndpoint>();
			endpoint.CompileAndRun(new CompileAndRunInputModel { PhysicalApplicationPath = Context.AppRoot, ProjectPath = Context.ProjectPath.PathRelativeTo(Context.RepositoryRoot), RepositoryName = Context.REPO_NAME });
		}
	}
}
