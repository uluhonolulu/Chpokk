using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.LanguageSupport;
using ChpokkWeb.Features.RepositoryManagement;
using MbUnit.Framework;
using Roslyn.Compilers;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class CallingIntelEndpoint : BaseQueryTest<SolutionWithProjectAndClassFileContext, IEnumerable<IntelOutputModel.IntelModelItem>> {
		public override IEnumerable<IntelOutputModel.IntelModelItem> Act() {
			var endpoint = Context.Container.Get<IntellisenseEndpoint>();
			var source = @"public class X {public void Y(){(new A()).}}";
			var position = source.IndexOf('.');
			var model = new IntelInputModel()
			            {
			            	NewChar = '.',
			            	Position = position,
			            	Text = source,
			            	PhysicalApplicationPath = Context.AppRoot,
			            	RepositoryName = Context.REPO_NAME,
							PathRelativeToRepositoryRoot = "x.cs",
							ProjectPath = Path.Combine("src", Context.PROJECT_PATH) // src\ProjectName\ProjectName.csproj
			            };
			return endpoint.GetIntellisenseData(model).Items;
		}

		[Test]
		public void ContainsTheMethodOfTheClass() {
			var memberNames = Result.Select(item => item.Name);
			Assert.Contains(memberNames, "B");
		}
	}

	//public struct CompletionOptions {
	//	public string Name { get; set; }
	//}

	public class IntellisenseEndpoint {
		private readonly CompletionProvider _completionProvider;
		private readonly IntelDataLoader _intelDataLoader;
		private readonly RepositoryManager _repositoryManager;
		private readonly LanguageDetector _languageDetector;
		public IntellisenseEndpoint(CompletionProvider completionProvider, IntelDataLoader intelDataLoader, RepositoryManager repositoryManager, LanguageDetector languageDetector) {
			_completionProvider = completionProvider;
			_intelDataLoader = intelDataLoader;
			_repositoryManager = repositoryManager;
			_languageDetector = languageDetector;
		}

		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			var projectPath = _repositoryManager.GetAbsolutePathFor(input.RepositoryName, input.PhysicalApplicationPath, input.ProjectPath);
			var filePath = _repositoryManager.GetAbsolutePathFor(input.RepositoryName, input.PhysicalApplicationPath, input.PathRelativeToRepositoryRoot);
			var intelData = _intelDataLoader.CreateIntelData(projectPath, filePath, input.Text);
			var symbols = _completionProvider.GetSymbols(input.Text, input.Position, intelData.OtherContent, intelData.ReferencePaths, _languageDetector.GetRoslynLanguage(input.PathRelativeToRepositoryRoot));
			var completionItems = from symbol in symbols
			                      select new IntelOutputModel.IntelModelItem {Name = symbol.Name, EntityType = symbol.Kind.ToString()};
			return new IntelOutputModel { Items = completionItems.Distinct().OrderBy(item => item.Name).ToArray()};
		}
	}
}
