using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Authentication;
using System.Web;
using ChpokkWeb.Features.Remotes.SaveCommit;
using FubuMVC.Core.Http;
using SharpSvn;

namespace ChpokkWeb.Features.Remotes.SVN {
	public class SvnCommitter : SvnDetectionPolicy, ICommitter {
		readonly SvnClient _svnClient;
		private readonly IHttpWriter _httpWriter;
		private readonly CredentialsCache _credentialCache;
		public SvnCommitter(SvnClient svnClient, IHttpWriter httpWriter, CredentialsCache credentialCache)
			: base() {
			_svnClient = svnClient;
			_httpWriter = httpWriter;
			_credentialCache = credentialCache;
		}

		//https://subversion.assembla.com/svn/chpokk-samplesolution/
		public void Commit(string filePath, string commitMessage, string repositoryPath) {
			//_svnClient.Add(filePath, new SvnAddArgs(){Force = true});
			//_svnClient.Commit(filePath, new SvnCommitArgs(){LogMessage = commitMessage});

			Collection<SvnStatusEventArgs> changedFiles;
			_svnClient.GetStatus(repositoryPath, out changedFiles);

			//delete files from subversion that are not in filesystem
			//add files to subversion that are new in filesystem
			//modified files are automatically included as part of the commit

			UpdateFileStatus(changedFiles);

			DoCommit(commitMessage, repositoryPath);
		}

		private void DoCommit(string commitMessage, string repositoryPath) {
			var commitArgs = new SvnCommitArgs {LogMessage = commitMessage};
			//if (_credentialCache.ContainsKey(repositoryPath)) {
			//	_svnClient.Authentication.Clear(); // prevents checking cached credentials
			//	var credentials = _credentialCache[repositoryPath];
			//	_svnClient.Authentication.ForceCredentials(credentials.UserName, credentials.Password);
			//}

			try {
				_svnClient.Commit(repositoryPath, commitArgs);
			}
			catch (SvnAuthenticationException exception) {
				_httpWriter.WriteResponseCode(HttpStatusCode.Unauthorized, exception.Message);
				HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
				//throw new AuthenticationException(exception.RootCause.Message, exception);
				//exception.SvnErrorCode == SvnErrorCode.SVN_ERR_AUTHN_FAILED;
			} //SharpSvn.SvnAuthenticationException
		}

		private void UpdateFileStatus(IEnumerable<SvnStatusEventArgs> changedFiles) {
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
				if (changedFile.LocalContentStatus == SvnStatus.NotVersioned) _svnClient.Add(changedFile.Path);
			}
		}
	}
}