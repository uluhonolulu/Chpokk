using System.Xml;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddItem;
using FubuCore;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.ItemAdding {
	[TestFixture]
	public class ExecutingAddItemCommandForNonCodeFiles : BaseCommandTest<SingleSolutionWithProjectFileContext> {
		private const string FILE_NAME = @"folder\filename.html";

		[Test]
		public void TheAddedItemHasContentBuildAction() {
			var xmlDocument = new XmlDocument();
			xmlDocument.Load(Context.ProjectFilePath);
			DocumentShouldHaveContentEntryForTheNewFile(xmlDocument);
		}

		private void DocumentShouldHaveContentEntryForTheNewFile(XmlDocument xmlDocument) {
			var manager = new XmlNamespaceManager(xmlDocument.NameTable);
			manager.AddNamespace("x", xmlDocument.DocumentElement.NamespaceURI);
			var xpath = "//x:Content[@Include='{0}']".ToFormat(FILE_NAME);
			xmlDocument.SelectSingleNode(xpath, manager).ShouldNotBe(null);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddItemEndpoint>();
			endpoint.DoIt(new AddItemInputModel()
				{
					RepositoryName = Context.REPO_NAME,
					ProjectPath = Context.ProjectPathRelativeToRepositoryRoot,
					PathRelativeToRepositoryRoot = FileSystem.Combine(Context.ProjectFolderRelativeToRepositoryRoot, FILE_NAME) 
				});
			//_project.Save();
			//project.Build(new ConsoleLogger());
		}
	}
}