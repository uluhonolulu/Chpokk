using System.IO;
using Arractas;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Remotes;
using MbUnit.Framework;

namespace Chpokk.Tests.GitHub {
	[TestFixture]
	public class Cloning: BaseCommandTest<RemoteRepositoryContext> {
		public override void Act() {
			const string repoUrl = "git://github.com/uluhonolulu/Chpokk-Scratchpad.git";
			var model = new CloneInputModel {PhysicalApplicationPath = Context.AppRoot, RepoUrl = repoUrl};
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
			Assert.IsTrue(manager.RepositoryNameIsValid(Context.RepositoryName, Context.AppRoot));
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
