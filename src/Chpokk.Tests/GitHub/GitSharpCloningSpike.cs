using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.GitHub;
using Gallio.Framework;
using GitSharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests {
	[TestFixture]
	public class GitSharpCloningSpike : BaseCommandTest<RemoteRepositoryContext> {
		const string repoUrl = "git://github.com/uluhonolulu/Chpokk-Scratchpad.git";
		[Test]
		public void CreatesThePhysicalFiles() {
			var existingFiles = Directory.GetFiles(Context.RepositoryPath);
			Assert.AreElementsEqual(new[] { Context.FilePath }, existingFiles);
		}

		public override void Act() {
			var path = Context.RepositoryPath;
			Git.Clone(repoUrl, path).Dispose();
		}
	}
}
