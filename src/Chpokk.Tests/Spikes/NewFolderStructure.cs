using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Storage;
using Emkay.S3;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class NewFolderStructure: BaseCommandTest<SimpleConfiguredContext> {
		[Test]
		public void FileExists() {
			var client = Context.Container.Get<IS3Client>();
			var remoteFileSystem = Context.Container.Get<RemoteFileSystem>();
			var localPath = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\art_Foursquare\ubzik\ubzik.sln";
			File.Exists(localPath).ShouldBe(true);
			remoteFileSystem.FileExists(localPath).ShouldBe(true);
			//client.EnumerateChildren("chpokk", "UserFiles/__anonymous__/Kudna/Kudna.sln").Any().ShouldBe(true);
		}

		[Test]
		public void DeletingAFile() {
			var remoteFileSystem = Context.Container.Get<RemoteFileSystem>();
			var localPath = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\art_Foursquare\ubzik\ubzik.sln";
			remoteFileSystem.DeleteFile(localPath);
			remoteFileSystem.FileExists(localPath).ShouldBe(false);
			var trashPath = localPath.Replace("UserFiles", @"Trash\UserFiles");
			remoteFileSystem.FileExists(trashPath).ShouldBe(true);
		}

		[Test]
		public void MovingFoldersToRepositories() {
			var localFileSystem = Context.Container.Get<FileSystem>();
			var remoteFileSystem = Context.Container.Get<RemoteFileSystem>();
			var manager = Context.Container.Get<RepositoryManager>();
			//manager.GetUserFolder()
			//Console.WriteLine(manager.NewGetAbsolutePathFor("ubzik"));
			manager.MoveFilesToRepositoryFolder();

			var root = manager.GetUserFolder();
			localFileSystem.FileExists(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\__anonymous__\Kudna\Kudna.sln").ShouldBe(false);
			localFileSystem.FileExists(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\__anonymous__\Repositories\Kudna\Kudna.sln").ShouldBe(true);
			remoteFileSystem.FileExists("UserFiles/__anonymous__/Repositories/Kudna/Kudna.sln").ShouldBe(true);
			remoteFileSystem.FileExists("UserFiles/__anonymous__/Kudna/Kudna.sln").ShouldBe(false);
		}

		public override void Act() {
			var uploader = Context.Container.Get<Uploader>();
			uploader.UploadFolder(Context.AppRoot.AppendPath(@"UserFiles\__anonymous__"), Context.AppRoot);
		}
	}
}
