using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.ProjectTemplates;
using ChpokkWeb.Infrastructure.Windows;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Win32;
using Shouldly;

namespace Chpokk.Tests.Newing.ProjectTemplates {
	[TestFixture]
	public class ListOfPreinstalledPackages : BaseQueryTest<RepositoryFolderContext, TemplatePackages> {
		[Test]
		public void ShouldHavePathToPackageFolder() {
			Result.PackageRepositoryPath.ShouldBe(@"C:\Program Files (x86)\Microsoft ASP.NET\ASP.NET MVC 4\Packages\");
		}

		[Test]
		public void ShouldListPackageIDs() {
			Result.Packages.ShouldContain(package => package.PackageId == "EntityFramework");
		}

		public override TemplatePackages Act() {
			var templateContent =
				@"<VSTemplate Version=""3.0.0"" xmlns=""http://schemas.microsoft.com/developer/vstemplate/2005"" Type=""Project"">
				  <WizardData>
					<packages repository=""registry"" keyName=""AspNetMvc4VS12"" isPreunzipped=""true""><package id=""EntityFramework"" version=""5.0.0"" skipAssemblyReferences=""true"" /></packages>
				  </WizardData>
				</VSTemplate>";
			var template = new Template(templateContent);

			var templateSource =
				"<packages repository=\"registry\" keyName=\"AspNetMvc4VS12\" isPreunzipped=\"true\"><package id=\"EntityFramework\" version=\"5.0.0\" skipAssemblyReferences=\"true\" /></packages>";
			return template.TemplatePackages;
		}


	}


}
