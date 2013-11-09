using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddItem;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.ItemAdding {
	[TestFixture]
	public class ExecutingAddItemCommand : BaseCommandTest<SingleSolutionWithProjectFileContext> {
		private Project _project;
		private const string FILE_NAME = @"folder\filename";

		[Test]
		public void AddsAnItemToTheProjectFile() {
			var xmlDocument = new XmlDocument();
			xmlDocument.Load(Context.ProjectFilePath);
			DocumentShouldHaveCompileEntryForTheNewFile(xmlDocument);
		}

		[Test]
		public void CreatesAPhysicalFile() {
			Console.WriteLine("Checking " + Context.GetFilePath(FILE_NAME)); 
			Context.Container.Get<IFileSystem>().FileExists(Context.GetFilePath(FILE_NAME)).ShouldBe(true);
		}

		private void DocumentShouldHaveCompileEntryForTheNewFile(XmlDocument xmlDocument) {
			var manager = new XmlNamespaceManager(xmlDocument.NameTable);
			manager.AddNamespace("x", xmlDocument.DocumentElement.NamespaceURI);
			var xpath = "//x:Compile[@Include='{0}']".ToFormat(FILE_NAME);
			xmlDocument.SelectSingleNode(xpath, manager).ShouldNotBe(null);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddItemEndpoint>();
			endpoint.DoIt(new AddItemInputModel()
			{
				PhysicalApplicationPath = Context.AppRoot,
				RepositoryName = Context.REPO_NAME,
				ProjectPath = Context.ProjectPathRelativeToRepositoryRoot,
				PathRelativeToRepositoryRoot = FileSystem.Combine(Context.ProjectFolderRelativeToRepositoryRoot, FILE_NAME) 
			});
			//_project.Save();
			//project.Build(new ConsoleLogger());
		}
	}

	public class ConsoleLogger: ILogger {
		public void Initialize(IEventSource eventSource) {
			eventSource.AnyEventRaised += (sender, args) => Console.WriteLine(args.Message);
		}
		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }
	}
}
