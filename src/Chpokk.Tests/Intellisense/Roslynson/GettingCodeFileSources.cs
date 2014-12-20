using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Shouldly;
using UnitTests.Roslynson;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class GettingCodeFileSources : BaseQueryTest<SolutionWithProjectAndClassFileContext, IEnumerable<string>> {
		[Test]
		public void ContainsTheSourceofTheCodeFile() {
			Result.FirstOrDefault().ShouldBe(Context.CLASS_CONTENT);
		}

		public override IEnumerable<string> Act() {
			var loader = Context.Container.Get<IntelDataLoader>();
			ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
			var root = ProjectRootElement.Open(Context.ProjectFilePath);
			var pathToExclude = string.Empty;
			return loader.GetSources(root, pathToExclude);
		}
	}
}
