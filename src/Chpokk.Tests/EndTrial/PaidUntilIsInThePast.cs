using System;
using System.Collections.Generic;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;
using TypeMock.ArrangeActAssert;

namespace Chpokk.Tests.EndTrial {
	[TestFixture, Isolated]
	public class PaidUntilIsInThePast : BaseEndTrial {
		private readonly DateTime DATE2014 = DateTime.Parse("2014-01-01");
		private readonly DateTime DATE2015 = DateTime.Parse("2015-01-01");

		[Test]
		public void ShouldRedirect() {
			Result.ShouldBe(true);
		}

		protected override dynamic GetTestUser() {
			return new { PaidUntil = DATE2014, Status = "not null"};
		}

		public override bool Act() {
			Isolate.WhenCalled(() => DateTime.Now).WillReturn(DATE2015);
			return base.Act();
		}
	}
}
