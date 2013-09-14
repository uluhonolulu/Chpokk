using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chpokk.Tests.Infrastructure {
	public class SimpleAuthenticatedContext: SimpleConfiguredContext, IDisposable {
		public override void Create() {
			base.Create();
			this.FakeSecurityContext.UserName = "ulu";
		}

		public virtual void Dispose() {
			this.FakeSecurityContext.UserName = null;
		}
	}
}
