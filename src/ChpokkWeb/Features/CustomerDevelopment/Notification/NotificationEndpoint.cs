using System;
using ChpokkWeb.Features.Authentication;
using Simple.Data;

namespace ChpokkWeb.Features.CustomerDevelopment.Notification {
	public class NotificationEndpoint {
		private readonly UserManager _userManager;
		public NotificationEndpoint(UserManager userManager) {
			_userManager = userManager;
		}

		public void UpdateSubscriptionStatus(string userName) {
			var db = Database.Open();
			var user = _userManager.GetUser(userName);// db.Users.FindByUserId(userName);
			user.PaidUntil = DateTime.Today.AddMonths(1);
			user.Status = "base";
			db.Users.Update(user);
		}
	}
}