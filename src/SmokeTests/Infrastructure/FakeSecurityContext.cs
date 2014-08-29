using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using FubuMVC.Core.Security;

namespace Chpokk.Tests.Infrastructure {
	public class FakeSecurityContext : ISecurityContext {
		public bool IsAuthenticated() {
			return CurrentUser != null;
		}

		public IIdentity CurrentIdentity { get; private set; }
		public IPrincipal CurrentUser { get; set; }

		public string UserName {
			get { return CurrentIdentity!=null? CurrentIdentity.Name : null; }
			set { 
				if (value != null) {
					CurrentIdentity = new GenericIdentity(value);
					CurrentUser = new GenericPrincipal(CurrentIdentity, null);
				}
				else {
					CurrentIdentity = null;
					CurrentUser = null;
				}
			}
		}

		public FakeSecurityContext() {
			Console.WriteLine("I am constructed");
		}
	}
}
