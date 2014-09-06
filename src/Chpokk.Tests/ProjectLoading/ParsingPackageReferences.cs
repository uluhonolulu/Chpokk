using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.ProjectLoading {
	[TestFixture]
	public class ParsingPackageReferences : BaseQueryTest<ProjectContentWithOnePackageReferenceContext, IEnumerable<string>> {
		[Test]
		public void ShouldReturnTheNameOfThePackage() {
			var packageName = Result.Single();
			packageName.ShouldBe(Context.PACKAGE_NAME);
		}

		public override IEnumerable<string> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			return parser.GetPackageReferences(Context.PROJECT_FILE_CONTENT);
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
}
