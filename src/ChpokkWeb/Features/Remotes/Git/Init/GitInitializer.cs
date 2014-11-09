using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Git.Init {
	public class GitInitializer {
		private readonly GitCommitter _gitCommitter;
		private readonly IFileSystem _fileSystem;
		public GitInitializer(GitCommitter gitCommitter, IFileSystem fileSystem) {
			_gitCommitter = gitCommitter;
			_fileSystem = fileSystem;
		}

		public void Init(string repositoryRoot) {
			var allFiles = _fileSystem.FindFiles(repositoryRoot, FileSet.Everything());
			Repository.Init(repositoryRoot);
			if (allFiles.Any()) {
				_gitCommitter.Commit(allFiles, "InitialCommit", repositoryRoot);	
			}
		}

		public bool GitRepositoryExistsIn(string repositoryRoot) {
			var path = FileSystem.Combine(repositoryRoot, ".git");
			if (Directory.Exists(path)) {
				//sometimes directory exists, but still it's not a valid repo, let's check
				try {
					new Repository(repositoryRoot).Dispose();
					return true; //if no exception, there's a repository indeed
				}
				catch (RepositoryNotFoundException) {
					return false;
				}
			}
			return false;			
		}
	}
}