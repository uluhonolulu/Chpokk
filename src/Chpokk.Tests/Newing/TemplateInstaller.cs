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
		private FileSystem _fileSystem;
		private TemplateTransformer _templateTransformer;
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
			//actually, let's use the code from ProjectCreator
			var templateRelativePath = @"SystemFiles\Templates\ProjectTemplates\CSharp\Windows\1033\ConsoleApplication\csConsoleApplication.vstemplate";
			var projectPath = Context.Container.Get<RepositoryManager>()
			                            .NewGetAbsolutePathFor(Context.REPO_NAME, PROJECT_NAME.AppendPath(PROJECT_NAME + ".csproj"));
			var templatePath = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath(templateRelativePath);
			_fileSystem = Context.Container.Get<FileSystem>();
			_templateTransformer = Context.Container.Get<TemplateTransformer>();
			CreateProjectFromTemplate(projectPath, templatePath);
		}

		public string CreateProjectFromTemplate(string projectPath, string templatePath) {
			// from template
			var projectFolder = projectPath.ParentDirectory();
			var projectName = PROJECT_NAME;
			var projectTemplateFolder = templatePath.ParentDirectory();
			var replacements = new Dictionary<string, string>() { { "$safeprojectname$", projectName }, { "$targetframeworkversion$", "4.5" }, { "$guid1$", Guid.NewGuid().ToString() } };
			var template = new Template(templatePath);
			var projectItems = template.GetProjectItems();
			foreach (var projectItem in projectItems) {
				var templateFileRelativePath = projectItem.FileName;	//relative to template folder
				var templateFileSourcePath = projectTemplateFolder.AppendPath(templateFileRelativePath);
				var destinationRelativePath = projectItem.TargetFileName;
				var destinationPath = projectFolder.AppendPath(destinationRelativePath);
				var templateFileContent = _fileSystem.ReadStringFromFile(templateFileSourcePath);
				var processedContent = _templateTransformer.Evaluate(templateFileContent, replacements);
				_fileSystem.WriteStringToFile(destinationPath, processedContent);
				
			}

			var projectFileSourceRelativePath = template.ProjectFileName;
			var projectFileSourcePath = projectTemplateFolder.AppendPath(projectFileSourceRelativePath);
			var destinationProjectPath = projectFolder.AppendPath(projectName + Path.GetExtension(projectFileSourceRelativePath));
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFileSourcePath);
			var processedProjectContent = _templateTransformer.Evaluate(projectFileContent, replacements);
			_fileSystem.WriteStringToFile(destinationProjectPath, processedProjectContent);
			return null;
		}

		private string ProjectFolder {
			get { return Context.RepositoryRoot.AppendPath(PROJECT_NAME); }
		}
	}
}
