using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture, RunOnWeb, Ignore("Typemock")]
	public class ICanSeeIt {
		[Test]
		public void DoesntThrow() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("CompilerError"), 5));
			//CThruEngine.AddAspect(new TraceAspect(info => info.MethodName.EndsWith("ReferencedAssemblies"), 5));
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
			CThruEngine.AddAspect(new LetsSeeMyAssemblies(info => info.MethodName.EndsWith("CompileAssemblyFromSource")));
			CThruEngine.AddAspect(new TraceAspect(info => info.TypeName == "Microsoft.CSharp.CSharpCodeGenerator" && info.MethodName == "Compile", 5));
			Assert.DoesNotThrow(() => new TestSession().Get("/Main"));  
		}

		private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args) {
			if (args.Name == "Configuration") {
				var asses = AppDomain.CurrentDomain.GetAssemblies();
				var confies = asses.Where(ass => ass.GetName().Name == "Configuration").ToArray();
				Console.WriteLine(confies.Count());
			}
			return null;
		}

		void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args) {
			if (args.LoadedAssembly.GetName().Name == "Configuration") {
				var asses = AppDomain.CurrentDomain.GetAssemblies();
				var confies = asses.Where(ass => ass.GetName().Name == "Configuration").ToArray();
				Console.WriteLine(confies.Count());
				//AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
			}
		}
	}

	class LetsSeeMyAssemblies : CommonAspect {
		public LetsSeeMyAssemblies(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}

		public override void MethodBehavior(DuringCallbackEventArgs e) {
			var asses = AppDomain.CurrentDomain.GetAssemblies();
			var confies = asses.Where(ass => ass.GetName().Name == "Configuration").ToArray();
			base.MethodBehavior(e);
		}
	}
}
