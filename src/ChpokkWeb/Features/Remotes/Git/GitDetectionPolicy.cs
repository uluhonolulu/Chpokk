using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Remotes.Git {
	public class GitDetectionPolicy : IVersionControlDetectionPolicy {
		private readonly RepositoryManager _repositoryManager;
		public GitDetectionPolicy(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public bool Matches(string repositoryName) {
			var path = _repositoryManager.NewGetAbsolutePathFor(repositoryName, ".git");
			return Directory.Exists(path);
		}
	}
}