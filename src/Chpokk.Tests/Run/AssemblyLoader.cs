using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chpokk.Tests.Run {
	public class AssemblyLoader: MarshalByRefObject {
		private TextWriter _standardOutput;

		public object Run(string exePath) {
			var assembly = Assembly.LoadFrom(exePath);
			var entryPoint = assembly.EntryPoint;
			var parameters = entryPoint.GetParameters();
			var arguments = from parameter in parameters select GetDefaultValue(parameter.ParameterType);
			return entryPoint.Invoke(null, arguments.ToArray());
		}

		object GetDefaultValue(Type t) {
			if (t.IsValueType) {
				return Activator.CreateInstance(t);
			}
			else {
				return null;
			}
		}


		public TextWriter StandardOutput {
			get { return _standardOutput; }
			set {
				_standardOutput = value;
				Console.SetOut(value);
			}
		}
	}
}