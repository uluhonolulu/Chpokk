using System;
using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.Files;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Features.Running;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.AspNet.SignalR;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class CompilerHub : Hub, ILogger {
		private readonly RepositoryManager _repositoryManager;
		private readonly MsBuildCompiler _compiler;
		private readonly ExeRunner _exeRunner;
		private readonly Savior _savior;
		public CompilerHub(RepositoryManager repositoryManager, MsBuildCompiler compiler, ExeRunner exeRunner, Savior savior) {
			_repositoryManager = repositoryManager;
			_compiler = compiler;
			_exeRunner = exeRunner;
			_savior = savior;
		}

		public void Compile(CompileInputModel model) {
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			_compiler.Compile(projectFilePath, this);
		}

		public AjaxContinuation CompileAndRun(CompileAndRunInputModel model) {
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.GetAbsolutePathFor(model.RepositoryName, model.PhysicalApplicationPath,
				                                                        model.ProjectPath);
			var compilationResult = _compiler.Compile(projectFilePath, this);
			if (!compilationResult.Success) {
				return new AjaxContinuation { Success = false};
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
			Clients.Caller.success(message);
			return new AjaxContinuation{Success = success, Message = message};
		}

		private enum MessageType {
			Info, Error, Success
		}

		private void SendMessage(string text, MessageType type = MessageType.Info) {
			switch (type) {
				case MessageType.Success:
					Clients.Caller.success(text);
					break;
				case MessageType.Info:
					Clients.Caller.info(text);
					break;
				case MessageType.Error:
					Clients.Caller.danger(text);
					break;
			}
		}

		public void Initialize(IEventSource eventSource) {
			Verbosity = LoggerVerbosity.Normal;
			eventSource.BuildStarted += (sender, args) => SendMessage(args.Message + " - BuildStarted");
			eventSource.ProjectStarted += (sender, args) => SendMessage(args.Message + " - ProjectStarted"); // much more info here
			//eventSource.StatusEventRaised += (sender, args) => SendMessage(args.Message + " - StatusEventRaised");
			eventSource.ProjectFinished += (sender, args) =>
			{
				var messageType = args.Succeeded ? MessageType.Success : MessageType.Error;
				SendMessage(args.Message + " - " + args.GetType(), messageType);
			};
			eventSource.MessageRaised += (sender, args) => SendMessage(args.Message + " - MessageRaised");
			eventSource.ErrorRaised += (sender, args) => SendMessage(args.Message, MessageType.Error);
			eventSource.BuildFinished += (sender, args) =>
			{
				var messageType = args.Succeeded ? MessageType.Success : MessageType.Error;
				SendMessage(args.Message + " - " + args.GetType(), messageType);
			};
		}
		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }
	}

	public class CompileInputModel : SaveFileInputModel {
		public string ProjectPath { get; set; }
	}

	public class CompileAndRunInputModel : CompileInputModel {}
}