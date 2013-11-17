using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha {
	class Program {
		static void Main(string[] args) {
			try {

				// there should be no gap between the imap command and the \r\n       
				// ssl.read() -- while ssl.readbyte!= eof does not work because there is no eof from server 
				// cannot check for \r\n because in case of larger response from server ex:read email message 
				// there are lot of lines so \r \n appears at the end of each line 
				//ssl.timeout sets the underlying tcp connections timeout if the read or write 
				//time out exceeds then the undelying connection is closed 


				var gimapster = new Gimapster("uluhonolulu@gmail.com", "xd11SvG23");

				//receiveResponse("$ LIST " + "\"\"" + " \"*\"" + "\r\n");  
				gimapster.SelectFolder("chpokk");

				//receiveResponse("$ FETCH 1 (X-GM-MSGID)\r\n"); 

				Console.WriteLine("searching");



				//Console.WriteLine("enter the email number to fetch :"); 
				var results = gimapster.GetSearchResults("from:features");
				foreach (var number in results) {
					Console.WriteLine("header");
					var header = gimapster.GetHeader(number);
					Console.WriteLine(header);                                
					Console.WriteLine("body");
					var body = gimapster.GetBody(number);
					Console.WriteLine(body);					
				}



			}
			catch (Exception ex) {
				Console.WriteLine("error: " + ex.Message);
			}


			//Console.ReadKey(); 
		}
	} 
}
