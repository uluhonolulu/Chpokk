using System.Collections.Generic;
using System.Linq;
using Arractas;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using Shouldly;

namespace UnitTests.Roslynson {
	public class GettingCodeFileSources : BaseQueryTest<SolutionWithProjectAndClassFileContext, IEnumerable<string>> {
		[Test]
		public void ContainsTheSourceofTheCodeFile() {
			Result.FirstOrDefault().ShouldBe(Context.CLASS_CONTENT);
		}

		public override IEnumerable<string> Act() {
			var loader = Context.Container.Get<IntelDataLoader>();
			var root = ProjectRootElement.Open(Context.ProjectFilePath);
			var pathToExclude = string.Empty;
			return loader.GetSources(root, pathToExclude);
		}
	}
}
