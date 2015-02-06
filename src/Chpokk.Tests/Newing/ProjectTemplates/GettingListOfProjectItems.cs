using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.ProjectManagement;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;
using FubuCore;
using System.Linq;

namespace Chpokk.Tests.Newing.ProjectTemplates {
	[TestFixture]
	public class GettingListOfProjectItems : BaseQueryTest<SimpleConfiguredContext, IEnumerable<Template.ProjectItem>> {
		[Test]
		public void CanGetRootItems() {
			Result.ShouldContain(item => item.FileName == "Web.config" && item.TargetFileName == "Web.config");
			Result.ShouldContain(item => item.FileName == "MyWebExtension.vb" && item.TargetFileName == @"My Project\MyExtensions\MyWebExtension.vb");
		}

		[Test]
		public void CanGetItemsInSubfolders() {
			Result.ShouldContain(item => item.FileName == @"My Project\AssemblyInfo.vb" && item.TargetFileName == @"My Project\AssemblyInfo.vb");
		}

		public override IEnumerable<Template.ProjectItem> Act() {
			var templateContent =
				@"<VSTemplate Version=""3.0.0"" xmlns=""http://schemas.microsoft.com/developer/vstemplate/2005"" Type=""Project"">
				  <TemplateContent>
					<Project File=""WebApplication.vbproj"" ReplaceParameters=""true"">
					  <ProjectItem ReplaceParameters=""true"" TargetFileName=""Web.config"">Web.config</ProjectItem>
					  <ProjectItem ReplaceParameters=""true"" TargetFileName=""My Project\MyExtensions\MyWebExtension.vb"">MyWebExtension.vb</ProjectItem>
					  <Folder Name=""My Project"" TargetFolderName=""My Project"">
						<ProjectItem ReplaceParameters=""true"" TargetFileName=""AssemblyInfo.vb"">AssemblyInfo.vb</ProjectItem>
					  </Folder>
					</Project>
				  </TemplateContent>
				</VSTemplate>";
			var template = new Template(templateContent);
			var projectItems = template.GetProjectItems();
			foreach (var projectItem in projectItems) {
				Console.WriteLine(projectItem.FileName + " -> " + projectItem.TargetFileName);
			}
			return projectItems;
		}
	}


}
