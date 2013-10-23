using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class CompletionProvider {
		public IEnumerable<ISymbol> GetSymbols(string source, int position, LanguageNames language) {
			var tree = SyntaxTree.ParseText(source);
			switch (language) {
					
			}
			CommonCompilation compilation = Roslyn.Compilers.CSharp.Compilation.Create("MyCompilation", syntaxTrees: new[] { tree });
			var semanticModel = compilation.GetSemanticModel(tree);
			var declaredSymbol = GetContainingClass(position, tree, semanticModel);
			var symbols = semanticModel.LookupSymbols(position, declaredSymbol);

			Console.WriteLine();
			Console.WriteLine("symbols:");
			foreach (var symbol in symbols) {
				Console.WriteLine(symbol);
				//foreach (var propertyInfo in typeof(ISymbol).GetProperties()) {
				//	Console.WriteLine("\t" + propertyInfo.Name + ": " + propertyInfo.GetValue(symbol));
				//}
				//Console.WriteLine();
			}
			return symbols.AsEnumerable();
		}

		private INamedTypeSymbol GetContainingClass(int position, CommonSyntaxTree tree, ISemanticModel semanticModel) {
			var syntaxToken = tree.GetRoot().FindToken(position);
			var nodeHierarchy = syntaxToken.Parent.AncestorsAndSelf();
			foreach (var syntaxNode in nodeHierarchy) {
				var thisNode = syntaxNode;
				Console.WriteLine(thisNode.GetText() + ": " + thisNode.GetType());
				if (syntaxNode is MemberAccessExpressionSyntax) {
					thisNode = (syntaxNode as MemberAccessExpressionSyntax).Expression;
					Console.WriteLine("replaced with: " + thisNode.GetText() + ": " + thisNode.GetType());
				}
				var typeInfo = semanticModel.GetTypeInfo(thisNode);
				Console.WriteLine("Type: " + typeInfo.Type);
				if (typeInfo.Type != null) {
					return (INamedTypeSymbol) typeInfo.Type;
				}
				var symbol = semanticModel.GetDeclaredSymbol(thisNode);
				if (symbol != null) {
					Console.WriteLine("Symbol: " + symbol);
					Console.WriteLine("Type: " + symbol.ContainingType);
					return symbol.ContainingType;
				}
			}
			return null;
		}
	}
}