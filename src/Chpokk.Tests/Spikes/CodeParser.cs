using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using ICSharpCode.NRefactory.CSharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class CodeParser {
		[Test]
		public void Test() {
			var parser = new CSharpParser();
			parser.Parse("class yo { var x = string.Empty; }", "");
			//Console.WriteLine(parser.ErrorPrinter.Errors[0].Message);
			//Console.WriteLine(parser.ErrorPrinter.Errors[0].ErrorType);
			Assert.AreEqual(0, parser.ErrorPrinter.ErrorsCount);
			Assert.IsFalse(parser.HasErrors);
		}
	}
}
