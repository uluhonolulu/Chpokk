using System;
using System.IO;
using Arractas;
using Chpokk.Tests.GitHub.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes;
using LibGit2Sharp.Tests.TestHelpers;
using MbUnit.Framework;
using StructureMap;

namespace Chpokk.Tests.Cloning {
	public class WhenYouSendACloneCommandToAServer {
		private string _fileName;
		private string _targetFolder;

		[FixtureSetUp]
		public void Setup() {
			const string repoUrl = "git://github.com/uluhonolulu/Chpokk-Scratchpad.git";
			// ARRANGE

			// Create a random filename
			_fileName = Guid.NewGuid().ToString();
			// Commit a new file to the remote repository
			var content = "stuff";
			Api.CommitFile(_fileName, content);

			// Prepare the target folder
			// This is where we get the relative repository path 
 			// See discussion about Rule #2
			var repositoryInfo = ObjectFactory.GetInstance<RepositoryInfo>();
			_targetFolder = Path.Combine(Path.GetFullPath(@".."), repositoryInfo.Path);
			// We cannot clone into a nonempty directory, so delete it
			if (Directory.Exists(_targetFolder))
				DirectoryHelper.DeleteDirectory(_targetFolder);

			// ACT

			// Get an instance of our controller.
			// I'm using a container so that I don't have to rewrite it
			// each time I change the signature of the constructor.
			// For unit tests, use automocking container.
			var controller = ObjectFactory.GetInstance<CloneController>();
			// Create a model for using with our Action Method.
			// PhysicalApplicationPath is bound automatically,
			// but in out test we need to submit it.
			var model = new CloneInputModel 
				{PhysicalApplicationPath = Path.GetFullPath(".."), 
					RepoUrl = repoUrl};
			// Finally, execute the Action method.
			controller.CloneRepository(model);

		}

		[Test]
		public void RepositoryFilesShouldAppearInTheDestinationFolder() {
			var expectedFile = Path.Combine(_targetFolder, _fileName);
			var existingFiles = Directory.GetFiles(_targetFolder);
			Assert.AreElementsEqual(new[] { expectedFile }, existingFiles);
			
		}
	}

}

namespace Chpokk.Tests.GitHub {
	[TestFixture, Ignore("Long running test")]
	public class Cloning: BaseCommandTest<RemoteRepositoryContext> {
		public override void Act() {
			const string repoUrl = "git://github.com/uluhonolulu/Chpokk-Scratchpad.git";
			var model = new CloneInputModel {PhysicalApplicationPath = Path.GetFullPath(".."), RepoUrl = repoUrl};
			Context.Container.Get<CloneController>().CloneRepository(model);
		}

		[Test]
		public void CreatesThePhysicalFiles() {
			var expectedFile = Path.Combine(Context.RepositoryPath, Context.FileName);
			var existingFiles = Directory.GetFiles(Context.RepositoryPath); 
			Assert.AreElementsEqual(new [] {expectedFile}, existingFiles);
		}

		[Test]
		public void RepositoryIsRegisteredByTheManager() {
			var manager = Context.Container.Get<RepositoryManager>();
			Assert.IsTrue(manager.RepositoryNameIsValid(Context.RepositoryName));
		}

	}



	//public static class CheckoutExtensions {
	//    /// <summary>
	//    /// Performs a checkout of an existing branch.
	//    /// </summary>
	//    /// <param name="branch">The <see cref = "LibGit2Sharp.Branch" /> to be checked out.</param>
	//    /// <remarks>Overwrites the existing files.</remarks>
	//    public static void Checkout(this Repository repo, Branch branch) {
	//        repo.Refs.UpdateTarget("HEAD", branch.CanonicalName);
	//        repo.Reset(ResetOptions.Mixed, branch.CanonicalName);

	//        repo.WriteFilesToDisk(branch);
	//    }

	//    public static void WriteFilesToDisk(this Repository repo, Branch branch) {
	//        IEnumerable<string> fileNames = repo.Index.Select(entry => entry.Path).Where(path => branch.Tip[path] != null);
	//        string workingDirectory = repo.Info.WorkingDirectory;
	//        foreach (string fileName in fileNames) {
	//            byte[] content = ((Blob)(branch.Tip.Tree[fileName].Target)).Content;
	//            string filePath = Path.Combine(workingDirectory, fileName);
	//            File.WriteAllBytes(filePath, content);
	//        }
	//    }		
	//}


}
