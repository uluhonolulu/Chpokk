using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using System.Linq;
using NuGet;
using Shouldly;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class ParsingPackageReferences : BaseQueryTest<ProjectFileWithOnePackageReferenceContext, IEnumerable<dynamic>> {
		[Test]
		public void ShouldReturnTheNameOfThePackage() {
			var referencedPackageInfo = Result.Single();
			var packageName = referencedPackageInfo.Name;
			((string) packageName).ShouldBe(Context.PACKAGE_NAME);
			((bool) referencedPackageInfo.Selected).ShouldBe(true);
		}

		public override IEnumerable<dynamic> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			//var package = new DataServicePackage() {Id = Context.PACKAGE_NAME}; //new InMemoryPackage();
			//foreach (var reference in package.AssemblyReferences) {
			//	Console.WriteLine(reference.Name);
			//}
			var package = new InMemoryPackage() {AssemblyReferences = new List<IPackageAssemblyReference>(), Id = Context.PACKAGE_NAME};
			return parser.GetPackageReferences(Context.ProjectPath, new[]{package});
		}
	}

	public class ProjectContentWithOnePackageReferenceContext : SimpleConfiguredContext {
		public string PACKAGE_NAME = "Autofac";
		public string PROJECT_FILE_CONTENT;

		public ProjectContentWithOnePackageReferenceContext() {
			PROJECT_FILE_CONTENT = @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Reference Include=""{0}"">
				        <HintPath>..\packages\Autofac.3.3.0\lib\net40\{0}.dll</HintPath>
					</Reference>
				  </ItemGroup>
				</Project>".ToFormat(PACKAGE_NAME);
		}
	}

	public class ProjectFileWithOnePackageReferenceContext: ProjectFileContext {
		readonly ProjectContentWithOnePackageReferenceContext contentContext = new ProjectContentWithOnePackageReferenceContext();
		public string PACKAGE_NAME;
		public override string ProjectFileContent {
			get {
				return contentContext.PROJECT_FILE_CONTENT;
			}
		}
		public override void Create() {
			base.Create();
			PACKAGE_NAME = contentContext.PACKAGE_NAME;
		}
	}

	public class InMemoryPackage: LocalPackage {
		public new IEnumerable<IPackageAssemblyReference> AssemblyReferences  { get; set; }

		public override Stream GetStream() {
			return null;
		}

		protected override IEnumerable<IPackageFile> GetFilesBase() {
			yield break;
		}

		protected override IEnumerable<IPackageAssemblyReference> GetAssemblyReferencesCore() {
			return AssemblyReferences;
		}
	}
}
