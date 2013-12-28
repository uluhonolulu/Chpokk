using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.References.OtherProjects;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class GettingTheListOfExistingProjects : BaseQueryTest<SingleSolutionWithProjectFileContext, OtherProjectsModel> {
		[Test]
		public void ShouldReturnExistingProject() {
			Result.Projects.Count().ShouldBe(1);
		}

		public override OtherProjectsModel Act() {
			var endpoint = Context.Container.Get<OtherProjectsEndPoint>();
			var model = new OtherProjectsInputModel {RepositoryName = Context.REPO_NAME, SolutionPath = Context.RelativeSolutionPath};
			return endpoint.DoIt(model);
		}
	}
}
