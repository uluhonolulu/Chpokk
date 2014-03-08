using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using SharpSvn;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class SvnSpike {
		[Test]
		public void CanCreateARepositoryAndCheckoutFilesAndCommit() {
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies) {
				Console.WriteLine(assembly.FullName + ": " + assembly.GetName().ProcessorArchitecture);
			}
			using (var client = new SvnClient()) {
				SvnUpdateResult result;
				SvnRepositoryClient repositoryClient;
				var tmpFolder = Path.GetTempPath();
				Console.WriteLine(tmpFolder);
			}
		}
	}
}
