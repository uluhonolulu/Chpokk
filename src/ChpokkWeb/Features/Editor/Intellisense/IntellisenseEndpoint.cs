using System.Linq;
using Chpokk.Tests.Intellisense.Roslynson;
using ChpokkWeb.Features.LanguageSupport;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Editor.Intellisense {
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

		//TODO: Check what happens when "Console" or "Sys" or"System" 

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