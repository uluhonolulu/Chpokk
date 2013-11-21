﻿using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
//using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense {
	
	public class CompletionProvider {
		private readonly KeywordProvider _keywordProvider;
		public CompletionProvider(KeywordProvider keywordProvider) {
			_keywordProvider = keywordProvider;
		}

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
					syntaxTrees = new[] {tree};
					compilation = Roslyn.Compilers.VisualBasic.Compilation.Create("MyCompilation", syntaxTrees: syntaxTrees.Cast< Roslyn.Compilers.VisualBasic.SyntaxTree>()); break;
				default: throw new NotSupportedException("Language '" + language + "' is not supported.");
			}
			
			var semanticModel = compilation.GetSemanticModel(tree);
			var declaredSymbol = GetContainingClass(position, tree, semanticModel);
			var symbols = semanticModel.LookupSymbols(position, declaredSymbol).AsEnumerable();
			if (declaredSymbol != null && !this.IsDotCompletion(position, tree)) {
				var globals = semanticModel.LookupSymbols(position);
				symbols = symbols.Union(globals.AsEnumerable());
			}
			//tinky-winky

			var symbolItems = from s in symbols select new IntelOutputModel.IntelModelItem {Name = s.Name, EntityType = s.Kind.ToString()};
			if (!this.IsDotCompletion(position, tree)) {
				var keywords = language == LanguageNames.CSharp? _keywordProvider.CSharpKeywords : _keywordProvider.VBNetKeywords;
				var keywordItems = from keyword in keywords select new IntelOutputModel.IntelModelItem{Name = keyword, EntityType = "Keyword"};
				symbolItems = symbolItems.Union(keywordItems);
			}
			return symbolItems.OrderBy(symbol => symbol.Name);
		}

		private ITypeSymbol GetContainingClass(int position, CommonSyntaxTree tree, ISemanticModel semanticModel) {
			var syntaxToken = tree.GetRoot().FindToken(position);
			var nodeHierarchy = syntaxToken.Parent.AncestorsAndSelf();
			foreach (var syntaxNode in nodeHierarchy) {
				var thisNode = syntaxNode;
				Console.WriteLine(thisNode.GetText() + ": " + thisNode.GetType());
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