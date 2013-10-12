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
using Microsoft.Build.Evaluation;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class CreatingASimpleVbExeSolution : BaseCommandTest<RepositoryCleanupContext> {
		public const string NAME = "Carramba";
		[Test]
		public void ProducesABuildableProject() {
			new Project(ProjectPath).Build(new CreatingASimpleSolution.ConsoleBuildLogger()).ShouldBe(true);
			var binPath = RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, "bin"));
			Directory.Exists(binPath).ShouldBe(true);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddSimpleProjectEndpoint>();
			endpoint.DoIt(new AddSimpleProjectInputModel { PhysicalApplicationPath = Context.AppRoot, RepositoryName = NAME, OutputType = "Exe", Language = SupportedLanguage.VBNet });
		}

		private string ProjectPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, NAME + ".vbproj")); }
		}
		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }
	}
}
