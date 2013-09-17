using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Compilation;
using FubuCore;
using FubuMVC.Core.Ajax;
using MbUnit.Framework;
using LibGit2Sharp.Tests.TestHelpers;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class InvalidCode: BaseQueryTest<ProjectWithInvalidCodeFileContext, AjaxContinuation> {
		[Test]
		public void ShouldReturnErrorStatus() {
			Result.Success.ShouldBeFalse();
		}

		public override AjaxContinuation Act() {
			var endpoint = Context.Container.Get<CompilerEndpoint>();
			return endpoint.Compile(new CompileInputModel() { PhysicalApplicationPath = Context.AppRoot, ProjectPath = Context.ProjectPath.PathRelativeTo(Context.RepositoryRoot), RepositoryName = Context.REPO_NAME });
		}
	}

	public class ProjectWithInvalidCodeFileContext : BuildableProjectWithSingleRootFileContext {
		public override void Create() {
			base.Create();
			Container.Get<IFileSystem>().WriteStringToFile(FilePath, "blah blah error");
		}
	}
}
