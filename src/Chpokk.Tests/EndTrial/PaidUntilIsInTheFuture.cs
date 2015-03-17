using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;
using TypeMock.ArrangeActAssert;

namespace Chpokk.Tests.EndTrial {
	[TestFixture, Isolated]
	public class PaidUntilIsInTheFuture : BaseEndTrial {
		private readonly DateTime DATE2016 = DateTime.Parse("2016-01-01");
		private readonly DateTime DATE2015 = DateTime.Parse("2015-01-01");
		[Test]
		public void ShouldNotRedirect() {
			Result.ShouldBe(false);
		}

		protected override dynamic GetTestUser() {
			return new { PaidUntil = DATE2016, Status = (string)null };
		}

		public override bool Act() {
			Isolate.WhenCalled(() => DateTime.Now).WillReturn(DATE2015);
			return base.Act();
		}
	}
}
