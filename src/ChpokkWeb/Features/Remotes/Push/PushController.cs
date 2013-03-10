using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core.Ajax;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Push {
	public class PushController {
		private RepositoryManager _manager;
		public PushController(RepositoryManager manager) {
			_manager = manager;
		}

		public AjaxContinuation Push(PushInputModel model) {
			var credentials = model.Username.IsEmpty()? null: new Credentials {Username = model.Username, Password = model.Password};
			var path = FileSystem.Combine(model.PhysicalApplicationPath, _manager.GetPathFor(model.RepositoryName)) ;
			var success = true;
			var errorMessage = string.Empty;
			var ajaxContinuation = AjaxContinuation.Successful();
			using (var repo = new Repository(path)) {
				var remote = repo.Remotes["origin"];
				repo.Network.Push(remote, "refs/heads/master", error => {
					ajaxContinuation.Success = false;
					errorMessage = error.Reference + ": " + error.Message + "/r";
					ajaxContinuation.Errors.Add(new AjaxError { message = errorMessage });
				}, credentials);
			}
			return ajaxContinuation;
		}
	}
}