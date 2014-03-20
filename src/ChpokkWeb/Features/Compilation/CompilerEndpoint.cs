using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChpokkWeb.Features.Files;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Running;
using FubuCore;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.Compilation {
	public class CompilerEndpoint {
		private readonly ChpokkLogger _logger;
		private readonly RepositoryManager _repositoryManager;
		private readonly MsBuildCompiler _compiler;
		private readonly ExeRunner _exeRunner;
		private readonly Savior _savior;
		public CompilerEndpoint(ChpokkLogger logger, RepositoryManager repositoryManager, MsBuildCompiler compiler, ExeRunner exeRunner, Savior savior) {
			_logger = logger;
			_repositoryManager = repositoryManager;
			_compiler = compiler;
			_exeRunner = exeRunner;
			_savior = savior;
		}		
		public AjaxContinuation Compile(CompileInputModel model) {
			_logger.ConnectionId = model.ConnectionId;
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			var result = _compiler.Compile(projectFilePath, _logger);
			return new AjaxContinuation {Success = result.Success};
		}

		public AjaxContinuation CompileAndRun(CompileAndRunInputModel model) {
			_logger.ConnectionId = model.ConnectionId;
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			var compilationResult = _compiler.Compile(projectFilePath, _logger);
			if (!compilationResult.Success) {
				return new AjaxContinuation { Success = false};
			}

			//now, RUN!!!
			if (compilationResult.OutputType != "Exe") {
				throw new InvalidOperationException("I can run only console apps");
			}
			Task.Run(() => {
				_logger.SendMessage("");
				_logger.SendMessage("Running..", ChpokkLogger.MessageType.Success);
				var exePath = compilationResult.OutputFilePath;
				var runnerResult = _exeRunner.RunMain(exePath, c => _logger.SendMessage(c.ToString(), ChpokkLogger.MessageType.Info, false), c => _logger.SendMessage(c.ToString(), ChpokkLogger.MessageType.Error, false));
				runnerResult = runnerResult ?? "null";
				_logger.SendMessage("The program returned " + runnerResult, ChpokkLogger.MessageType.Success, true);
			});

			return AjaxContinuation.Successful();
		}
	}

	public class CompileInputModel : SaveFileInputModel {
		public string ProjectPath { get; set; }
		public string ConnectionId { get; set; }
	}

	public class CompileAndRunInputModel : CompileInputModel {}
}