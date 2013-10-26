using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class CompletionProvider {
		public IEnumerable<ISymbol> GetSymbols(string source, int position, IEnumerable<string> otherSources, IEnumerable<string> assemblyPaths, string language) {
			CommonSyntaxTree tree;
			CommonCompilation compilation;
			IEnumerable<CommonSyntaxTree> syntaxTrees;
			var references = from assemblyPath in assemblyPaths select  new MetadataFileReference(assemblyPath);
			switch (language) {
				case LanguageNames.CSharp: 
					tree = SyntaxTree.ParseText(source);
					syntaxTrees = from code in otherSources select SyntaxTree.ParseText(code);
					syntaxTrees = syntaxTrees.Union(new[] {tree});
					compilation = Roslyn.Compilers.CSharp.Compilation.Create("MyCompilation", syntaxTrees: syntaxTrees.Cast<SyntaxTree>(), references: references);break;
				case LanguageNames.VisualBasic:
					tree = Roslyn.Compilers.VisualBasic.SyntaxTree.ParseText(source);
					syntaxTrees = new[] {tree};
					compilation = Roslyn.Compilers.VisualBasic.Compilation.Create("MyCompilation", syntaxTrees: syntaxTrees.Cast< Roslyn.Compilers.VisualBasic.SyntaxTree>(), references: references); break;
				default: throw new NotSupportedException("Language '" + language + "' is not supported.");
			}
			
			var semanticModel = compilation.GetSemanticModel(tree);
			var declaredSymbol = GetContainingClass(position, tree, semanticModel);
			var symbols = semanticModel.LookupSymbols(position, declaredSymbol);

			//Console.WriteLine();
			//Console.WriteLine("symbols:");
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
				//Console.WriteLine(thisNode.GetText() + ": " + thisNode.GetType());
				if (syntaxNode is MemberAccessExpressionSyntax) {
					thisNode = (syntaxNode as MemberAccessExpressionSyntax).Expression;
					//Console.WriteLine("replaced with: " + thisNode.GetText() + ": " + thisNode.GetType());
				}
				var typeInfo = semanticModel.GetTypeInfo(thisNode);
				//Console.WriteLine("Type: " + typeInfo.Type);
				if (typeInfo.Type != null) {
					return (INamedTypeSymbol) typeInfo.Type;
				}
				//not sure we need the next part
				var symbol = semanticModel.GetDeclaredSymbol(thisNode);
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