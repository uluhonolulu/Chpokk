using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using ChpokkWeb.Features.RepositoryManagement;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Shouldly;
using System.Linq;
using FubuCore;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class CreatingASimpleExeSolution : BaseCommandTest<RepositoryCleanupContext> {
		public const string NAME = "Carramba";
		[Test]
		public void ProjectShouldHaveAProgramItem() {
			var project = ProjectCollection.GlobalProjectCollection.LoadProject(ProjectPath);
			project.GetItems("Compile").Count.ShouldBe(1);
			project.GetItems("Compile").First().EvaluatedInclude.ShouldBe("Program.cs");
			var root = ProjectRootElement.Open(ProjectPath);
			root.Items.Count(element => element.ItemType == "Compile").ShouldBe(1);
		}

		[Test]
		public void ProgramFileShouldBeCreated() {
			var filePath = Path.Combine(ProjectPath.ParentDirectory(), "Program.cs");
			File.Exists(filePath).ShouldBe(true);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddSimpleProjectEndpoint>();
			endpoint.DoIt(new AddSimpleProjectInputModel { ProjectName = NAME, OutputType = "Exe", Language = SupportedLanguage.CSharp });
		}

		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }

		private string SolutionPath {
			get { return RepositoryManager.NewGetAbsolutePathFor(NAME, NAME + ".sln"); }
		}

		private string ProjectPath {
			get { return RepositoryManager.NewGetAbsolutePathFor(NAME, Path.Combine(NAME, NAME + ".csproj")); }
		}

	}
}
