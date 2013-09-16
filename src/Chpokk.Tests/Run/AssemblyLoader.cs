using System;
using System.Reflection;

namespace Chpokk.Tests.Run {
	public class AssemblyLoader: MarshalByRefObject {
		public void Run(string exePath) {
			var assembly = Assembly.LoadFrom(exePath);
			assembly.EntryPoint.Invoke(null, new object[]{});
		}
	}
}