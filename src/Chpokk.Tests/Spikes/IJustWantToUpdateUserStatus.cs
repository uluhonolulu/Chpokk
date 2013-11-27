using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Authentication;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class IJustWantToUpdateUserStatus: BaseCommandTest<SimpleAuthenticatedContext> {
		[Test]
		public void Test() {
			var userManager = Context.Container.Get<UserManager>();
			var user = userManager.GetCurrentUser();
			user.Status = (string) UserStatus.Trial;
			user.PaidUntil = DateTime.Today.Add(TimeSpan.FromDays(10));
			userManager.UpdateUser(user);
		}

		public override void Act() {}
	}
}
