using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data;

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


				gimapster.SelectFolder("[Gmail]/&BBIEQQRP- &BD8EPgRHBEIEMA-");


				Console.WriteLine("searching");



				//Console.WriteLine("enter the email number to fetch :"); 
				var results = gimapster.GetSearchResults("from:features");

				var db = Database.Open();
				foreach (var number in results) {
					Console.WriteLine("header");
					var header = gimapster.GetHeader(number);
					Console.WriteLine(header);                                
					Console.WriteLine("body");
					var body = gimapster.GetBody(number);
					Console.WriteLine(body);
					if (body.StartsWith("{")) {
						db.Usages.Insert(Header: header, Body: body, Id: number, DateOfUsage: GetDateFromHeader(header));
					}

				}



			}
			catch (Exception ex) {
				Console.WriteLine("error: " + ex.Message);
			}


			//Console.ReadKey(); 
		}

		static DateTime GetDateFromHeader(string header) {
			foreach (var line in header.Split(new[]{Environment.NewLine}, StringSplitOptions.None)) {
				if (line.StartsWith("Date: ")) {
					var dateString = line.Substring("Date: ".Length);
					return DateTime.Parse(dateString);
				}
			}
			return default(DateTime);
		}
	} 
}
