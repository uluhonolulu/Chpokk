using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data;

namespace Gotcha {
	class Import {
		static void DoIt() {
			using (var gimapster = new Gimapster("uluhonolulu@gmail.com", "xd11SvG23")) {
				gimapster.SelectFolder("[Gmail]/&BBIEQQRP- &BD8EPgRHBEIEMA-");

				Console.WriteLine("searching");

				var results = gimapster.GetSearchResults("from:features");
				var db = Database.Open();
				foreach (var number in results) {
					Console.WriteLine("header");
					var header = gimapster.GetHeader(number);
					//Console.WriteLine(header);                                
					Console.WriteLine("body");
					var body = gimapster.GetBody(number);
					//Console.WriteLine(body);
					if (body.StartsWith("{")) {
						db.Usages.Insert(Header: header, Body: body, Id: number, DateOfUsage: GetDateFromHeader(header));
					}

				}
			}
			

		}
		static DateTime GetDateFromHeader(string header) {
			foreach (var line in header.Split(new[] { Environment.NewLine }, StringSplitOptions.None)) {
				if (line.StartsWith("Date: ")) {
					var dateString = line.Substring("Date: ".Length);
					return DateTime.Parse(dateString);
				}
			}
			return default(DateTime);
		}
	}
}
