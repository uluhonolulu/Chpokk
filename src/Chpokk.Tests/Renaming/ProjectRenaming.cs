using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using FubuCsProjFile;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;
using FubuCore;
using Shouldly;

namespace Chpokk.Tests.Renaming {
	[TestFixture]
	public class ProjectRenaming : BaseCommandTest<SingleSolutionWithProjectFileContext> {
		private const string NEW_PROJECT_NAME = "NewProjectName";
		[Test]
		public void NewPathShouldPonitToTheNewFile() {
			var newProjectRelativePath = Context.PROJECT_PATH.ParentDirectory().AppendPath(NEW_PROJECT_NAME + ".csproj");
			ProjectItem.Path.ShouldBe(newProjectRelativePath);
		}


		public override void Act() {
			var fileSystem = Context.Container.Get<IFileSystem>();
			var solutionContent = fileSystem.ReadStringFromFile(Context.SolutionPath);
			var newProjectRelativePath = Context.PROJECT_PATH.ParentDirectory().AppendPath(NEW_PROJECT_NAME + ".csproj");
			solutionContent = solutionContent.Replace(Context.PROJECT_PATH, newProjectRelativePath);
			fileSystem.WriteStringToFile(Context.SolutionPath, solutionContent);
		}

		private ProjectItem ProjectItem {
			get {
				var solutionParser = Context.Container.Get<SolutionParser>();
				//projectItem.Path is ProjectName\ProjectName.csproj
				var projectItem = solutionParser.GetProjectItems(Context.SolutionPath).First();
				return projectItem;
			}
		}
	}
}
