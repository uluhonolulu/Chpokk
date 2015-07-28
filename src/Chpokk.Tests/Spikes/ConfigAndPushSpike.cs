using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Amazon;
using ChpokkWeb.Features.Remotes.Git.Push;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class ConfigAndPushSpike: BaseCommandTest<OneFileInRepositoryFolderContext> {
		[Test]
		public void Test() {
			//
			// TODO: Add test logic here
			//
		}

		public override void Act() {
			var model = new PushInputModel
			{
				NewRemote = "Azure",
				NewRemoteUrl = "https://chpokkie1.scm.azurewebsites.net:443/chpokkie1.git",
				RepositoryName = Context.REPO_NAME,
				Username = "uluhonolulu",
				Password = "xd11SvG23"
			};
			Context.Container.Get<PushEndpoint>().Push(model);

		}
	}
}
