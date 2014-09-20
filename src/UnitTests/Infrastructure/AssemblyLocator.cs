using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Infrastructure {
	public static class AssemblyLocator {
		static Dictionary<string, Assembly> assemblies;

		public static void Init() {
			assemblies = new Dictionary<string, Assembly>();
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			Assembly assembly = null;
			assemblies.TryGetValue(args.Name, out assembly);
			if (assembly != null) {
				Console.WriteLine("Resolved " + assembly.FullName);			
			}
			else {
				Console.WriteLine("Couldn't resolve " + args.Name + " requested by " + args.RequestingAssembly.FullName);
			}

			return assembly;
		}

		static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args) {
			Assembly assembly = args.LoadedAssembly;
			Console.WriteLine("Loading " + assembly.FullName);
			assemblies[assembly.FullName] = assembly;
		}
	}
}
