using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using ChpokkWeb.Features.RepositoryManagement;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
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
		}

		[Test]
		public void ProgramFileShouldBeCreated() {
			var filePath = Path.Combine(ProjectPath.ParentDirectory(), "Program.cs");
			File.Exists(filePath).ShouldBe(true);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddSimpleProjectEndpoint>();
			endpoint.DoIt(new AddSimpleProjectInputModel { PhysicalApplicationPath = Context.AppRoot, RepositoryName = NAME, OutputType = "EXE" });
		}

		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }

		private string SolutionPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, NAME + ".sln"); }
		}

		private string ProjectPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, NAME + ".csproj")); }
		}

	}
}
