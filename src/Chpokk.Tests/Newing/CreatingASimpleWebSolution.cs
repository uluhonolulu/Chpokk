using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class CreatingASimpleWebSolution : BaseCommandTest<RepositoryCleanupContext> {
		public const string NAME = "Carramba";

		[Test]
		public void ProjectTypeShouldBeLibrary() {
			var root = ProjectRootElement.Open(ProjectPath);
			root.Properties.First(element => element.Name == "OutputType").Value.ShouldBe("Library");
		}

		[Test]
		public void ProjectShouldHaveAWebconfigItem() {
			var project = ProjectCollection.GlobalProjectCollection.LoadProject(ProjectPath);
			project.GetItems("Content").Count.ShouldBe(1);
			project.GetItems("Content").First().EvaluatedInclude.ShouldBe("Web.config");
		}

		[Test]
		public void WebconfigFileShouldBeCreated() {
			var filePath = Path.Combine(ProjectPath.ParentDirectory(), "Web.config");
			File.Exists(filePath).ShouldBe(true);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddSimpleProjectEndpoint>();
			endpoint.DoIt(new AddSimpleProjectInputModel { ProjectName = NAME, OutputType = "Web", Language = SupportedLanguage.CSharp});
		}
		private string ProjectPath {
			get { return RepositoryManager.NewGetAbsolutePathFor(NAME, Path.Combine(NAME, NAME + ".csproj")); }
		}

		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }


	}
}
