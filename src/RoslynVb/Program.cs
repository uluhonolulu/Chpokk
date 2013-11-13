using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynVb {
	class Program {
		static void Main(string[] args) {
			var source = "Public Module Module1 \r\n Public Sub X() \r\n End Sub \r\n End Module";
			var syntaxTree = Roslyn.Compilers.VisualBasic.SyntaxTree.ParseText(source);
			var compilation = Roslyn.Compilers.VisualBasic.Compilation.Create("MyCompilation", syntaxTrees: new[] { syntaxTree });
			var semanticModel = compilation.GetSemanticModel(syntaxTree);
		}
	}
}
