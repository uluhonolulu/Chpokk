using System;
using ChpokkWeb.Features.Authentication;
using Simple.Data;

namespace ChpokkWeb.Features.CustomerDevelopment.Notification {
	public class NotificationEndpoint {
		private readonly UserManager _userManager;
		public NotificationEndpoint(UserManager userManager) {
			_userManager = userManager;
		}

		public void UpdateSubscriptionStatus(NotificationInputModel model) {
			var user = _userManager.GetUser(model.SubscriptionReferrer);
			user.PaidUntil = DateTime.Today.AddMonths(1);
			user.Status = "base";
			_userManager.UpdateUser(user);
		}
	}
}