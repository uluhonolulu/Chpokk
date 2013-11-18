using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simple.Data;

namespace Gotcha {
	class Program {
		static void Main(string[] args) {
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
				if (thisUser == null) {
					db.Users.Insert(Data: data, Email: email, FullName: fullName, UserId: userId, Photo: photo);
				}

				usage.UserId = userId.ToString();
				db.Usages.Update(usage);
			}	


		}


	} 
}
