using System;
using System.IO;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Running {
	public class ExeRunner {
		public object RunMain(string exePath, Action<char> standardOutput, Action<char> errorOutput) {
			var appDomain = AppDomain.CreateDomain("runner");
			try {
				var loader =
					(AssemblyLoader)
					appDomain.CreateInstanceFromAndUnwrap(typeof (AssemblyLoader).Assembly.CodeBase, typeof (AssemblyLoader).FullName,
					                                      null); //TODO: low trust
				loader.StandardOutput = new LambdaTextWriter(standardOutput);
				loader.ErrorOutput = new LambdaTextWriter(errorOutput);
				//loader.CurrentDirectory = exePath.ParentDirectory(); //commented out -- affects othe users
				return loader.Run(exePath);
			}
			finally {
				AppDomain.Unload(appDomain);
			}
		}
	}

	//public class ExeRunnerOutput {
	//	public object Result { get; set; }
	//	public string StandardOutput { get; set; }
	//	public string ErrorOutput { get; set; }
	//}
}