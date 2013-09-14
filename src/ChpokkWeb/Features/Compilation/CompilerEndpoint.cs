using System;
using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class CompilerEndpoint {
		private readonly ProjectCollection _projectCollection;
		private readonly ChpokkLogger _logger;
		private readonly RepositoryManager _repositoryManager;
		public CompilerEndpoint(ProjectCollection projectCollection, ChpokkLogger logger, RepositoryManager repositoryManager) {
			_projectCollection = projectCollection;
			_logger = logger;
			_repositoryManager = repositoryManager;
		}

		public AjaxContinuation DoIt(CompileInputModel model) {
			//_logger.Verbosity = LoggerVerbosity.Minimal;
			_projectCollection.RegisterLogger(_logger);
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
			                                                            model.ProjectPath);
			var project = _projectCollection.LoadProject(projectFilePath);
			var result = project.Build();
			foreach (var @event in _logger.Events.OfType<BuildErrorEventArgs>()) {
				Console.WriteLine(@event.ToString() + ": " + @event.Message);
			}
			_projectCollection.UnregisterAllLoggers();

			var ajaxContinuation = AjaxContinuation.Successful();
			ajaxContinuation.Success = result;
			//ajaxContinuation.Message = "Success: " + result.ToString();
			ajaxContinuation.Errors.AddRange(from message in _logger.Messages select new AjaxError() {message = message});
			return ajaxContinuation;
		}
	}

	public class CompileInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
	}
}