using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Remotes.SaveCommit;
using ChpokkWeb.Features.RepositoryManagement;
using SharpSvn;

namespace ChpokkWeb.Features.Remotes.SVN {
	public class SvnCommitter : SvnDetectionPolicy, ICommitter {
		SvnClient _svnClient;
		public SvnCommitter(RepositoryManager repositoryManager, SvnClient svnClient) : base(repositoryManager) {
			_svnClient = svnClient;
		}

		public void Commit(string filePath, string commitMessage) {
			_svnClient.Add(filePath, new SvnAddArgs(){Force = true});
			_svnClient.Commit(filePath, new SvnCommitArgs(){LogMessage = commitMessage});
		}
	}
}