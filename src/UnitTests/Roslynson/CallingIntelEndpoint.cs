using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arractas;
using Chpokk.Tests.Intellisense;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;

namespace UnitTests.Roslynson {
	public class CallingIntelEndpoint : BaseQueryTest<SolutionWithProjectAndClassFileContext, IEnumerable<IntelOutputModel.IntelModelItem>> {
		public override IEnumerable<IntelOutputModel.IntelModelItem> Act() {
			var endpoint = Context.Container.Get<IntellisenseEndpoint>();
			var source = @"public class X {public void Y(){(new A()).}}";
			var position = source.IndexOf('.');
			var model = new IntelInputModel()
			            {
			            	NewChar = '.',
			            	Position = position,
			            	Content = source,
			            	RepositoryName = Context.REPO_NAME,
							PathRelativeToRepositoryRoot = "x.cs",
							ProjectPath = Path.Combine("src", Context.PROJECT_PATH) // src\ProjectName\ProjectName.csproj
			            };
			return endpoint.GetIntellisenseData(model).Items;
		}

		[Test]
		public void ContainsTheMethodOfTheClass() {
			var memberNames = Result.Select(item => item.Name);
			try {
				Assert.Contains(memberNames, "B");
			}
			catch (Exception e) {
				Console.WriteLine("Members:");
				foreach (var memberName in memberNames) {
					Console.WriteLine(memberName);
				}
				throw;
			}
		}
	}

	//public struct CompletionOptions {
	//	public string Name { get; set; }
	//}
}
