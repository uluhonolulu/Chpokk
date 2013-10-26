using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class GettingBclReferences: BaseQueryTest<SimpleConfiguredContext, IEnumerable<string>> {
		[Test]
		public void ReferencePathsShouldIncludeBclReferencesInProjectFile() {
			foreach (var reference in Result) {
				Console.WriteLine(reference);
			}
			Result.ShouldContain(s => s.EndsWith("System.Data.dll") && File.Exists(s));
		}

		public override IEnumerable<string> Act() {
			var loader = Context.Container.Get<IntelDataLoader>();
			var root = ProjectRootElement.Create();
			var reference = root.AddItem("Reference", "System.Data");
			return loader.GetReferencePaths(root);
		}
	}
}
