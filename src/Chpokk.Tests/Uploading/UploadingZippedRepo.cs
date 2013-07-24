using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Arractas;
using Bottles;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.Remotes.UploadZip;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure.SimpleZip;
using FubuCore;
using Gallio.Framework;
using ICSharpCode.SharpZipLib.Zip;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using LibGit2Sharp.Tests.TestHelpers;
using Shouldly;
using TypeMock.ArrangeActAssert;

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
			var zipper = Context.Container.Get<Zipper>();
			using (Stream fileStream = File.OpenRead(UploadingZipFileContext.ZIP_FILE_NAME)) {
				var postedFile = Isolate.Fake.Instance<HttpPostedFileBase>(Members.MustSpecifyReturnValues);
				Isolate.WhenCalled(() => postedFile.InputStream).WillReturn(fileStream);
				Isolate.WhenCalled(() => postedFile.FileName).WillReturn(UploadingZipFileContext.ZIP_FILE_NAME);
				var model = new UploadZipInputModel{ZippedRepository = postedFile, PhysicalApplicationPath = Context.AppRoot};
				var endpoint = Context.Container.Get<UploadZipEndpoint>();
				endpoint.Upload(model);
				
			}
		}
	}
}
