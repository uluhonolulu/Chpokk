using System;
using System.IO;
using System.Linq;
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
		private RepositoryManager _repositoryManager;

		[JsonEndpoint]
		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			if (input.Text == null) return null;
			var resolver = new NRefactoryResolver(LanguageProperties.CSharp);

			var projectContent = DefaultProjectContent;
			TextReader textReader = new StringReader(input.Text);
			var compilationUnit = Compile(projectContent, textReader);

			var repositoryRoot = _repositoryManager.GetRepositoryInfo(input.RepositoryName).Path;
			var projectFilePath = FileSystem.Combine(input.PhysicalApplicationPath, repositoryRoot, input.ProjectPath);
			var projectFileContent = _fileSystem.ReadStringFromFile(projectFilePath);
			var projectFolder = projectFilePath.ParentDirectory();
			foreach (var fileItem in _projectParser.GetCompiledFiles(projectFileContent)) {
				var filePath = FileSystem.Combine(projectFolder, fileItem.Path);
				if (_fileSystem.FileExists(filePath)) {
					var classContent = _fileSystem.ReadStringFromFile(filePath);
					Compile(projectContent, new StringReader(classContent), filePath);						
				}
			
			}


			var text = input.Text;//.Insert(input.Position, input.NewChar.ToString());
			var parseInformation =  new ParseInformation(compilationUnit);
			var expression = FindExpression(text, input.Position, parseInformation);
			var resolveResult = resolver.Resolve(expression, parseInformation, text);
			//Console.WriteLine(input.Text);
			//Console.WriteLine(compilationUnit);
			Console.WriteLine(expression.Expression);
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

		private ICompilationUnit Compile(DefaultProjectContent projectContent, TextReader textReader, string fileName = "nofile") {
			ICompilationUnit compilationUnit;
			using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, textReader)) {
				parser.ParseMethodBodies = false;
				parser.Parse();
				compilationUnit = this.ConvertCompilationUnit(parser.CompilationUnit, projectContent);
			}
			projectContent.UpdateCompilationUnit(null, compilationUnit, fileName);
			return compilationUnit;
		}

		private static DefaultProjectContent _projectContent;
		public IntelController(IFileSystem fileSystem, ProjectParser projectParser, RepositoryManager repositoryManager) {
			_fileSystem = fileSystem;
			_projectParser = projectParser;
			_repositoryManager = repositoryManager;
		}

		private static DefaultProjectContent DefaultProjectContent {
			get {
				if (_projectContent == null) {
					var pcRegistry = new ProjectContentRegistry();

					_projectContent = new DefaultProjectContent() {Language = LanguageProperties.CSharp};
					_projectContent.AddReferencedContent(pcRegistry.Mscorlib);
					
				}
				return _projectContent;
			}
		}

		public static void WarmUp() {
			var x = IntelController.DefaultProjectContent;
		}

		private ExpressionResult FindExpression(string text, int offset, ParseInformation parseInformation) {
			var finder = new CSharpExpressionFinder(parseInformation);
			var expression = finder.FindExpression(text, offset);
			return expression;
			return new ExpressionResult();
		}

		ICompilationUnit ConvertCompilationUnit(ICSharpCode.NRefactory.Ast.CompilationUnit cu, IProjectContent projectContent) {
			var converter = new NRefactoryASTConvertVisitor(projectContent, SupportedLanguage.CSharp);
			cu.AcceptVisitor(converter, null);
			return converter.Cu;
		}
	}
}