using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using ICSharpCode.NRefactory;
using Microsoft.Build.Construction;

namespace Chpokk.Tests.Newing {
	public class SolutionWithTwoProjectsContext : SingleSolutionWithProjectFileContext {
		public ProjectRootElement NewProject { get; private set; }

		public override void Create() {
			base.Create();
			var newProjectName = "trololo";
			var projectParser = this.Container.Get<ProjectParser>();
			var newProjectPath = this.SolutionFolder.AppendPath(newProjectName).AppendPath(newProjectName + ".csproj");
			NewProject = projectParser.CreateProject("Library", SupportedLanguage.CSharp, newProjectPath);
		}
	}
}
