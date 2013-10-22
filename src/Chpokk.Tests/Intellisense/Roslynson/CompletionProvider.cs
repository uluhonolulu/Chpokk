using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class CompletionProvider {
		public IEnumerable<ISymbol> GetSymbols(string source, int position) {
			var tree = SyntaxTree.ParseCompilationUnit(source);
			ICompilation compilation = Roslyn.Compilers.CSharp.Compilation.Create("Compilation", syntaxTrees: new[] { tree });
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
			return symbols;
		}

		private INamedTypeSymbol GetContainingClass(int position, CommonSyntaxTree tree, ISemanticModel semanticModel) {
			var syntaxToken = tree.Root.FindToken(position);
			foreach (var syntaxNode in syntaxToken.Parent.AncestorsAndSelf()) {
				var symbol = semanticModel.GetDeclaredSymbol(syntaxNode);
				//Console.WriteLine(syntaxNode.Kind.ToString());
				if (symbol != null) {
					//Console.WriteLine("Symbol: " + symbol);
					//Console.WriteLine("Type: " + symbol.ContainingType);
					return symbol.ContainingType;
				}
			}
			return null;
		}
	}
}