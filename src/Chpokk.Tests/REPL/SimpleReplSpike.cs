using MbUnit.Framework;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using Shouldly;

namespace Chpokk.Tests.REPL {
	[TestFixture]
	public class SimpleReplSpike {
		[Test]
		public void ShouldDisplayTheResultOfASimpleOperation() {
			var source = "1 + 1";
			var result = Process(source);
			result.ShouldBe(2);
		}

		private object Process(string source) {
			var engine = new ScriptEngine();
			var session = engine.CreateSession();
			return session.Execute(source);
		}
	}
}
