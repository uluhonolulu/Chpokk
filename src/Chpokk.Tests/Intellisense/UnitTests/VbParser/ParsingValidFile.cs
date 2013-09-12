using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Editor.Parsing;
using FubuMVC.Core.Ajax;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Intellisense.UnitTests.VbParser {
	[TestFixture]
	public class ParsingValidFile: BaseQueryTest<SimpleConfiguredContext, AjaxContinuation> {
		[Test]
		public void ReturnsSuccess() {
			if (!Result.Success) {
				Console.WriteLine(Result.Errors[0].message);
				Console.WriteLine();
			}
			Assert.IsTrue(Result.Success);
		}

		public override AjaxContinuation Act() {
			var controller = Context.Container.Get<ParserEndpoint>();
			var content = "Class A" + Environment.NewLine + "End Class";
			var model = new ParserInputModel() {Content = content, PathRelativeToRepositoryRoot = "stuff.vb", PhysicalApplicationPath = Context.AppRoot, RepositoryName = String.Empty};
			return controller.Parse(model);
		}
	}
}
