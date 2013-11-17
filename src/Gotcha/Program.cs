using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha {
	class Program {
		static System.IO.StreamWriter sw = null;
		static System.Net.Sockets.TcpClient tcpc = null;
		static System.Net.Security.SslStream ssl = null;
		static string username, password;
		static string path;
		static int bytes = -1;
		static byte[] buffer;
		static byte[] dummy;
		static void Main(string[] args) {
			try {
				path = Environment.CurrentDirectory + "\\emailresponse.txt";

				if (System.IO.File.Exists(path))
					System.IO.File.Delete(path);

				sw = new System.IO.StreamWriter(System.IO.File.Create(path));
				// there should be no gap between the imap command and the \r\n       
				// ssl.read() -- while ssl.readbyte!= eof does not work because there is no eof from server 
				// cannot check for \r\n because in case of larger response from server ex:read email message 
				// there are lot of lines so \r \n appears at the end of each line 
				//ssl.timeout sets the underlying tcp connections timeout if the read or write 
				//time out exceeds then the undelying connection is closed 
				tcpc = new System.Net.Sockets.TcpClient("imap.gmail.com", 993);

				ssl = new System.Net.Security.SslStream(tcpc.GetStream());
				ssl.AuthenticateAsClient("imap.gmail.com");
				ReceiveResponse("");

				//Console.WriteLine("username : "); 
				username = "uluhonolulu@gmail.com";

				//Console.WriteLine("password : "); 
				password = "xd11SvG23";
				ReceiveResponse("$ LOGIN " + username + " " + password + "  \r\n");
				//Console.Clear();                

				//receiveResponse("$ LIST " + "\"\"" + " \"*\"" + "\r\n");  
				Console.WriteLine("selecting");
				Console.WriteLine(ReceiveResponse("$ SELECT chpokk\r\n"));
				;

				//receiveResponse("$ FETCH 1 (X-GM-MSGID)\r\n"); 

				Console.WriteLine("searching");



				//Console.WriteLine("enter the email number to fetch :"); 
				var results = GetSearchResults();
				foreach (var number in results) {
					Console.WriteLine("header");
					Console.WriteLine(ReceiveResponse("$ FETCH " + number + " body[header]\r\n"));                                
					Console.WriteLine("body");
					Console.WriteLine(ReceiveResponse("$ FETCH " + number + " body[text]\r\n"));					
				}



				ReceiveResponse("$ LOGOUT\r\n");
			}
			catch (Exception ex) {
				Console.WriteLine("error: " + ex.Message);
			}
			finally {
				if (sw != null) {
					sw.Close();
					sw.Dispose();
				}
				if (ssl != null) {
					ssl.Close();
					ssl.Dispose();
				}
				if (tcpc != null) {
					tcpc.Close();
				}
			}


			//Console.ReadKey(); 
		}

		static IEnumerable<int> GetSearchResults() {
			var results = ReceiveResponse("$ SEARCH X-GM-RAW \"from:features\"\r\n");
			results = results.Split(new string[] {"\r\n"}, StringSplitOptions.None)[0];
			results = results.Substring("* SEARCH ".Length);
			return from result in results.Split(' ') select int.Parse(result);

		}
		static string ReceiveResponse(string command) {
			StringBuilder sb = new StringBuilder();
			try {
				if (command != "") {
					if (tcpc.Connected) {
						dummy = Encoding.ASCII.GetBytes(command);
						ssl.Write(dummy, 0, dummy.Length);
					}
					else {
						throw new ApplicationException("TCP CONNECTION DISCONNECTED");
					}
				}
				ssl.Flush();


				buffer = new byte[2048];
				bytes = ssl.Read(buffer, 0, 2048);
				sb.Append(Encoding.ASCII.GetString(buffer));


				Console.WriteLine(sb.ToString());
				return sb.ToString().Trim('\0');
			}
			catch (Exception ex) {
				throw new ApplicationException(ex.Message);
			}
		}

	} 
}
