using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using NuGet.Common;
using Console = System.Console;

namespace Chpokk.Tests.Infrastructure {
	public class NuGet2Console : IConsole {
		public FileConflictResolution ResolveFileConflict(string message) {
			return FileConflictResolution.Overwrite;
		}

		public void Log(MessageLevel level, string message, params object[] args) {
			Console.WriteLine(message, args);
		}
		public void Write(object value) {
			Console.Write(value);
		}
		public void Write(string value) {
			Console.Write(value);
		}
		public void Write(string format, params object[] args) {
			Console.Write(format, args);
		}
		public void WriteLine() {
			Console.WriteLine();
		}
		public void WriteLine(object value) {
			Console.WriteLine(value);
		}
		public void WriteLine(string value) {
			Console.WriteLine(value);
		}
		public void WriteLine(string format, params object[] args) {
			Console.WriteLine(format, args);
		}
		public void WriteLine(ConsoleColor color, string value, params object[] args) {
			Console.ForegroundColor = color;
			Console.WriteLine(value, args);
			Console.ResetColor();
		}
		public void WriteError(object value) {
			Console.Error.Write(value);
		}
		public void WriteError(string value) {
			Console.Error.Write(value);
		}
		public void WriteError(string format, params object[] args) {
			Console.Error.Write(format, args);
		}
		public void WriteWarning(string value) {
			WriteLine(ConsoleColor.Yellow, value);
		}
		public void WriteWarning(bool prependWarningText, string value) {
			WriteLine(ConsoleColor.Yellow, value);
		}
		public void WriteWarning(string value, params object[] args) {
			WriteLine(ConsoleColor.Yellow, value, args);
		}
		public void WriteWarning(bool prependWarningText, string value, params object[] args) {
			WriteLine(ConsoleColor.Yellow, value, args);
		}
		public bool Confirm(string description) {
			return true;
		}

		public ConsoleKeyInfo ReadKey() {
			return new ConsoleKeyInfo();
		}

		public string ReadLine() {
			return null;
		}

		public void ReadSecureString(SecureString secureString) {}
		public void PrintJustified(int startIndex, string text) {}
		public void PrintJustified(int startIndex, string text, int maxWidth) {}
		public int CursorLeft { get; set; }
		public int WindowWidth { get; set; }
		public Verbosity Verbosity { get; set; }
		public bool IsNonInteractive { get; set; }
	}
}
