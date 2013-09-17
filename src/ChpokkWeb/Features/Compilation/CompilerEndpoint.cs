using System;
using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class CompilerEndpoint {
		private readonly ChpokkLogger _logger;
		private readonly RepositoryManager _repositoryManager;
		private readonly MsBuildCompiler _compiler;
		public CompilerEndpoint(ChpokkLogger logger, RepositoryManager repositoryManager, MsBuildCompiler compiler) {
			_logger = logger;
			_repositoryManager = repositoryManager;
			_compiler = compiler;
		}

		public AjaxContinuation DoIt(CompileInputModel model) {
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			var result = _compiler.Compile(projectFilePath);
			return new AjaxContinuation {Success = result, Message = _logger.GetLogMessage()};
		}
	}

	public class CompileInputModel: BaseRepositoryInputModel {
		public string ProjectPath { get; set; }
	}
}