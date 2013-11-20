using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

		public static void InsertUsers() {
			var db = Database.Open();
			var allUsages = db.Usages.All();
			foreach (var usage in allUsages) {
				var data = JsonConvert.DeserializeObject<dynamic>(usage.Body);
				var profile = data.profile;
				var email = profile.email;
				var fullName = profile.displayName;
				var userId = profile.preferredUsername + "_" + profile.providerName;
				var photo = profile.photo;

				var thisUser = db.Users.FindAllByUserId(userId.ToString()).FirstOrDefault();
				if (thisUser == null) db.Users.Insert(Data: data, Email: email, FullName: fullName, UserId: userId, Photo: photo);

				usage.UserId = userId.ToString();
				db.Usages.Update(usage);
			}
		}
	}
}
