using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class DotfuscatorProblem {
		[Test]
		public void Test() {
			var path = Path.GetFullPath(".");
			var assemblyFiles = Directory.GetFiles(path, "*.dll");
			foreach (var file in assemblyFiles) {
				try {
					var assembly = Assembly.LoadFile(file);
					var attributes = assembly.GetCustomAttributes(false);
					Console.WriteLine();
					Console.WriteLine(assembly.FullName);
					foreach (var attribute in attributes) {
						Console.WriteLine(attribute.ToString());
					}
				}
				catch (Exception exception) {

					Console.WriteLine();
					Console.WriteLine(file);
					Console.WriteLine(exception);
				}				
			}

		}
	}
}
