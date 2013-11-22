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
			var chimper = new Chimper();
			var db = Database.Open();
			var users = db.Users.All();
			foreach (var user in users) {
				if (user.Email != null) {
					chimper.SubscribeUser(user.Email, user.FullName);
				}
				
			}
		}
	} 
}
