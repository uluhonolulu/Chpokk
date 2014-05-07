using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Intellisense;
using ChpokkWeb.Features.Exploring.Rename;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.Renaming {
	[TestFixture]
	public class ItemRenaming: BaseCommandTest<SolutionWithProjectAndClassFileContext> {
		private const string NEW_NAME = "NewName";
		[Test]
		public void NameAndPathChangedInProjectFile() {
			ProjectItemElement.Include.ShouldBe(NEW_NAME);
		}

		private ProjectItemElement ProjectItemElement {
			get {
				var project = ProjectRootElement.Open(Context.ProjectFilePath);
				var itemElement = project.Items.First();
				return itemElement;
			}
		}

		public override void Act() {
			var oldName = "Class1.cs";
			var endpoint = Context.Container.Get<RenameEndpoint>();
			endpoint.DoIt(new RenameInputModel {
				RepositoryName = Context.REPO_NAME,
				PathRelativeToRepositoryRoot = Context.ClassFileRelativePath,
				NewFileName = NEW_NAME,
				ProjectPath = Context.ProjectPathRelativeToRepositoryRoot,
				ItemType = "Item"
			});
		}
	}
}
