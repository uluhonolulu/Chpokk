using System;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.RepositoryManagement;
using LibGit2Sharp.Tests.TestHelpers;

namespace Chpokk.Tests.Uploading {
	public class UploadingZipFileContext: SimpleAuthenticatedContext, IDisposable {
		public const string ZIP_FILE_NAME = @"D:\Projects\Chpokk\src\Chpokk.Tests\Uploading\Fixture\repo.zip";
		public const string SOURCE_FILE_NAME = @"D:\Projects\Chpokk\src\Chpokk.Tests\Uploading\Fixture\Subfolder\sumfile.txt";
		public override void Dispose() {
			var repositoryManager = this.Container.Get<RepositoryManager>();
			var repoPath = repositoryManager.GetAbsolutePathFor("repo", AppRoot);
			DirectoryHelper.DeleteDirectory(repoPath);
			base.Dispose();
		}
	}
}