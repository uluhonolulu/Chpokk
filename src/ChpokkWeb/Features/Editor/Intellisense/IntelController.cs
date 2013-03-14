using System;
using System.IO;
using System.Linq;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core;
using ICSharpCode.AvalonEdit.Utils;
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

			if (input.Text == null) return null;

			var projectContent = Compiler.DefaultProjectContent;

			var repositoryRoot = _repositoryManager.GetRepositoryInfo(input.RepositoryName).Path;
			var projectFilePath = FileSystem.Combine(input.PhysicalApplicationPath, repositoryRoot, input.ProjectPath);
			var filePaths = _projectParser.GetFullPathsForCompiledFilesFromProjectFile(projectFilePath);
			var readers = from path in filePaths where _fileSystem.FileExists(path) select new StreamReader(path) as TextReader;
			_compiler.CompileAll(projectContent, readers);

			var text = input.Text;//.Insert(input.Position, input.NewChar.ToString());
			TextReader textReader = new StringReader(text);
			var compilationUnit = _compiler.Compile(projectContent, textReader);
			var parseInformation =  new ParseInformation(compilationUnit);
			var expression = Compiler.FindExpression(text, input.Position, parseInformation);
			var resolveResult = _resolver.Resolve(expression, parseInformation, text);
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