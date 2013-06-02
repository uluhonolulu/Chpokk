using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using Emkay.S3;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class Uploading : BaseCommandTest<OneFileInRepositoryFolderContext> {

		[Test]
		public void TheUploadedFolderAppearsOnS3() {
			var client = Context.Container.Get<IS3Client>();
			var allFiles = client.EnumerateChildren("chpokk");
			foreach (var file in allFiles) {
				Console.WriteLine(file);
			}
			Assert.Contains(allFiles, "UserFiles/ulu/" + Context.REPO_NAME + "/" + Context.RELATIVE_PATH);
		}

		public override void Act() {
			var uploader =  Context.Container.Get<Uploader>();
			Console.WriteLine("--");
			var path = Context.RepositoryRoot;
			uploader.PublishFolder(path, Context.AppRoot);
			Console.WriteLine("--");
		}
	}

	public class OneFileInRepositoryFolderContext:RepositoryFolderContext {
		public readonly string RELATIVE_PATH = "folder/file";
		public string FileFullPath { get; set; }
		public override void Create() {
			base.Create();
			FileFullPath = FileSystem.Combine(base.RepositoryRoot, RELATIVE_PATH);
			new FileSystem().WriteStringToFile(FileFullPath, string.Empty);
		}

		
	}
}
