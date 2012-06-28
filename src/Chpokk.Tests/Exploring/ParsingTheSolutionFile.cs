using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class ParsingTheSolutionFile : BaseSolutionBrowserTest<SingleSolutionContext> {
		[Test]
		public void ShouldSeeTheProject() {
			var projectItem = SolutionItem.Children.FirstOrDefault();
			Assert.IsNotNull(projectItem);
		}

		public RepositoryItem SolutionItem {
			get { return Result.First(); }
		}

		public RepositoryItem ProjectItem {
			get { return SolutionItem.Children.First(); }
		}
	}

	public class SingleSolutionContext : SingleSlnFileContext {
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";

		private readonly string _slnFileContent =
			@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{1}"", ""{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}""
			EndProject";

		public override void CreateSolutionFile(string filePath) {
			File.WriteAllText(filePath, string.Format(_slnFileContent, PROJECT_NAME, PROJECT_PATH));
		}


	}
}
