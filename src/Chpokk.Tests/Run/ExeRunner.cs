using System;
using System.IO;

namespace Chpokk.Tests.Run {
	public class ExeRunner {
		public ExeRunnerOutput RunMain(string exePath) {
			var standardOutput = new StringWriter();
			var errorOutput = new StringWriter();
			var appDomain = AppDomain.CreateDomain("runner");
			object result;
			try {
				var loader =
					(AssemblyLoader)
					appDomain.CreateInstanceFromAndUnwrap(typeof (AssemblyLoader).Assembly.CodeBase, typeof (AssemblyLoader).FullName,
					                                      null);
				loader.StandardOutput = standardOutput;
				loader.ErrorOutput = errorOutput;
				result = loader.Run(exePath);
			}
			finally {
				AppDomain.Unload(appDomain);
			}
			return new ExeRunnerOutput{Result = result, StandardOutput = standardOutput.ToString(), ErrorOutput = errorOutput.ToString()};
		}
	}

	public class ExeRunnerOutput {
		public object Result { get; set; }
		public string StandardOutput { get; set; }
		public string ErrorOutput { get; set; }
	}
}