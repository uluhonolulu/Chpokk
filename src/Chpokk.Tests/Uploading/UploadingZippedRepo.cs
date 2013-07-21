using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Bottles;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using Gallio.Framework;
using ICSharpCode.SharpZipLib.Zip;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using LibGit2Sharp.Tests.TestHelpers;
using Shouldly;

namespace Chpokk.Tests.Uploading {
	[TestFixture]
	public class UploadingZippedRepo : BaseCommandTest<UploadingZipFileContext> {
		[Test]
		public void CreatesARepositoryWithNameSameAsZipFileName() {
			var repoName = Path.GetFileNameWithoutExtension(UploadingZipFileContext.ZIP_FILE_NAME);
			var repositoryManager = Context.Container.Get<RepositoryManager>();
			repositoryManager.GetRepositoryNames(Context.AppRoot).ShouldContain(repoName);
		}

		[Test, DependsOn("CreatesARepositoryWithNameSameAsZipFileName")]
		public void UnzipsTheFileIntoTheSubdirectoryItWasZippedFrom() {
			var relativeFilePath =
				UploadingZipFileContext.SOURCE_FILE_NAME.PathRelativeTo(UploadingZipFileContext.ZIP_FILE_NAME.ParentDirectory());
			var fileName = FileSystem.Combine(RepositoryPath, relativeFilePath);
			File.Exists(fileName).ShouldBeTrue();
		}

		private string RepositoryPath {
			get {
				var repositoryManager = Context.Container.Get<RepositoryManager>();
				return repositoryManager.GetAbsolutePathFor("repo", Context.AppRoot);
			}
		}

		public override void Act() {
			var fileSystem = Context.Container.Get<IFileSystem>();
			using (Stream fileStream = File.OpenRead(UploadingZipFileContext.ZIP_FILE_NAME)) {
				using (var zipFile = new ZipFile(fileStream) {IsStreamOwner = true}) {
					foreach (ZipEntry zipEntry in zipFile) {
						if (!zipEntry.IsDirectory) {
							var fileName = RepositoryPath.AppendPath(zipEntry.Name);
							fileSystem.WriteStreamToFile(fileName, zipFile.GetInputStream(zipEntry));
						}
					}
				}
			}
		}
	}

	public class UploadingZipFileContext: SimpleAuthenticatedContext, IDisposable {
		public const string ZIP_FILE_NAME = @"D:\Projects\Chpokk\src\Chpokk.Tests\Uploading\Fixture\repo.zip";
		public const string SOURCE_FILE_NAME = @"D:\Projects\Chpokk\src\Chpokk.Tests\Uploading\Fixture\Subfolder\sumfile.txt";
		public void Dispose() {
			var repositoryManager = this.Container.Get<RepositoryManager>();
			var repoPath = repositoryManager.GetAbsolutePathFor("repo", AppRoot);
			DirectoryHelper.DeleteDirectory(repoPath);
		}
	}
}
