using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FubuCore;

namespace Gotcha {
	public class Gimapster: IDisposable {
		private readonly SslStream _ssl;
		private readonly TcpClient _tcpc;

		public Gimapster(string username, string password) {
			_tcpc = new TcpClient("imap.gmail.com", 993);
			_ssl = new SslStream(_tcpc.GetStream());
			_ssl.AuthenticateAsClient("imap.gmail.com");
			ReceiveResponse("");

			ReceiveResponse("$ LOGIN " + username + " " + password + "  \r\n");
		}


		public IEnumerable<int> GetSearchResults(string query) {
			var results = ReceiveResponse("$ SEARCH X-GM-RAW \"{0}\"\r\n".ToFormat(query));
			results = results.Split(new[] {"\r\n"}, StringSplitOptions.None)[0];
			results = results.Substring("* SEARCH ".Length);
			return from result in results.Split(' ') select Int32.Parse(result);
		}

		public void SelectFolder(string folderName) {
			var receiveResponse = ReceiveResponse("$ LIST " + "\"\"" + " \"*\"" + "\r\n");
			ReceiveResponse("$ SELECT \"{0}\"\r\n".ToFormat(folderName));
		}

		public string GetHeader(int number) {
			var header = ReceiveResponse("$ FETCH " + number + " body[header]\r\n");
			return ParseResponse(header);
			// response starts like * 2 FETCH (BODY[HEADER] {2488}\r\n
			// ends with \r\n\r\n)\r\n$ OK Success
		}

		public string GetBody(int number) {
			var body = ReceiveResponse("$ FETCH " + number + " body[text]\r\n");
			return ParseResponse(body);
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


				do {
					var buffer = new byte[2048];
					var bytes = _ssl.Read(buffer, 0, 2048);
					sb.Append(Encoding.ASCII.GetString(buffer, 0, bytes));
					if (sb.ToString().StartsWith("$ BAD")) {
						throw new Exception("IMAP responded with " + sb.ToString());
					}
					if (sb.ToString().Contains("\r\n$ OK") || sb.ToString().StartsWith("* OK")) {
						break;
					}
				} while (true);
				return sb.ToString();
			}
			catch (Exception ex) {
				throw new ApplicationException(ex.Message);
			}
		}

		private string ParseResponse(string output) {
			var regExp = new Regex(@"\* \d* FETCH .* \{(?<size>\d*)\}\r\n");
			var size = Int32.Parse(regExp.Matches(output)[0].Groups["size"].Value);
			Console.WriteLine(regExp.Matches(output)[0]);
			var startPos = regExp.Matches(output)[0].Length;
			Console.WriteLine(output.Substring(startPos + size));
			return output.Substring(startPos, size).Replace("=\r\n", "");
			
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
