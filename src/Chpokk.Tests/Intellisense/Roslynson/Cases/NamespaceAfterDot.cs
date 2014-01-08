using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class NamespaceAfterDot: BaseCompletionTest {
		public NamespaceAfterDot() : base("class ClassA{ void method{ System./**/ } }") {}

		[Test]
		public void DotCompletingNamespace() {
			GetSymbols().ShouldContain(item => item.Name == "whoops");
		}
	}
}
