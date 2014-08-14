using System;
using System.IO;
using Gallio.Runtime.ConsoleSupport;
using Microsoft.AspNet.SignalR;

namespace ChpokkWeb.Features.Testing {
	public class WebGallioConsole : IRichConsole {
		private readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<TestingHub>();

		public WebGallioConsole() {
			SyncRoot = new object();
			Width = 80;
			Out = Console.Out;
			Error = Console.Error;
		}
		public void ResetColor() { }
		public void SetFooter(Gallio.Common.Action showFooter, Gallio.Common.Action hideFooter) {}
		public void SetFooter(Action showFooter, Action hideFooter) { }
		public void Write(char c) { }
		public void Write(string str) { }
		public void WriteLine() { this.WriteLine(string.Empty); }
		public void WriteLine(string str) {
			Client.log(str, ForegroundColor.ToString());
		}

		public object SyncRoot { get; private set; }
		public bool IsCancelationEnabled { get; set; }
		public bool IsCanceled { get; set; }
		public bool IsRedirected { get; private set; }
		public TextWriter Error { get; private set; }
		public TextWriter Out { get; private set; }
		public ConsoleColor ForegroundColor { get; set; }
		public int CursorLeft { get; set; }
		public int CursorTop { get; set; }
		public string Title { get; set; }
		public int Width { get; private set; }
		public bool FooterVisible { get; set; }
		public event EventHandler Cancel;
		public string ConnectionId { get; set; }

		private dynamic Client {
			get { return _hubContext.Clients.Client(ConnectionId); }
		}
	}
}