﻿using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Exploring.Rename;
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
		public void NewPathShouldPointToTheNewFile() {
			var newProjectRelativePath = Context.PROJECT_PATH.ParentDirectory().AppendPath(NEW_PROJECT_NAME + ".csproj");
			ProjectItem.Path.ShouldBe(newProjectRelativePath);
		}

		[Test]
		public void NewNameShouldBeTheNewProjectTitle() {
			ProjectItem.Name.ShouldBe(NEW_PROJECT_NAME);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<RenameEndpoint>();
			endpoint.DoIt(new RenameInputModel {
				RepositoryName = Context.REPO_NAME,
				PathRelativeToRepositoryRoot = Context.ProjectPathRelativeToRepositoryRoot,
				NewFileName = "New" + Context.PROJECT_NAME + ".csproj",
				SolutionPath = Context.RelativeSolutionPath,
				ItemType = "Project"
			});
			//var fileSystem = Context.Container.Get<IFileSystem>();
			//var solutionContent = fileSystem.ReadStringFromFile(Context.SolutionPath);
			//var newProjectRelativePath = Context.PROJECT_PATH.ParentDirectory().AppendPath(NEW_PROJECT_NAME + ".csproj");
			//solutionContent = solutionContent.Replace(Context.PROJECT_PATH, newProjectRelativePath).Replace("\"" + Context.PROJECT_NAME + "\"", "\"" + NEW_PROJECT_NAME + "\"");
			//fileSystem.WriteStringToFile(Context.SolutionPath, solutionContent);
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
