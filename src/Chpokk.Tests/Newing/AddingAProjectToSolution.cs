using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddProject;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;
using Shouldly;
using FubuCore;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class AddingAProjectToSolution : BaseCommandTest<EmptySlnFileContext> {
		private const string PROJECT_NAME = "AiLuLu";
		[Test]
		public void SolutionShouldContainAProject() {
			var solutionPath = Context.SolutionPath;
			var projectItems = Context.Container.Get<SolutionParser>().GetProjectItems(solutionPath);
			projectItems.Count().ShouldBe(1);
		}

		[Test]
		public void ProjectFileShouldBeCreated() {
			var projectPath = Context.SolutionFolder.AppendPath(@"AiLuLu\AiLuLu.csproj");
			File.Exists(projectPath).ShouldBe(true);
		}

		public override void Act() {
			var model = new AddProjectInputModel
				{
					Language = SupportedLanguage.CSharp,
					OutputType = "Exe",
					RepositoryName = Context.REPO_NAME,
					ProjectName = PROJECT_NAME,
					SolutionPath = Context.SolutionPath.PathRelativeTo(Context.RepositoryRoot) //TODO:relative path
				};
			Console.WriteLine("SolutionPath is: " + model.SolutionPath);
			var endpoint = Context.Container.Get<AddProjectEndpoint>();
			endpoint.DoIt(model);

		}
	}
}
