using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Editor.Compilation {
	public class CompilerEndpoint {
		public AjaxContinuation DoIt(CompileInputModel model) {
			var projectCollection = ProjectCollection.GlobalProjectCollection;
			var logger = new ChpokkLogger();
			projectCollection.RegisterLogger(logger);
			var projectFilePath = FileSystem.Combine(model.PhysicalApplicationPath,
										  @"UserFiles\uluhonolulu\Chpokk-SampleSol\src\ConsoleApplication1\ConsoleApplication1.csproj");
			var project = projectCollection.LoadProject(projectFilePath);
			bool result = project.Build();
			var ajaxContinuation = AjaxContinuation.Successful();
			ajaxContinuation.Message = "Success: " + result.ToString();
			ajaxContinuation.Errors.AddRange(from message in logger.Messages select new AjaxError() {message = message});
			return ajaxContinuation;
		}
	}

	public class CompileInputModel: BaseRepositoryInputModel {}

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