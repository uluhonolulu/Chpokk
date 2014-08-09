using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ChpokkWeb.Features.Authentication;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuCore;

namespace ChpokkWeb.Features.CustomerDevelopment.TimeToPay {
	public class EndOfTrialTimeToPayBehavior : IActionBehavior {
		private readonly IActionBehavior _innerBehavior;
		private readonly IHttpWriter _httpWriter;
		private readonly UserManagerInContext _userManager;
		private readonly SmtpClient _smtpClient;
		private readonly ICurrentHttpRequest _request;

		public EndOfTrialTimeToPayBehavior(IActionBehavior innerBehavior, IHttpWriter httpWriter, UserManagerInContext userManager, SmtpClient smtpClient, ICurrentHttpRequest request) {
			_innerBehavior = innerBehavior;
			_httpWriter = httpWriter;
			_userManager = userManager;
			_smtpClient = smtpClient;
			_request = request;
		}

		public void Invoke() {
			var currentUser = _userManager.GetCurrentUser();
			if (ShouldRedirect(currentUser)) {
				_smtpClient.Send("endoftrial@chpokk.apphb.com", "uluhonolulu@gmail.com", "End of trial for " + currentUser.UserId, "hurray! " + _request.FullUrl() + @", her email: {0} <{1}>".ToFormat(
					((string) currentUser.FullName), ((string) currentUser.Email)));
				_httpWriter.Redirect("http://sites.fastspring.com/geeksoft/product/chpokkstarter?referrer=" + currentUser.UserId);			
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