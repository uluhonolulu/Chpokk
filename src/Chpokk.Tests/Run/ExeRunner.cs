using System;

namespace Chpokk.Tests.Run {
	public class ExeRunner {
		public void RunMain(string exePath) {
			var appDomain = AppDomain.CreateDomain("runner");
			try {
				var loader =
					(AssemblyLoader)
					appDomain.CreateInstanceFromAndUnwrap(typeof (AssemblyLoader).Assembly.CodeBase, typeof (AssemblyLoader).FullName,
					                                      null);
				loader.Run(exePath);
			}
			finally {
				AppDomain.Unload(appDomain);
			}
		}
	}
}