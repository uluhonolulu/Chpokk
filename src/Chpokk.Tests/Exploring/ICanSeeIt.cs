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

namespace Chpokk.Tests.Exploring {
	[TestFixture, RunOnWeb]
	public class ICanSeeIt {
		[Test]
		public void DoesntThrow() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("CompilerError"), 5));
			CThruEngine.AddAspect(new TraceAspect(info => info.MethodName.EndsWith("ReferencedAssemblies"), 5));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName == "Microsoft.CSharp.CSharpCodeGenerator" && info.MethodName == "Compile"));
			Assert.DoesNotThrow(() => new TestSession().Get("/Main"));  
		}
	}
}
