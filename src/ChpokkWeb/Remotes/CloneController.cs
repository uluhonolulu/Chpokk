using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Repa;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using LibGit2Sharp;

namespace ChpokkWeb.Remotes {
	public class CloneController {

		[JsonEndpoint]
		public AjaxContinuation CloneRepo(CloneInputModel model) {
			CloneRepository(model);
			var continuation = AjaxContinuation.Successful();
			continuation["navigatePage"] = "/Main/repka";
			return continuation;
		}

		public static Repository CloneRepository(CloneInputModel input) {
			var repositoryPath = Path.Combine(input.PhysicalApplicationPath, RepositoryInfo.Path);
			var repository = Repository.Clone(input.RepoUrl, repositoryPath);
			var master = repository.Branches["master"];
			repository.Checkout(master);
			return repository;
		}
	}

	public class CloneInputModel {
		public string RepoUrl { get; set; }
		public string PhysicalApplicationPath { get; set; }
	}
}