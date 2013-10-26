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
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class GettingLocalReferences : BaseQueryTest<SimpleConfiguredContext, IEnumerable<string>> {
		private const string PROJECT_PATH = @"C:\src\proj.csproj";
		private const string RELATIVE_DLL_PATH = @"..\local.dll";
		private const string ASSEMBLY_NAME = "irrelevant";
		private const string ABSOLUTE_DLL_PATH = @"C:\local.dll";

		[Test]
		public void ReferencePathsShouldIncludeBclReferencesInProjectFile() {
			Result.ShouldContain(ABSOLUTE_DLL_PATH);
		}

		public override IEnumerable<string> Act() {
			var loader = Context.Container.Get<IntelDataLoader>();
			var root = ProjectRootElement.Create();
			root.FullPath = PROJECT_PATH;
			var reference = root.AddItem("Reference", ASSEMBLY_NAME, new Dictionary<string, string>() { { "HintPath", RELATIVE_DLL_PATH } });
			return loader.GetReferencePaths(root);
		}
	}
}
