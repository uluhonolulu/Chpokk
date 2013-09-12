using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class CompilerEndpoint {
		private readonly ProjectCollection _projectCollection;
		private readonly ChpokkLogger _logger;
		public CompilerEndpoint(ProjectCollection projectCollection, ChpokkLogger logger) {
			_projectCollection = projectCollection;
			_logger = logger;
		}

		public AjaxContinuation DoIt(CompileInputModel model) {
			_projectCollection.RegisterLogger(_logger);
			var projectFilePath = FileSystem.Combine(model.PhysicalApplicationPath, model.ProjectPath);
			var project = _projectCollection.LoadProject(projectFilePath);
			bool result = project.Build();
			_projectCollection.UnregisterAllLoggers();

			var ajaxContinuation = AjaxContinuation.Successful();
			ajaxContinuation.Message = "Success: " + result.ToString();
			ajaxContinuation.Errors.AddRange(from message in _logger.Messages select new AjaxError() {message = message});
			return ajaxContinuation;
		}
	}

	public class CompileInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
	}

	public class ChpokkLogger: ILogger {
		public ChpokkLogger() {
			Messages = new List<string>();
		}

		public List<string> Messages { get; set; }
		public void Initialize(IEventSource eventSource) {
			eventSource.AnyEventRaised += (sender, args) => Messages.Add(args.Message);
		}
		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }
	}
}