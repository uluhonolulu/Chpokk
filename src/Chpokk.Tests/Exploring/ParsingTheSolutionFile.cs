using System;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using MbUnit.Framework;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingTheSolutionFile : BaseSolutionBrowserTest<SingleSolutionContext> {
		[Test]
		public void ShouldSeeTheProject() {
			Assert.AreEqual(1, SolutionItem.Children.Count);
		}

		[Test]
		public void CanSeeTheProjectsName() {
			Assert.AreEqual(Context.PROJECT_NAME, ProjectItem.Name);
		}

		[Test]
		public void DontWantToEditAProject () {
			Assert.AreEqual("folder", ProjectItem.Type);
		}

		public RepositoryItem SolutionItem {
			get { return Result.First(); }
		}

		public RepositoryItem ProjectItem {
			get { return SolutionItem.Children.First(); }
		}
	}

	public class SingleSolutionContext : SingleSlnFileContext {
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";

		private readonly string _slnFileContent =
			@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{1}"", ""{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}""
			EndProject";

		public override void CreateSolutionFile(string filePath) {
			var fileSystem = Container.Get<FileSystem>();
			fileSystem.WriteStringToFile(filePath, string.Format(_slnFileContent, PROJECT_NAME, PROJECT_PATH));
			var projectFilePath = FileSystem.Combine(filePath.ParentDirectory(), PROJECT_PATH);
			Console.WriteLine("Writing the project to " + projectFilePath);
			fileSystem.WriteStringToFile(projectFilePath, "<root/>");
		}
	}
}
