using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha {
	public class Gimapster: IDisposable {
		private static SslStream _ssl;
		private readonly TcpClient _tcpc;

		public Gimapster(string username, string password) {
			_tcpc = new TcpClient("imap.gmail.com", 993);
			_ssl = new SslStream(_tcpc.GetStream());
			_ssl.AuthenticateAsClient("imap.gmail.com");
			ReceiveResponse("");

			ReceiveResponse("$ LOGIN " + username + " " + password + "  \r\n");
		}


		public IEnumerable<int> GetSearchResults() {
			var results = ReceiveResponse("$ SEARCH X-GM-RAW \"from:features\"\r\n");
			results = results.Split(new[] {"\r\n"}, StringSplitOptions.None)[0];
			results = results.Substring("* SEARCH ".Length);
			return from result in results.Split(' ') select Int32.Parse(result);
		}

		public void SelectChpokkFolder() {
			ReceiveResponse("$ SELECT chpokk\r\n");
		}

		public string GetHeader(int number) {
			return ReceiveResponse("$ FETCH " + number + " body[header]\r\n");
		}

		public string GetBody(int number) {
			return ReceiveResponse("$ FETCH " + number + " body[text]\r\n");
		}

		private string ReceiveResponse(string command) {
			var sb = new StringBuilder();
			try {
				if (command != "") {
					if (_tcpc.Connected) {
						var dummy = Encoding.ASCII.GetBytes(command);
						_ssl.Write(dummy, 0, dummy.Length);
					}
					else {
						throw new ApplicationException("TCP CONNECTION DISCONNECTED");
					}
				}
				_ssl.Flush();


				var buffer = new byte[2048];
				var bytes = _ssl.Read(buffer, 0, 2048); 
				sb.Append(Encoding.ASCII.GetString(buffer));
				return sb.ToString().Trim('\0');
			}
			catch (Exception ex) {
				throw new ApplicationException(ex.Message);
			}
		}

		public void Dispose() {
			ReceiveResponse("$ LOGOUT\r\n");
			if (_ssl != null) {
				_ssl.Close();
				_ssl.Dispose();
			}
			if (_tcpc != null) {
				_tcpc.Close();
			}
		}
	}
}
