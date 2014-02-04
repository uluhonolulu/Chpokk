using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Remotes.SVN {
	public class SvnDetectionPolicy: IVersionControlDetectionPolicy {
		private readonly RepositoryManager _repositoryManager;
		public SvnDetectionPolicy(RepositoryManager repositoryManager) {
			_repositoryManager = repositoryManager;
		}

		public bool Matches(string repositoryName) {
			var path = _repositoryManager.NewGetAbsolutePathFor(repositoryName, ".svn");
			return Directory.Exists(path);
		}
	}
}