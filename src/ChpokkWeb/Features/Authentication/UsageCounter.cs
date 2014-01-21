using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;

namespace ChpokkWeb.Features.Authentication {
	public class UsageCounter {
		public int GetUsageCount(string userName) {
			var db = Database.Open();
			return db.Usages.GetCountByUserId(userName);
		}
	}
}