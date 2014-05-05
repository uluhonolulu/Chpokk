using System;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using MbUnit.Framework;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingTheSolutionFile : BaseSolutionBrowserTest<SingleSolutionWithProjectFileContext> {
		[Test]
		public void ShouldSeeTheProject() {
			Assert.AreEqual(1, SolutionItem.Children.Count);
		}

		[Test]
		public void PathShouldBeRelativeToRepositoryRoot() {
			SolutionItem.PathRelativeToRepositoryRoot.ShouldBe(Context.RelativeSolutionPath);
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

	public class SingleSolutionWithProjectFileContext : SingleSlnFileContext {
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";

		private readonly string _slnFileContent =
			@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{1}"", ""{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}""
			EndProject";

		private readonly string _projectFileContent =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				</Project>";

		public string ProjectFilePath { get; set; }

		public string ProjectPathRelativeToRepositoryRoot {
			get { return ProjectFilePath.PathRelativeTo(RepositoryRoot); }
		}

		public string ProjectFolderRelativeToRepositoryRoot {
			get { return ProjectFolder.PathRelativeTo(RepositoryRoot); }
		}

		public string ProjectFolder { get; set; }

		public override void CreateSolutionFile(string filePath) {
			var fileSystem = Container.Get<FileSystem>();
			fileSystem.WriteStringToFile(filePath, string.Format(_slnFileContent, PROJECT_NAME, PROJECT_PATH));
			ProjectFilePath = FileSystem.Combine(filePath.ParentDirectory(), PROJECT_PATH);
			ProjectFolder = ProjectFilePath.ParentDirectory();
			Console.WriteLine("Writing the project to " + ProjectFilePath);
			fileSystem.WriteStringToFile(ProjectFilePath, _projectFileContent);
		}

		public string GetFilePath(string pathRelativeToProject) {
			return ProjectFolder.AppendPath(pathRelativeToProject);
		}
	}
}
