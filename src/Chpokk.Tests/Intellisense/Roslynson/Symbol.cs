using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using Roslyn.Compilers.CSharp;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class Symbol {
		[Test]
		public void HasLocalSymbolsInIntellisenseData() {
			var source = "class ClassA { string field1; void method1(){fie} }";
			var position = source.IndexOf("fie") + "fie".Length - 1;
			var symbols = GetSymbols(source, position);

			var result = from symbol in symbols where symbol.Name.StartsWith("fie") select symbol;

			result.Any(symbol => symbol.Name == "field1").ShouldBe(true);

		}

		private static IEnumerable<Roslyn.Compilers.CSharp.Symbol> GetSymbols(string source, int position) {
			var tree = SyntaxTree.ParseCompilationUnit(source);
			var compilation = Roslyn.Compilers.CSharp.Compilation.Create("Compilation", syntaxTrees: new[] { tree });
			var semanticModel = compilation.GetSemanticModel(tree);
			var declaredSymbol = GetContainingClass(position, tree, semanticModel);
			//var identifier = tree.Root.FindToken(position-1).Parent;
			//var semanticInfo = semanticModel.GetSemanticInfo(identifier as ExpressionSyntax);
			//var type = semanticInfo.Type;
			var symbols = semanticModel.LookupSymbols(position, declaredSymbol, options:LookupOptions.Default | LookupOptions.IncludeExtensionMethods);

			Console.WriteLine();
			Console.WriteLine("symbols:");
			foreach (var symbol in symbols) Console.WriteLine(symbol);
			return symbols;
		}

		private static NamedTypeSymbol GetContainingClass(int position, SyntaxTree tree, SemanticModel semanticModel) {
			var syntaxToken = tree.Root.FindToken(position);
			Console.WriteLine("Token at position: " + syntaxToken);
			var parent = syntaxToken.Parent;
			while (parent != null) { // can also use var nearestExpr = token.Parent.AncestorsAndSelf().OfType<ExpressionSyntax>().First();var type = semModel.GetTypeInfo(nearestExpr).Type;
				Console.WriteLine("Parent node: " + parent + "(" + parent.GetType() + ")");
				var symbol = semanticModel.GetDeclaredSymbol(parent);
				Console.WriteLine("Symbol: " +
								  (symbol != null ? symbol.ToString() + " - " + symbol.Kind + ", " + symbol.ContainingType : "null"));
				if (parent is ExpressionSyntax)
					Console.WriteLine("Type info: " + semanticModel.GetSemanticInfo((ExpressionSyntax)parent).Type);

				if (symbol != null && symbol.ContainingType != null) {
					return symbol.ContainingType;
				}
				Console.WriteLine();
				parent = parent.Parent;
			}
			return null;
		}
	}
}
