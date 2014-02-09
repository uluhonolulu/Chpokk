using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web;
using ChpokkWeb.Features.Remotes.SaveCommit;
using ChpokkWeb.Features.RepositoryManagement;
using FubuMVC.Core.Http;
using SharpSvn;

namespace ChpokkWeb.Features.Remotes.SVN {
	public class SvnCommitter : SvnDetectionPolicy, ICommitter {
		readonly SvnClient _svnClient;
		private readonly RepositoryManager _manager;
		private readonly IHttpWriter _httpWriter;
		public SvnCommitter(RepositoryManager repositoryManager, SvnClient svnClient, RepositoryManager manager, IHttpWriter httpWriter)
			: base() {
			_svnClient = svnClient;
			_manager = manager;
			_httpWriter = httpWriter;
		}

		//https://subversion.assembla.com/svn/chpokk-samplesolution/
		public void Commit(string filePath, string commitMessage, string repositoryName) {
			//_svnClient.Add(filePath, new SvnAddArgs(){Force = true});
			//_svnClient.Commit(filePath, new SvnCommitArgs(){LogMessage = commitMessage});

			var repositoryPath = _manager.NewGetAbsolutePathFor(repositoryName);
			Collection<SvnStatusEventArgs> changedFiles;
			_svnClient.GetStatus(repositoryPath, out changedFiles);

			//delete files from subversion that are not in filesystem
			//add files to subversion that are new in filesystem
			//modified files are automatically included as part of the commit

			foreach (SvnStatusEventArgs changedFile in changedFiles) {
				if (changedFile.LocalContentStatus == SvnStatus.Missing) {
					// SVN thinks file is missing but it still exists hence
					// a change in the case of the filename.
					if (System.IO.File.Exists(changedFile.Path)) {
						var deleteArgs = new SvnDeleteArgs {KeepLocal = true};
						_svnClient.Delete(changedFile.Path, deleteArgs);
					}
					else
						_svnClient.Delete(changedFile.Path);
				}
				if (changedFile.LocalContentStatus == SvnStatus.NotVersioned) {
					_svnClient.Add(changedFile.Path);
				}
			}

			var commitArgs = new SvnCommitArgs {LogMessage = commitMessage};

			try {
				_svnClient.Commit(repositoryPath, commitArgs);
			}
			catch (SvnAuthenticationException exception) {
				_httpWriter.WriteResponseCode(HttpStatusCode.Unauthorized, exception.Message);
			}//SharpSvn.SvnAuthenticationException
		}
	}
}