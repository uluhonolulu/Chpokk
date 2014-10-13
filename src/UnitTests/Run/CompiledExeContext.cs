using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Chpokk.Tests.Exploring;
using FubuCore;

namespace Chpokk.Tests.Run {
	public abstract class CompiledExeContext: RepositoryFolderContext {

		public override void Create() {
			base.Create();
			var code = Code;
			var path = Path.Combine(this.RepositoryRoot, "program.cs");
			this.ExePath = Compile(path, code);
		}

		public string ExePath { get; private set; }

		public abstract string Code { get; }

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