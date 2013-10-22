using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using Roslyn.Compilers.CSharp;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class ThisDotMember {
		[Test]
		public void HasMemberNamesInIntellisenseData() {
			var source = "class ClassA { string field1; void method1(){this.} }";
			var position = source.IndexOf("this.") + "this.".Length - 1;
			var symbols = new CompletionProvider().GetSymbols(source, position);
			//symbols = GetSymbols(source, position);
			symbols.Any(symbol => symbol.Name == "field1").ShouldBe(true);
			symbols.Any(symbol => symbol.Name == "ClassA").ShouldBe(false);
		}

		private static IEnumerable<Roslyn.Compilers.CSharp.Symbol> GetSymbols(string source, int position) {
			var tree = SyntaxTree.ParseCompilationUnit(source);
			var compilation = Roslyn.Compilers.CSharp.Compilation.Create("Compilation", syntaxTrees: new[] { tree });
			var semanticModel = compilation.GetSemanticModel(tree);

			var declaredSymbol = GetContainingClass(position, tree, semanticModel);
			//var containingSymbol = (NamespaceOrTypeSymbol)declaredSymbol.ContainingSymbol;
			//var identifier = tree.Root.FindToken(position-1).Parent;
			//var semanticInfo = semanticModel.GetSemanticInfo(identifier as ExpressionSyntax);
			//var type = semanticInfo.Type;
			var symbols = semanticModel.LookupSymbols(position, container: declaredSymbol, options: LookupOptions.IncludeExtensionMethods);

			Console.WriteLine();
			Console.WriteLine("symbols:");
			foreach (var symbol in symbols) Console.WriteLine(symbol);
			return symbols;
		}

		private static NamedTypeSymbol GetContainingClass(int position, SyntaxTree tree, SemanticModel semanticModel) {
			var syntaxToken = tree.Root.FindToken(position);
			Console.WriteLine("Token at position: " + syntaxToken);
			var parent = syntaxToken.Parent;
			var identifier = parent.ChildNodes().FirstOrDefault() as ExpressionSyntax;
			Console.WriteLine("Identifier: " + identifier + "(" + identifier.GetType() + ")");
			Console.WriteLine(semanticModel.GetDeclaredSymbol(identifier));
			while (parent != null) { //see Symbol.cs for a better syntax
				Console.WriteLine("Parent node: " + parent + "(" + parent.GetType() + ")");
				var symbol = semanticModel.GetDeclaredSymbol(parent);
				Console.WriteLine("Symbol: " +
				                  (symbol != null ? symbol.ToString() + " - " + symbol.Kind + ", " + symbol.ContainingType : "null"));
				if (parent is ExpressionSyntax)
					Console.WriteLine("Type info: " + semanticModel.GetSemanticInfo((ExpressionSyntax) parent).Type);

				Console.WriteLine();
				if (symbol != null && symbol.ContainingType != null) {
					return symbol.ContainingType;
				}
				parent = parent.Parent;
			}
			return null;
		}
	}
}
