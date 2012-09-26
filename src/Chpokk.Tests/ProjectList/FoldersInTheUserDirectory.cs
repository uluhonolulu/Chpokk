using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.ProjectList
{
	[TestFixture]
	public class FoldersInTheUserDirectory:BaseQueryTest<RepositoryFolderContext, IEnumerable<string>>
	{
		[Test]
		public void ProvideTheListOfRepositories  ()
		{
			Assert.AreElementsEqual(new[] {Context.REPO_NAME}, Result);
		}

		public override IEnumerable<string> Act() {
			var manager = Context.Container.Get<RepositoryManager>();
			return manager.GetRepositoryNames(Context.AppRoot);
		}
	}
}
