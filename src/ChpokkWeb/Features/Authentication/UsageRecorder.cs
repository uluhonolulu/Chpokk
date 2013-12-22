using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;

namespace ChpokkWeb.Features.Authentication {
	public class UsageRecorder {
		public void AddUsage(string userName, string data) {
			var db = Database.Open();
			db.Usages.Insert(UserId: userName, Body: data);
		}
	}
}