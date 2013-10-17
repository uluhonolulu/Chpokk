﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.LanguageSupport;
using FubuCore;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;
using QuantConnect.CodingServices.Models;
using Roslyn.Compilers.CSharp;

//fail: 
//text: {public class X {public void Y(){new A().}}}
//+		expression	{<new A()>}	ICSharpCode.SharpDevelop.Dom.ExpressionResult
//+		compilationUnit	{[CompilationUnit: classes = 0, fileName = ]}	ICSharpCode.SharpDevelop.Dom.ICompilationUnit {ICSharpCode.SharpDevelop.Dom.DefaultCompilationUnit}

//win:
//+		expression	{<x> ([DefaultExpressionContext: IdentifierExpected])}	ICSharpCode.SharpDevelop.Dom.ExpressionResult
//+		compilationUnit	{[CompilationUnit: classes = 1, fileName = ]}	ICSharpCode.SharpDevelop.Dom.ICompilationUnit {ICSharpCode.SharpDevelop.Dom.DefaultCompilationUnit}
//        text	"using System;\r\nclass A\r\n{\r\n void B()\r\n {\r\n  string x;\r\n  x\r\n }\r\n}\r\n"	string
//+		resolveResult	{ICSharpCode.SharpDevelop.Dom.LocalResolveResult}	ICSharpCode.SharpDevelop.Dom.ResolveResult {ICSharpCode.SharpDevelop.Dom.LocalResolveResult}


//{public class X {public void Y(){}}}

namespace Chpokk.Tests.Intellisense {
	[TestFixture]
	public class IntellisenseList : BaseQueryTest<SolutionWithProjectAndClassFileContext, IntelOutputModel> {
		[Test]
		public void ContainsTheMethodOfTheClass() {
			Console.WriteLine(Result.Message);
			var items = Result.Items;
			var memberNames = items.Select(item => item.Name);
			Assert.Contains(memberNames, "B");
		}

		public override IntelOutputModel Act() {
			var controller = Context.Container.Get<IntelController>();
			var source = "public class X {public void Y(){new A().}}";
			var position = source.IndexOf('.');
			var model = new IntelInputModel()
			            {
			            	NewChar = '.',
			            	Position = position,
			            	Text = source,
			            	PhysicalApplicationPath = Context.AppRoot,
			            	RepositoryName = Context.REPO_NAME,
							PathRelativeToRepositoryRoot = "x.cs",
							ProjectPath = FileSystem.Combine("src", Context.PROJECT_PATH) // src\ProjectName\ProjectName.csproj
			            };

			var tree = SyntaxTree.ParseCompilationUnit(source);
			var compilation = Roslyn.Compilers.CSharp.Compilation.Create("Compilation", syntaxTrees: new[] { tree });
			var semanticModel = compilation.GetSemanticModel(tree);
			var symbols = semanticModel.LookupSymbols(position);

			Console.WriteLine("symbols:");
			foreach (var symbol in symbols) {
				Console.WriteLine(symbol);
			}

			return controller.GetIntellisenseData(model);
		}
	}
}
