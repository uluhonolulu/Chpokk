using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;

namespace ChpokkWeb.Features.CustomerDevelopment.TimeToPay {
	public class EndOfTrialTimeToPayBehavior : IActionBehavior {
		private readonly IActionBehavior _innerBehavior;
		private readonly IHttpWriter _httpWriter;
		private readonly UserManagerInContext _userManager;
		private readonly SmtpClient _smtpClient;

		public EndOfTrialTimeToPayBehavior(IActionBehavior innerBehavior, IHttpWriter httpWriter, UserManagerInContext userManager, SmtpClient smtpClient) {
			_innerBehavior = innerBehavior;
			_httpWriter = httpWriter;
			_userManager = userManager;
			_smtpClient = smtpClient;
		}

		public void Invoke() {
			var currentUser = _userManager.GetCurrentUser();
			if (ShouldRedirect(currentUser)) {
				_smtpClient.Send("endoftrial@chpokk.apphb.com", "uluhonolulu@gmail.com", "End of trial for " + currentUser.UserId, "hurray!");
				_httpWriter.Redirect("http://sites.fastspring.com/geeksoft/product/chpokkstarter");			
			}
			else {
				_innerBehavior.Invoke();
			}

		}



		public void InvokePartial() {
			_innerBehavior.InvokePartial();
		}

		public bool ShouldRedirect(dynamic user) {
			if (user == null || user.Status == null) return false;
			if (user.Status.ToString() == "canceled") return true;
			if (user.PaidUntil == null || user.PaidUntil > DateTime.Now ) return false;
			return true;
		}
	}
}