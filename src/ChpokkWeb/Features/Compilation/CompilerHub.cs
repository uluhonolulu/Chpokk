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
			var projectFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			_compiler.Compile(projectFilePath, this);
		}

		public void CompileAndRun(CompileAndRunInputModel model) {
			if (model.Content.IsNotEmpty()) {
				_savior.SaveFile(model);
			}
			var projectFilePath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName, model.ProjectPath);
			var compilationResult = _compiler.Compile(projectFilePath, this);
			if (!compilationResult.Success) {
				return;
			}

			//now, RUN!!!
			SendMessage("");
			SendMessage("Running..");
			if (compilationResult.OutputType != "Exe") {
				throw new InvalidOperationException("I can run only console apps");
			}
			var exePath = compilationResult.OutputFilePath;
			var runnerResult = _exeRunner.RunMain(exePath, c => SendMessage(c.ToString(), MessageType.Info, false), c => SendMessage(c.ToString(), MessageType.Error, false));
			runnerResult = runnerResult ?? "null";
			SendMessage("The program returned " + runnerResult);
		}

		private enum MessageType {
			Info, Error, Success
		}

		private void SendMessage(string text, MessageType type = MessageType.Info, bool wrap = true) {
			switch (type) {
				case MessageType.Success:
					Clients.Caller.success(text, wrap);
					break;
				case MessageType.Info:
					Clients.Caller.info(text, wrap);
					break;
				case MessageType.Error:
					Clients.Caller.danger(text, wrap);
					break;
			}
		}

		public void Initialize(IEventSource eventSource) {
			Verbosity = LoggerVerbosity.Quiet;
			eventSource.BuildStarted += (sender, args) => SendMessage(args.Message);
			eventSource.ProjectStarted += (sender, args) => SendMessage(args.Message); // much more info here
			eventSource.ProjectFinished += (sender, args) =>
			{
				var messageType = args.Succeeded ? MessageType.Success : MessageType.Error;
				SendMessage(args.Message, messageType);
			};
			eventSource.MessageRaised += (sender, args) => {
				if (args.Importance > MessageImportance.Normal) {
					SendMessage(args.Message);
				}
			};
			eventSource.ErrorRaised += (sender, args) => SendMessage(args.Message + ": " + args.File + ", line " + args.LineNumber + ", position " + args.ColumnNumber, MessageType.Error);
			eventSource.BuildFinished += (sender, args) =>
			{
				var messageType = args.Succeeded ? MessageType.Success : MessageType.Error;
				SendMessage(args.Message, messageType);
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