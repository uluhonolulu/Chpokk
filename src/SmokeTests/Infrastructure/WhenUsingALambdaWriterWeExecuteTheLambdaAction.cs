using System;
using System.Collections.Generic;
using System.Text;
using ChpokkWeb.Infrastructure;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Infrastructure {
	[TestFixture]
	public class WhenUsingALambdaWriterWeExecuteTheLambdaAction {
		private const string TestString = "hey";

		[Test]
		public void LambdaAssignsTheValue() {
			var stringToTest = string.Empty;
			new LambdaTextWriter(c => stringToTest += c).Write(TestString);

			stringToTest.ShouldBe(TestString);
		}
	}
}
