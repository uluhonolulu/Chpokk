using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTests.Infrastructure;

namespace Chpokk.Tests.Infrastructure {
	public class SimpleAuthenticatedContext: SimpleConfiguredContext {
		public override void Create() {
			base.Create();
			this.FakeSecurityContext.UserName = "ulu";
		}

		public override void Dispose() {
			base.Dispose();
			this.FakeSecurityContext.UserName = null;
		}
	}
}
