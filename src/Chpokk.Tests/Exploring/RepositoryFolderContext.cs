﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using LibGit2Sharp.Tests.TestHelpers;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class RepositoryFolderContext : SimpleConfiguredContext, IDisposable {
		public readonly string REPO_NAME = "Perka";
		public string RepositoryRoot { get; private set; }

		public string RepoPath { get; private set; }

		public override void Create() {
			base.Create();
			var repositoryManager = Container.Get<RepositoryManager>();
			this.FakeSecurityContext.UserName = "ulu";
			RepoPath = repositoryManager.GetPathFor(REPO_NAME);
			var repositoryInfo = new RepositoryInfo(RepoPath, REPO_NAME);
			RepositoryRoot = Path.Combine(AppRoot, repositoryInfo.Path);
			if (Directory.Exists(RepositoryRoot.ParentDirectory())) {
				DirectoryHelper.DeleteSubdirectories(RepositoryRoot.ParentDirectory());	
			}
			if (!Directory.Exists(RepositoryRoot))
				Directory.CreateDirectory(RepositoryRoot);
		}

		public void Dispose() {
			DirectoryHelper.DeleteDirectory(RepositoryRoot);
		}
	}
}
