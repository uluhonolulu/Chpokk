using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Authentication.Context;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;

namespace Chpokk.Tests.Authentication {
	[TestFixture]
	public class DontSeeOthersRepos : BaseQueryTest<ClonedPrivateRepoContext, IEnumerable<string>> {
		[Test]
		public void TheListOfRepositoriesShouldbeEmpty() {
			Result.Each(s => Console.Write(s));
			Assert.IsEmpty(Result);
		}

		public override IEnumerable<string> Act() {

			var manager = Context.Container.Get<RepositoryManager>();
			return manager.GetRepositoryNames(Context.AppRoot);
		}
	}


}
