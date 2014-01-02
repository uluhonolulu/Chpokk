using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Web;
using NuGet;
using NuGet.Common;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet.Spike {
	public abstract class ConsoleBase : IConsole {
		public abstract int WindowWidth { get; set; }

		public Verbosity Verbosity {
			get;
			set;
		}

		public bool IsNonInteractive {
			get;
			set;
		}

		public void Write(object value) {
			Out.Write(value);
		}

		public void Write(string value) {
			Out.Write(value);
		}

		public void Write(string format, params object[] args) {
			if (args == null || !args.Any()) {
				// Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
				// and contained braces (for instance html)
				Out.Write(format);
			}
			else {
				Out.Write(format, args);
			}
		}

		public void WriteLine() {
			Out.WriteLine();
		}

		public void WriteLine(object value) {
			Out.WriteLine(value);
		}

		public void WriteLine(string value) {
			Out.WriteLine(value);
		}

		public void WriteLine(string format, params object[] args) {
			Out.WriteLine(format, args);
		}

		public void WriteError(object value) {
			WriteError(value.ToString());
		}

		public void WriteError(string value) {
			WriteError(value, new object[0]);
		}

		public void WriteError(string format, params object[] args) {
			WriteColor(Out, ConsoleColor.Red, format, args);
		}

		protected abstract void WriteColor(System.IO.TextWriter textWriter, ConsoleColor consoleColor, string format, object[] args);

		public void WriteWarning(string value) {
			WriteWarning(prependWarningText: true, value: value, args: new object[0]);
		}

		public void WriteWarning(bool prependWarningText, string value) {
			WriteWarning(prependWarningText, value, new object[0]);
		}

		public void WriteWarning(string value, params object[] args) {
			WriteWarning(prependWarningText: true, value: value, args: args);
		}

		public void WriteWarning(bool prependWarningText, string value, params object[] args) {
			string message = prependWarningText
								 ? String.Format(CultureInfo.CurrentCulture, "Warning: {0}", value)
								 : value;

			WriteColor(Out, ConsoleColor.Yellow, message, args);
		}

		public abstract bool Confirm(string description);
		public abstract ConsoleKeyInfo ReadKey();
		public abstract string ReadLine();
		public abstract void ReadSecureString(SecureString secureString);
		public abstract void PrintJustified(int startIndex, string text);
		public abstract void PrintJustified(int startIndex, string text, int maxWidth);
		public abstract int CursorLeft { get; set; }

		public void WriteLine(ConsoleColor color, string value, params object[] args) {
			WriteColor(Out, color, value, args);
		}


		public abstract System.IO.TextWriter Out { get; set; }

		public void Log(MessageLevel level, string message, params object[] args) {
			switch (level) {
				case MessageLevel.Info:
					WriteLine(message, args);
					break;
				case MessageLevel.Warning:
					WriteWarning(message, args);
					break;
				case MessageLevel.Debug:
					WriteColor(Out, ConsoleColor.Gray, message, args);
					break;
			}
		}

		public abstract FileConflictResolution ResolveFileConflict(string message);
	}
}