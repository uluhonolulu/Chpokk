using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.CustomerDevelopment.TimeToPay;

namespace Chpokk.Tests.EndTrial {
	public abstract class BaseEndTrial : BaseQueryTest<SimpleConfiguredContext, bool> {
		public override bool Act() {
			var behavior = Context.Container.Get<EndOfTrialTimeToPayBehavior>();
			var user = GetTestUser();
			return behavior.ShouldRedirect(user);
		}

		protected abstract dynamic GetTestUser();
	}
}
