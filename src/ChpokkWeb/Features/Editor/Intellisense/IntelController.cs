using System;
using System.IO;
using System.Linq;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntelController {
		private readonly IFileSystem _fileSystem;
		private readonly ProjectParser _projectParser;
		private readonly RepositoryManager _repositoryManager;
		private readonly Compiler _compiler;
		private readonly NRefactoryResolver _resolver;

		[JsonEndpoint]
		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			//TODO: make it accept just a bunch of strings that are file contentses
			if (input.Text == null) return null;

			var projectContent = Compiler.DefaultProjectContent;
			TextReader textReader = new StringReader(input.Text);
			var compilationUnit = _compiler.Compile(projectContent, textReader);

			var repositoryRoot = _repositoryManager.GetRepositoryInfo(input.RepositoryName).Path;
			var projectFilePath = FileSystem.Combine(input.PhysicalApplicationPath, repositoryRoot, input.ProjectPath);
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolder = projectFilePath.ParentDirectory();
			foreach (var fileItem in _projectParser.GetCompiledFiles(projectFileContent)) {
				var filePath = FileSystem.Combine(projectFolder, fileItem.Path);
				if (_fileSystem.FileExists(filePath)) {
					var classContent = _fileSystem.ReadStringFromFile(filePath);
					_compiler.Compile(projectContent, new StringReader(classContent), filePath);						
				}
			
			}


			var text = input.Text;//.Insert(input.Position, input.NewChar.ToString());
			var parseInformation =  new ParseInformation(compilationUnit);
			var expression = Compiler.FindExpression(text, input.Position, parseInformation);
			var resolveResult = _resolver.Resolve(expression, parseInformation, text);
			//Console.WriteLine(input.Text);
			//Console.WriteLine(compilationUnit);
			//Console.WriteLine(expression.Expression);
			//Console.WriteLine(resolveResult.ToString());
			if (resolveResult == null) {
				return new IntelOutputModel{Message = "ResolveResult is null"};
			}
			var completionData = resolveResult.GetCompletionData(projectContent);
			if (completionData == null) {
				return new IntelOutputModel{Message = "Completion Data is null"};
			}

			var items = from entry in completionData.OfType<IMember>() select new IntelOutputModel.IntelModelItem {Name = entry.Name, EntityType = entry.EntityType.ToString()};
			var model = new IntelOutputModel {Message = input.Message, Items = items.Distinct().ToArray()};
			return model;
		}

		public IntelController(IFileSystem fileSystem, ProjectParser projectParser, RepositoryManager repositoryManager, Compiler compiler, NRefactoryResolver resolver) {
			_fileSystem = fileSystem;
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
			_compiler = compiler;
			_resolver = resolver;
		}
	}
}