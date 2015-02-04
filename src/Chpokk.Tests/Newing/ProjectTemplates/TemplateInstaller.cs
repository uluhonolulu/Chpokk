using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class TemplateInstaller: BaseCommandTest<RepositoryFolderContext> {
		private const string CONTENT_FILENAME = "App.config";
		private const string CONTENT_OTHERFILENAME = @"Properties\AssemblyInfo.cs";
		private const string PROJECT_NAME = "NewProjectName";

		[Test]
		public void CopiesTheRootFiles() {
			var targetFilePath = ProjectFolder.AppendPath(CONTENT_FILENAME);
			File.Exists(targetFilePath).ShouldBe(true);
		}

		[Test]
		public void CopiesTheNestedFiles() {
			var targetFilePath = ProjectFolder.AppendPath(CONTENT_OTHERFILENAME);
			File.Exists(targetFilePath).ShouldBe(true);			
		}

		[Test, DependsOn("CopiesTheRootFiles")]
		public void CopiedFilesShouldBeProcessed() {
			var targetFilePath = ProjectFolder.AppendPath(CONTENT_FILENAME);
			var processedContent = Context.Container.Get<FileSystem>().ReadStringFromFile(targetFilePath);
			processedContent.Contains("$if$").ShouldBe(false);
		}

		[Test]
		public void ProjectFileShouldBeRenamed() {
			var projectFilePath = ProjectFolder.AppendPath(PROJECT_NAME + ".csproj");
			File.Exists(projectFilePath).ShouldBe(true);			
		}

		public override void Act() {
			var templateRelativePath = @"SystemFiles\Templates\ProjectTemplates\CSharp\Windows\1033\ConsoleApplication\csConsoleApplication.vstemplate";
			var templatePath = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath(templateRelativePath);
			var projectPath = Context.Container.Get<RepositoryManager>()
			                            .NewGetAbsolutePathFor(Context.REPO_NAME, PROJECT_NAME.AppendPath(PROJECT_NAME + ".csproj"));
			Context.Container.Get<ChpokkWeb.Features.ProjectManagement.TemplateInstaller>().CreateProjectFromTemplate(projectPath, templatePath);
		}


		private string ProjectFolder {
			get { return Context.RepositoryRoot.AppendPath(PROJECT_NAME); }
		}
	}
}
