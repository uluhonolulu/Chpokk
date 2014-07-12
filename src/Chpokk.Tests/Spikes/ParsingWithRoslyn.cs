using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Roslyn.Compilers.Common;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class ParsingWithRoslyn {
		[Test]
		public void Test() {
			var source = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
class Program {
	static void Main(string[] args) {
		Console.WriteLine(""Hi!"");
	}
//}
";
			CommonSyntaxTree tree;
			tree = Roslyn.Compilers.CSharp.SyntaxTree.ParseText(source);
			var diagnostics = tree.GetRoot().GetDiagnostics();
			foreach (var diagnostic in diagnostics) {
				Console.WriteLine(diagnostic.ToString());
				Console.WriteLine(diagnostic.Info);
				Console.WriteLine(diagnostic.Location.GetLineSpan(true));
			}
		}
	}
}
