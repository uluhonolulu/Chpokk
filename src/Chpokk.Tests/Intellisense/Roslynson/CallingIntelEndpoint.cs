using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.RepositoryManagement;
using Roslyn.Compilers;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class CallingIntelEndpoint : BaseQueryTest<PhysicalCodeFileContext, IEnumerable<IntelOutputModel.IntelModelItem>> {
		public override IEnumerable<IntelOutputModel.IntelModelItem> Act() {
			var endpoint = Context.Container.Get<IntellisenseEndpoint>();
			return endpoint.GetIntellisenseData(new IntelInputModel()).Items;
		}
	}

	public struct CompletionOptions {
		public string Name { get; set; }
	}

	public class IntellisenseEndpoint {
		private readonly CompletionProvider _completionProvider;
		private readonly IntelDataLoader _intelDataLoader;
		private readonly RepositoryManager _repositoryManager;
		public IntellisenseEndpoint(CompletionProvider completionProvider, IntelDataLoader intelDataLoader, RepositoryManager repositoryManager) {
			_completionProvider = completionProvider;
			_intelDataLoader = intelDataLoader;
			_repositoryManager = repositoryManager;
		}

		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			var otherSources = new string[] {};
			var projectPath = _repositoryManager.GetAbsolutePathFor(input.RepositoryName, input.PhysicalApplicationPath, input.ProjectPath);
			var filePath = _repositoryManager.GetAbsolutePathFor(input.RepositoryName, input.PhysicalApplicationPath, input.PathRelativeToRepositoryRoot);
			var intelData = _intelDataLoader.CreateIntelData(projectPath, filePath, input.Text);
			var symbols = _completionProvider.GetSymbols(input.Text, input.Position, otherSources, intelData.ReferencePaths, LanguageNames.CSharp);
			var completionItems = from symbol in symbols
			                      select new IntelOutputModel.IntelModelItem {Name = symbol.Name, EntityType = symbol.Kind.ToString()};
			return new IntelOutputModel { Items = completionItems.ToArray()};
		}
	}
}
