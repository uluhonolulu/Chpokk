using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using Roslyn.Compilers.CSharp;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class NamespaceDot: BaseCompletionTest {
		public NamespaceDot() : base("class ClassA{ void method(){ System./**/ } }") {}

		[Test]
		public void CompletesNestedNamespace() {
			GetSymbols().ShouldContain(item => item.Name == "Collections");
		}

		[Test]
		public void CompletesClassName() {
			GetSymbols().ShouldContain(item => item.Name == "String");
		}
	}
}
