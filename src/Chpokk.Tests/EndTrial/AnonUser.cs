using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.CustomerDevelopment.TimeToPay;
using FubuMVC.Core.Behaviors;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.EndTrial {
	[TestFixture]
	public class AnonUser : BaseEndTrial {
		[Test]
		public void ShouldNotBeRedirected() {
			Result.ShouldBe(false);
		}

		protected override dynamic GetTestUser() {
			return null;
		}

	}
}
