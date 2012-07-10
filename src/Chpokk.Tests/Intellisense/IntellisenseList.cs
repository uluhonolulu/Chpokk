using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Editor.Intellisense;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Intellisense {
	[TestFixture]
	public class IntellisenseList : BaseQueryTest<SolutionWithProjectAndClassFileContext, IntelOutputModel> {
		[Test]
		public void ContainsTheMethodOfTheClass() {
			Console.WriteLine(Result.Message);
			var items = Result.Items;
			var memberNames = items.Select(item => item.Text);
			Assert.Contains(memberNames, "B");
		}

		public override IntelOutputModel Act() {
			var controller = Context.Container.Get<IntelController>();
			var source = "{public class X {public void Y(){new A().}}}";
			var position = source.IndexOf('.');
			var model = new IntelInputModel() {NewChar = '.', Position = position, Text = source};
			return controller.GetIntellisenseData(model);
		}
	}
}
