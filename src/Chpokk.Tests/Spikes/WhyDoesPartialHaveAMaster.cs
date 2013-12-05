using System;
using System.Collections.Generic;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class WhyDoesPartialHaveAMaster {
		[Test]
		public void Test() {
			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TypeName.Contains("Spark")));
			session.AddAspect(new TraceAspect(info => info.MethodName == "SetConfigurationSystem"));
			session.Get("Main");
		}

		public void Execute() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.StartsWith("System.Web.Hosting.HostingEnvironment") , @"C:\logk.txt"));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("ConfigurationManager"), @"C:\logk.txt"));
			CThruEngine.AddAspect(new DebugAspect(info => info.TypeName.EndsWith("System.Web.Configuration.HttpConfigurationSystem")));
			CThruEngine.StartListening();
			Console.WriteLine("------------------");
		}
	}
}
