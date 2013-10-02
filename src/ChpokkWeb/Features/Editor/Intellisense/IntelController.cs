using System;
using System.IO;
using System.Linq;
using ChpokkWeb.Features.Compilation;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.LanguageSupport;
using ChpokkWeb.Features.ProjectManagement;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using FubuMVC.Core;
using ICSharpCode.AvalonEdit.Utils;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using ICSharpCode.SharpDevelop;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntelController {
		private readonly IFileSystem _fileSystem;
		private readonly ProjectParser _projectParser;
		private readonly RepositoryManager _repositoryManager;
		private readonly Compiler _compiler;
		//private readonly NRefactoryResolver _resolver;
		private readonly ProjectFactory _projectFactory;
		private readonly LanguageDetector _languageDetector;

		[JsonEndpoint]
		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {


			// for the new implementation, see D:\Projects\OSS\QC\QCCodingServices.NET\NRefactoryTestApp\Views\ProjectView.xaml.cs line 258
			//test
			//return new IntelOutputModel{Items = new IntelOutputModel.IntelModelItem[]{new IntelOutputModel.IntelModelItem{Name = "ahh"}}};

			if (input.Text == null) return null;
			var language = _languageDetector.GetLanguage(input.PathRelativeToRepositoryRoot);


			var projectFilePath = GetProjectFile(input);
			var filePaths = _projectParser.GetFullPathsForCompiledFilesFromProjectFile(projectFilePath);
			var readers = from path in filePaths where _fileSystem.FileExists(path) select new StreamReader(path) as TextReader;
			var projectContent = _projectFactory.GetProjectData(projectFilePath).ProjectContent;
			_compiler.CompileAll(projectContent, readers, language);

			var text = input.Text;//.Insert(input.Position, input.NewChar.ToString());
			TextReader textReader = new StringReader(text);
			var compilationUnit = _compiler.ParseCode(projectContent, textReader, language);
			var parseInformation =  new ParseInformation(compilationUnit);
			var expression = Compiler.FindExpression(text, input.Position, parseInformation, language);
			var languageProperties = _languageDetector.GetLanguageProperties(input.PathRelativeToRepositoryRoot);
			var resolver = new NRefactoryResolver(languageProperties);
			var resolveResult = resolver.Resolve(expression, parseInformation, text);
			if (resolveResult == null) {
				throw new InvalidDataException("ResolveResult is null"); //TODO: when is it null, really?
			}
			if (!resolveResult.IsValid) {
				if (resolveResult is UnknownIdentifierResolveResult) {
					var message = "Unknown identifier: '{0}'".ToFormat(resolveResult.As<UnknownIdentifierResolveResult>().Identifier);
					throw new InvalidDataException(message);
				}
				return new IntelOutputModel { Message = "Resolve Result is invalid: " + resolveResult.ToString() };
			}
			var completionData = resolveResult.GetCompletionData(projectContent);
			if (completionData == null) {
				return new IntelOutputModel{Message = "Completion Data is null"};
			}

			var items = from entry in completionData select GetCompletionModel(entry);
			var model = new IntelOutputModel {Message = input.Message, Items = items.Distinct().OrderBy(item => item.Name).ToArray()};
			return model;
		}

		public IntelOutputModel.IntelModelItem GetCompletionModel(ICompletionEntry entry) {
			var entityEntry = entry as IEntity;
			if (entityEntry != null)
				return new IntelOutputModel.IntelModelItem { Name = entry.Name, EntityType = entityEntry.EntityType.ToString() };
			var entryType = entry.GetType().Name;
			if (entryType.EndsWith("Entry")) entryType = entryType.CutoffEnd("Entry");
			return new IntelOutputModel.IntelModelItem{EntityType = entryType, Name = entry.Name};
		}

		private string GetProjectFile(IntelInputModel input) {
			if (input.ProjectPath.IsEmpty()) {
				throw new InvalidDataException("Project path shouldn't be empty");
			}
			var repositoryRoot = _repositoryManager.GetRepositoryInfo(input.RepositoryName).Path;
			var projectFilePath = FileSystem.Combine(input.PhysicalApplicationPath, repositoryRoot, input.ProjectPath);
			return projectFilePath;
		}

		public IntelController(IFileSystem fileSystem, ProjectParser projectParser, RepositoryManager repositoryManager, Compiler compiler, ProjectFactory projectFactory, LanguageDetector languageDetector) {
			_fileSystem = fileSystem;
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
			_compiler = compiler;
			_projectFactory = projectFactory;
			_languageDetector = languageDetector;
		}
	}
}