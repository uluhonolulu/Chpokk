using System;
using System.Collections.Generic;
using System.IO;
using Arractas;
using Chpokk.Tests.GitHub.Infrastructure;
using ChpokkWeb.Repa;
using LibGit2Sharp;
using LibGit2Sharp.Tests.TestHelpers;
using MbUnit.Framework;
using System.Linq;

namespace Chpokk.Tests.GitHub {
	[TestFixture]
	public class Cloning: BaseQueryTest<CloneContext, Repository> {
		public override Repository Act() {
			const string repoUrl = "git://github.com/uluhonolulu/Chpokk-Scratchpad.git";
			return CloneRepository(repoUrl, Context.RepositoryPath);
		}

		[Test]
		public void CreatesThePhysicalFiles() {
			var expectedFile = Path.Combine(Context.RepositoryPath, Context.FilePath);
			var existingFiles = Directory.GetFiles(Repository.Info.WorkingDirectory);
			Assert.AreElementsEqual(new [] {expectedFile}, existingFiles);
		}


		private static Repository CloneRepository(string repoUrl, string repositoryPath) {
			var repository = Repository.Clone(repoUrl, repositoryPath);
			var master = repository.Branches["master"];
			repository.Checkout(master);
			return repository;
		}

		private Repository Repository {
			get { return Result; }
		}
	}

	public class CloneContext : SimpleContext, IDisposable {
		public string RepositoryPath { get; private set; }
		public string FilePath { get; private set; }
		public override void Create() {
			RepositoryPath  = Path.Combine(Path.GetFullPath(@".."), new RepositoryInfo().Path);
			FilePath = Guid.NewGuid().ToString();
			var content = "stuff";
			Api.CommitFile(FilePath, content);
		}

		public void Dispose() {
			DirectoryHelper.DeleteDirectory(RepositoryPath);			
		}
	}

	public static class CheckoutExtensions {
		/// <summary>
		/// Performs a checkout of an existing branch.
		/// </summary>
		/// <param name="branch">The <see cref = "LibGit2Sharp.Branch" /> to be checked out.</param>
		/// <remarks>Overwrites the existing files.</remarks>
		public static void Checkout(this Repository repo, Branch branch) {
			repo.Refs.UpdateTarget("HEAD", branch.CanonicalName);
			repo.Reset(ResetOptions.Mixed, branch.CanonicalName);

			repo.WriteFilesToDisk(branch);
		}

		public static void WriteFilesToDisk(this Repository repo, Branch branch) {
			IEnumerable<string> fileNames = repo.Index.Select(entry => entry.Path).Where(path => branch.Tip[path] != null);
			string workingDirectory = repo.Info.WorkingDirectory;
			foreach (string fileName in fileNames) {
				byte[] content = ((Blob)(branch.Tip.Tree[fileName].Target)).Content;
				string filePath = Path.Combine(workingDirectory, fileName);
				File.WriteAllBytes(filePath, content);
			}
		}		
	}


}
