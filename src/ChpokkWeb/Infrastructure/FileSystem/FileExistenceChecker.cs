using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Storage;
using FubuCore;

namespace ChpokkWeb.Infrastructure.FileSystem {
	public class FileExistenceChecker {
		private readonly FubuCore.FileSystem _localSystem;
		private readonly RemoteFileSystem _remoteSystem;
		private readonly SmtpClient _mailer;
		private readonly RepositoryManager _repositoryManager;
		public FileExistenceChecker(FubuCore.FileSystem localSystem, RemoteFileSystem remoteSystem, RepositoryManager repositoryManager, SmtpClient mailer) {
			_localSystem = localSystem;
			_remoteSystem = remoteSystem;
			_repositoryManager = repositoryManager;
			_mailer = mailer;
		}

		public void VerifyFileExists(string path) {
			if (!_localSystem.FileExists(path)) {
				var repositoryFolder = _repositoryManager.GetRepositoryFolder();
				var allUserFiles = _localSystem.FindFiles(repositoryFolder, FileSet.Everything());
				var message = "Can't find the file: " + path + Environment.NewLine + 
								"Remote exists: " + _remoteSystem.FileExists(path) + Environment.NewLine + 
								"User files:" + Environment.NewLine +
				              allUserFiles.Join(Environment.NewLine);
				_mailer.Send("notfound@chpokk.apphb.com", "uluhonolulu@gmail.com", "Can't find", message);
			}
		}
	}
}