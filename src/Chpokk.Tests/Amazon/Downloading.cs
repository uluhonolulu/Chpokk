using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Storage;
using Emkay.S3;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class Downloading: BaseCommandTest<OneFileOnAmazonContext> {


		[Test]
		public void FileFromAmazonShouldAppearInUserFiles() {
			var repositoryFiles = Directory.GetFiles(Context.RepositoryRoot, "*.*", SearchOption.AllDirectories);
			Assert.Contains(repositoryFiles, Context.FileFullPath); 
		}

		public override void Act() {
			var downloader = Context.Container.Get<Downloader>();
			var subFolder = Context.RepositoryRoot.PathRelativeTo(Context.AppRoot);
			Console.WriteLine("Looking in " + subFolder);
			downloader.DownloadAllFiles(Context.AppRoot, subFolder, (s,l) => Console.WriteLine(s));
		}
	}

	public class OneFileOnAmazonContext : RepositoryFolderContext {
		public readonly string RELATIVE_PATH = "folder/file";
		public string FileFullPath { get; set; }
		public override void Create() {
			base.Create();
			FileFullPath = FileSystem.Combine(base.RepositoryRoot, RELATIVE_PATH.Replace('/', '\\'));

			//upload it to Amazon
			var client = Container.Get<IS3Client>();
			var sourcePath = FileSystem.Combine(AppRoot, "UserFiles", "dummy.txt");
			client.PutFile("chpokk", FilePathRelativeToAppRoot, sourcePath, true, 0);
		}
		public string FilePathRelativeToAppRoot {
			get {
				return "UserFiles/ulu/" + REPO_NAME + "/" + RELATIVE_PATH;
			}
		}
	}
}
