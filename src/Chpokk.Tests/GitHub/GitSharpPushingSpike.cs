using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Gallio.Framework;
using GitSharp;
using GitSharp.Commands;
using GitSharp.Core.Transport;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.GitHub {
	// Doesn't work
	//[TestFixture]
	//public class GitSharpPushingSpike : BaseCommandTest<ModifiedRepositoryContext> {
	//    [Test]
	//    public void Test() {
	//        //
	//        // TODO: Add test logic here
	//        //
	//    }

	//    public override void Act() {
	//        using (var repository = new Repository(Context.RepositoryPath)) {
	//            var command = new PushCommand() { Repository = repository, RefSpecs = new List<RefSpec> { new RefSpec("HEAD", "refs/heads/master") } };
	//            command.AddAll();
	//            command.Execute();
	//        }
	//        ;
	//    }
	//}
}
