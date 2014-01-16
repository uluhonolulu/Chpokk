using System;
using System.Collections.Generic;
using System.Linq;
using ChpokkWeb.Features.Editor.Intellisense.Providers;
using Microsoft.Scripting;
using Roslyn.Compilers;
//using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense {
	
	public class CompletionProvider {
		private readonly KeywordProvider _keywordProvider;
		public CompletionProvider(KeywordProvider keywordProvider) {
			_keywordProvider = keywordProvider;
		}

		//1. notdot can be local members (method params -- actually a global, local vars, class members), namespaces, and classes (interfaces) within imported namespaces, also keywords
		//2. dot can be depending on what's before the dot -- can be namespace -> class or namespace, class -> static members, identifier or member -> members
		//3. usings can have only namespaces or classes 
		//4. check the imported namespaces and the current namespace
		//5. maybe should use multiple providers 
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(string source, int position, IEnumerable<string> otherSources, IEnumerable<string> assemblyPaths, string language) {
			CommonSyntaxTree tree;
			CommonCompilation compilation;
			IEnumerable<CommonSyntaxTree> syntaxTrees; 
			var references = from assemblyPath in assemblyPaths select  new MetadataFileReference(assemblyPath);
			switch (language) {
				case LanguageNames.CSharp:
					tree = Roslyn.Compilers.CSharp.SyntaxTree.ParseText(source);
					syntaxTrees = from code in otherSources select Roslyn.Compilers.CSharp.SyntaxTree.ParseText(code);
					syntaxTrees = syntaxTrees.Union(new[] {tree});
					compilation = Roslyn.Compilers.CSharp.Compilation.Create("MyCompilation", syntaxTrees: syntaxTrees.Cast<Roslyn.Compilers.CSharp.SyntaxTree>(), references: references);break;
				case LanguageNames.VisualBasic:
					tree = Roslyn.Compilers.VisualBasic.SyntaxTree.ParseText(source);
					syntaxTrees = from code in otherSources select Roslyn.Compilers.VisualBasic.SyntaxTree.ParseText(code);
					syntaxTrees = syntaxTrees.Union(new[] {tree});
					compilation = Roslyn.Compilers.VisualBasic.Compilation.Create("MyCompilation", syntaxTrees: syntaxTrees.Cast< Roslyn.Compilers.VisualBasic.SyntaxTree>()); break;
				default: throw new NotSupportedException("Language '" + language + "' is not supported.");
			}
			
			var semanticModel = compilation.GetSemanticModel(tree);
			var declaredSymbol = GetContainingClass(position, tree, semanticModel); //actually return INamespaceOrTypeSymbol and check 
			var symbols = semanticModel.LookupSymbols(position, declaredSymbol).AsEnumerable();
			if (declaredSymbol != null && !this.IsDotCompletion(position, tree)) {
				var globals = semanticModel.LookupSymbols(position);
				symbols = symbols.Union(globals.AsEnumerable());
			}
			//tinky-winky
			var token = tree.GetRoot().FindToken(position);
			var dotlessSymbols = new DotlessCompletionProvider().GetSymbols(token, semanticModel, position);
			var namespaceSymbols = new DotCompletionProvider().GetSymbols(token, semanticModel, position);

			var symbolItems = from symbol in symbols select IntelOutputModel.IntelModelItem.FromSymbol(symbol);
			symbolItems = Enumerable.Empty<IntelOutputModel.IntelModelItem>();

			//TODO: using namespaces -- after dot completion

			var globalSymbols = new GlobalCompletionProvider().GetSymbols(token, semanticModel, position);
			symbolItems = symbolItems.Union(globalSymbols);
			symbolItems = symbolItems.Union(namespaceSymbols);
			symbolItems = symbolItems.Union(dotlessSymbols);
			symbolItems = symbolItems.Union(_keywordProvider.GetSymbols(token, semanticModel, position));

			return symbolItems.OrderBy(symbol => symbol.Name);
		}


		private INamespaceOrTypeSymbol GetContainingClass(int position, CommonSyntaxTree tree, ISemanticModel semanticModel) {
			var syntaxToken = tree.GetRoot().FindToken(position);
			var nodeHierarchy = syntaxToken.Parent.AncestorsAndSelf();
			foreach (var syntaxNode in nodeHierarchy) {
				var thisNode = syntaxNode;
				Console.WriteLine(thisNode.GetText() + ": " + thisNode.GetType());
				var symbolInfo = semanticModel.GetSymbolInfo(thisNode);
				if (symbolInfo.Symbol != null && symbolInfo.Symbol.Kind == CommonSymbolKind.Namespace) {
					return symbolInfo.Symbol as INamespaceOrTypeSymbol;
				}
				if (syntaxNode is Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax) {
					thisNode = (syntaxNode as Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax).Expression;
					Console.WriteLine("replaced with: " + thisNode.GetText() + ": " + thisNode.GetType());
				}
				if (syntaxNode is Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax) {
					thisNode = (syntaxNode as Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax).Expression;
					Console.WriteLine("replaced with: " + thisNode.GetText() + ": " + thisNode.GetType());
				}
				var typeInfo = semanticModel.GetTypeInfo(thisNode);
				Console.WriteLine("Type: " + typeInfo.Type);
				if (typeInfo.Type != null && typeInfo.Type.TypeKind != CommonTypeKind.Error) {
					return typeInfo.Type;
				}
				//not sure we need the next part
				var symbol = semanticModel.GetDeclaredSymbol(thisNode);
				if (symbol != null) {
					Console.WriteLine("Symbol: " + symbol);
					Console.WriteLine("Type: " + symbol.ContainingType);
					return symbol.ContainingType;
				}
			}
			return null;
		}

		private bool IsDotCompletion(int position, CommonSyntaxTree tree) {
			var syntaxToken = tree.GetRoot().FindToken(position);
			return syntaxToken.Parent is Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax || syntaxToken.Parent is Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax;
		}

	}
}