using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningValidCode : BaseCommandTest<CompiledExeContext> {
		[Test]
		public void CanRunTheMainMethod() {
		}

		[Test, DependsOn("CanRunTheMainMethod")]
		public void TheAssemblyIsNotLocked() {
			Assert.DoesNotExist(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.FullName.Contains(Path.GetFileNameWithoutExtension(Context.ExePath)));
		}

		public override void Act() {
			var exePath = Context.ExePath;
			Context.Container.Get<ExeRunner>().RunMain(exePath);
		}
	}

	public class CompiledExeContext: RepositoryFolderContext {

		public override void Create() {
			base.Create();
			var code = Code;
			var path = Path.Combine(this.RepositoryRoot, "program.cs");
			this.ExePath = Compile(path, code);
		}

		public string ExePath { get; private set; }

		protected virtual string Code {
			get { return "class program {static void Main(){}}"; }
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
	}
}
