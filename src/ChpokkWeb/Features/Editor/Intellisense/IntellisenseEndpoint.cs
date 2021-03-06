﻿using System;
using System.Linq;
using ChpokkWeb.Features.LanguageSupport;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core.Security;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntellisenseEndpoint {
		private readonly CompletionProvider _completionProvider;
		private readonly IntelDataLoader _intelDataLoader;
		private readonly RepositoryManager _repositoryManager;
		private readonly LanguageDetector _languageDetector;
		private readonly ExceptionNotifier _exceptionNotifier;
		public IntellisenseEndpoint(CompletionProvider completionProvider, IntelDataLoader intelDataLoader, RepositoryManager repositoryManager, LanguageDetector languageDetector, ExceptionNotifier exceptionNotifier) {
			_completionProvider = completionProvider;
			_intelDataLoader = intelDataLoader;
			_repositoryManager = repositoryManager;
			_languageDetector = languageDetector;
			_exceptionNotifier = exceptionNotifier;
		}

		//TODO: Check what happens when "Console" or "Sys" or"System" 
		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			_repositoryManager.EnsureAuthenticated();
			var projectPath = _repositoryManager.NewGetAbsolutePathFor(input.RepositoryName, input.ProjectPath);
			var filePath = _repositoryManager.NewGetAbsolutePathFor(input.RepositoryName, input.PathRelativeToRepositoryRoot);
			var intelData = _intelDataLoader.CreateIntelData(projectPath, filePath, input.Content);
			try {
				var symbols = _completionProvider.GetSymbols(input.Content, input.Position, intelData.OtherContent, intelData.ReferencePaths, _languageDetector.GetRoslynLanguage(input.PathRelativeToRepositoryRoot));
				return new IntelOutputModel { Items = symbols.Distinct().OrderBy(item => item.Name).ToArray() };
			}
			catch (Exception exception) {
				_exceptionNotifier.Notify(exception); 
				return new IntelOutputModel();
			}
		}
	}
}