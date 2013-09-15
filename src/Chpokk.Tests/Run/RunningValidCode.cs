using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningValidCode {
		[Test]
		public void CanCompile() {
			var code = "class program {static void Main(){}}";
			var path = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\program.cs";
			var exePath = Compile(path, code);
			Assert.IsTrue(File.Exists(exePath));
		}

		public static string Compile(string path, string code) {
			File.WriteAllText(path, code);
			var fileName = Path.GetFileName(path);
			var startInfo = new ProcessStartInfo(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\Csc.exe",
			                                     "/utf8output " + fileName)
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					WorkingDirectory = path.ParentDirectory(),
					StandardOutputEncoding = Encoding.UTF8,
					CreateNoWindow = true
				};
			using (var process = Process.Start(startInfo)) {
				process.WaitForExit();
				Console.WriteLine(process.StandardOutput.ReadToEnd());
				Console.WriteLine(process.StandardError.ReadToEnd());
			}
			var exeFileName = Path.GetFileNameWithoutExtension(fileName) + ".exe";
			var exePath = Path.Combine(path.ParentDirectory(), exeFileName);
			return exePath;
		}

		[Test]
		public void Test() {
			var path =
				@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\__anonymous__\Chpokk-SampleSol\src\ConsoleApplication1\bin\Debug\ConsoleApplication1.exe";
			var appDomain = AppDomain.CreateDomain("runner");
			var loader = (AssemblyLoader) appDomain.CreateInstanceFromAndUnwrap(typeof(AssemblyLoader).Assembly.CodeBase,
			                                                                               typeof (AssemblyLoader).FullName, null);
			Assert.DoesNotExist(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.FullName.Contains("ConsoleApplication1"));
			loader.Run(path);
			AppDomain.Unload(appDomain);
		}
	}
}
