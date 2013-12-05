using System;
using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Files;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Running;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class CompilerEndpoint {
		private readonly ChpokkLogger _logger;
		private readonly RepositoryManager _repositoryManager;
		private readonly MsBuildCompiler _compiler;
		private readonly ExeRunner _exeRunner;
		private Savior _savior;
		public CompilerEndpoint(ChpokkLogger logger, RepositoryManager repositoryManager, MsBuildCompiler compiler, ExeRunner exeRunner, Savior savior) {
			_logger = logger;
			_repositoryManager = repositoryManager;
			_compiler = compiler;
			_exeRunner = exeRunner;
			_savior = savior;
		}

		public AjaxContinuation Compile(CompileInputModel model) {
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			var result = _compiler.Compile(projectFilePath);
			return new AjaxContinuation {Success = result.Success, Message = _logger.GetLogMessage()};
		}

		public AjaxContinuation CompileAndRun(CompileAndRunInputModel model) {
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			var compilationResult = _compiler.Compile(projectFilePath);
			if (!compilationResult.Success) {
				return new AjaxContinuation { Success = false, Message = _logger.GetLogMessage() };
			}

			//now, RUN!!!
			if (compilationResult.OutputType != "Exe") {
				throw new InvalidOperationException("I can run only console apps");
			}
			var exePath = compilationResult.OutputFilePath;
			var runnerResult = _exeRunner.RunMain(exePath);
			var success = runnerResult.ErrorOutput.IsEmpty();
			var message = string.Concat(runnerResult.StandardOutput, runnerResult.ErrorOutput);
			var result = runnerResult.Result?? 0;
			message += "The program exited with code " + result;
			return new AjaxContinuation{Success = success, Message = message};
		}
	}

	public class CompileInputModel : SaveFileInputModel {
		public string ProjectPath { get; set; }
	}

	public class CompileAndRunInputModel : CompileInputModel {}
}