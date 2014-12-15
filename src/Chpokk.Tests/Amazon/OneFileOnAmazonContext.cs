using System;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Storage;
using ChpokkWeb.Infrastructure.FileSystem;
using Emkay.S3;
using FubuCore;

namespace Chpokk.Tests.Amazon {
	public class OneFileOnAmazonContext : RepositoryFolderContext {
		public readonly string RELATIVE_PATH = "folder/file";
		public string FileFullPath { get; set; }
		public override void Create() {
			base.Create();
			FileFullPath = FileSystem.Combine(base.RepositoryRoot, RELATIVE_PATH.Replace('/', '\\'));

			//upload it to Amazon
			var client = Container.Get<IS3Client>();
			var sourcePath = FileSystem.Combine(AppRoot, "UserFiles", "dummy.txt");
			client.PutFile("chpokk", FilePathRelativeToAppRoot, sourcePath, true, Int32.MaxValue);
		}
		public string FilePathRelativeToAppRoot {
			get {
				var manager = Container.Get<RepositoryManager>();
				var localPath = manager.NewGetAbsolutePathFor(REPO_NAME, RELATIVE_PATH);
				return localPath.ToRemoteFileName(AppRoot);
			}
		}

		public override void Dispose() {
			var client = Container.Get<IS3Client>();
			var remoteFiles = client.EnumerateChildren("chpokk", RepositoryRoot.ToRemoteFileName(AppRoot));
			foreach (string remoteFile in remoteFiles) {
				client.DeleteObject("chpokk", remoteFile);
			}
			base.Dispose();
		}
	}
}