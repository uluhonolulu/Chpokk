using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.Push {
	public class PushMenuItem: MenuItem {
		public PushMenuItem() {
			Id = "pusher";
			Caption = "Push";
		}
	}
}