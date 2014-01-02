﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using NuGet;
using NuGet.Common;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet.Spike {
	public class SignalRLogger : ConsoleBase {
		public override int WindowWidth { get; set; }
		protected override void WriteColor(TextWriter textWriter, ConsoleColor consoleColor, string format, object[] args) {}
		public override bool Confirm(string description) {
			throw new NotImplementedException();
		}

		public override ConsoleKeyInfo ReadKey() {
			throw new NotImplementedException();
		}

		public override string ReadLine() {
			throw new NotImplementedException();
		}

		public override void ReadSecureString(SecureString secureString) { throw new NotImplementedException(); }
		public override void PrintJustified(int startIndex, string text) { throw new NotImplementedException(); }
		public override void PrintJustified(int startIndex, string text, int maxWidth) { throw new NotImplementedException(); }
		public override int CursorLeft { get; set; }
		public override TextWriter Out { get; set; }
		public override FileConflictResolution ResolveFileConflict(string message) {
			return FileConflictResolution.Overwrite;
		}
	}
}