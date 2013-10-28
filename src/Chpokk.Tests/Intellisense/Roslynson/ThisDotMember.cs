﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class ThisDotMember {
		[Test]
		public void HasMemberNamesInIntellisenseData() {
			var source = "class ClassA { string field1; void method1(){this.} }";
			var position = source.IndexOf("this.") + "this.".Length - 1;
			var mscorlibPath = typeof(String).Assembly.Location;
			var symbols = new CompletionProvider().GetSymbols(source, position, new string[] { }, new string[] { mscorlibPath }, LanguageNames.CSharp);
			//symbols = GetSymbols(source, position);
			symbols.Any(symbol => symbol.Name == "field1").ShouldBe(true);
			symbols.Any(symbol => symbol.Name == "ClassA").ShouldBe(false);
		}


	}
}
