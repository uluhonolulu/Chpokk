using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Compilation;
using FubuMVC.Core.Ajax;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using LibGit2Sharp.Tests.TestHelpers;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class EmptyCode : BaseQueryTest<ProjectWithSingleRootFileContext, AjaxContinuation> {
		[Test]
		public void ShouldBeSuccessfull() {
			Result.Success.ShouldBeTrue();
		}

		public override AjaxContinuation Act() {
			var endpoint = Context.Container.Get<CompilerEndpoint>();
			return endpoint.DoIt(new CompileInputModel(){PhysicalApplicationPath = Context.AppRoot, ProjectPath = Context.ProjectPath.PathRelativeTo(Context.AppRoot)});
		}
	}
}
