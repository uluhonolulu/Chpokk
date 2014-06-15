using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChpokkWeb.Features.Running {
	public class AssemblyLoader: MarshalByRefObject {
		private TextWriter _standardOutput;
		private TextWriter _errorOutput;

		public object Run(string exePath) {
			var assembly = Assembly.LoadFrom(exePath);
			var entryPoint = assembly.EntryPoint;
			if (entryPoint == null) {
				throw new InvalidOperationException("Can't run a non-exe file.");
			}
			var parameters = entryPoint.GetParameters();
			var arguments = from parameter in parameters select GetDefaultValue(parameter.ParameterType);
			try {
				return entryPoint.Invoke(null, arguments.ToArray());
			}
			catch (TargetInvocationException exception) {
				Console.Error.WriteLine(exception.InnerException);
				return null;
			}
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

		public TextWriter ErrorOutput {
			get { return _errorOutput; }
			set {
				_errorOutput = value;
				Console.SetError(value);
			}
		}

		public string CurrentDirectory {
			get { return Directory.GetCurrentDirectory(); }
			set { Directory.SetCurrentDirectory(value); }
		}
	}
}