using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Bottles.Diagnostics;
using CThru;
using CThru.BuiltInAspects;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.ObjectGraph;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using dotless.Core;

namespace Chpokk.Tests {
	[TestFixture, RunOnWeb]
	public class LessProblem {

		[Test]
		public void CanUseTheLessConfig() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Asset")));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Exception")));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PackageLog && info.MethodName == "MarkFailure", 5));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is AssetPath));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PackageAssets));
			//CThruEngine.AddAspect(new DebugAspect(info => info.MethodName == "matchingType"));
			Assert.DoesNotThrow(() => new TestSession().Get("/"));
		}

		[Test]
		public void CangetTheLessFile() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("LessEngine") && info.MethodName == ".ctor", 5));
			//CThruEngine.AddAspect(new LessDebugger());
			//Assert.DoesNotThrow(() => new TestSession().Get("/_content/styles/lib/reset.less"));
			var session = new TestSession();
			session.Get("/"); //warmup
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Asset") || info.TypeName.Contains("dotless")));
			CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "TransformToCss"));
			var response = session.Get("/_content/styles/lib/cbedfa0c7dd255710f3f38f5edf90b70.css");
			Assert.GreaterThan(response.BodyAsString.Length, 0);
		}

		class LessDebugger : CommonAspect {
			public LessDebugger() : base(info => info.TargetInstance is ServiceGraph && info.MethodName == "AddService") {}

			public override void MethodBehavior(DuringCallbackEventArgs e) {
				if (e.ParameterValues[0] is ObjectDef) {
					var def = (ObjectDef) e.ParameterValues[0];
					if (def.Value is LessEngine) {
						Console.WriteLine("->" + def.Value.ToString());
						if (Debugger.IsAttached) 
							Debugger.Break();
					}
				}
				//Console.WriteLine("-" + e.ParameterValues[0].ToString());
				if (e.ParameterValues[0] is LessEngine) {
					var engine = (LessEngine) e.ParameterValues[0];
					var reader = engine.Parser.Importer.FileReader;
					if (Debugger.IsAttached) 
						Debugger.Break();
				}
				base.MethodBehavior(e);
			}
		}
	}
}
