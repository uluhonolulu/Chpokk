using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using System.Linq;
using Shouldly;
using UnitTests.References;

namespace Chpokk.Tests.References {
	[TestFixture, DependsOn(typeof(AddingABclReference))]
	public class AddingAFileBasedReference : BaseQueryTest<ProjectFileContext, ProjectItemElement> {
		private const string ASSEMBLY_PATH = @"packages\my.dll";
		[Test]
		public void IncludeShouldHaveTheAssemblyName() {
			Result.Include.ShouldBe("my");
		}

		[Test]
		public void HintPathShouldPointToTheAssemblysRelativeLocation() {
			var hintElement = Result.Metadata.FirstOrDefault(element => element.Name == "HintPath");
			hintElement.ShouldNotBe(null);
			hintElement.Value.ShouldBe(@"..\" + ASSEMBLY_PATH);
		}


		public override ProjectItemElement Act() {
			var parser = Context.Container.Get<ProjectParser>();
			var project = ProjectRootElement.Open(Context.ProjectPath);
			var assemblyPath = Path.Combine(Context.SolutionFolder, ASSEMBLY_PATH);
			Console.WriteLine("Assembly is located at " + assemblyPath);
			parser.AddReference(project, assemblyPath);

			return project.Items.First(element => element.ItemType == "Reference");
		}
	}
}
