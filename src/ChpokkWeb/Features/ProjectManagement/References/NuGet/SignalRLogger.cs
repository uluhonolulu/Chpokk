using System;
using System.IO;
using System.Security;
using System.Text;
using Microsoft.AspNet.SignalR;
using NuGet;
using FubuCore;

namespace ChpokkWeb.Features.ProjectManagement.References.NuGet {
	public class SignalRLogger : ConsoleBase {
		private readonly IHubContext _hubContext;
		private Guid id = Guid.NewGuid();
		public SignalRLogger() {
			_hubContext = GlobalHost.ConnectionManager.GetHubContext<NuGetHub>();
			Out = this;
		}

		public string ConnectionId { get; set; }

		public override void Write(string value) {
			Client.Write(value);
		}

		public override void WriteLine(string value) {
			Client.WriteLine(value);
		}

		protected override void WriteColor(TextWriter textWriter, ConsoleColor consoleColor, string format, object[] args) {
			var value = format.ToFormat(args);
			Client.WriteLine(value + ": " + consoleColor, consoleColor); 
		}
		private dynamic Client {
			get {
				EnsureConnectionId();
				return _hubContext.Clients.Client(ConnectionId);
			}
		}
		private void EnsureConnectionId() {
			if (ConnectionId.IsEmpty()) throw new InvalidOperationException("Can't write to an unknown connection");
		}
		public override Encoding Encoding {
			get { return Encoding.UTF8; }
		}
		public override int WindowWidth { get; set; }
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