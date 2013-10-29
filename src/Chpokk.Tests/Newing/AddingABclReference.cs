using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Newing {
	public class AddingABclReference: BaseQueryTest<SimpleConfiguredContext, IEnumerable<string>> {
		private const string ASSEMBLY_NAME = "System.Data";

		[Test]
		public void TheReferenceIsThere() {
			Result.First().ShouldBe(ASSEMBLY_NAME);
		}

		public override IEnumerable<string> Act() {
			var parser = Context.Container.Get<ProjectParser>();
			var projectRoot = parser.CreateProject("Exe", SupportedLanguage.CSharp);
			parser.AddReference(projectRoot, ASSEMBLY_NAME);
			return from item in projectRoot.Items where item.ItemType == "Reference" select item.Include;
		}


	}
}
