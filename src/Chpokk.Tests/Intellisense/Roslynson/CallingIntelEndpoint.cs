using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Microsoft.Build.Evaluation;
using Roslyn.Compilers;

namespace Chpokk.Tests.Intellisense.Roslynson {
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
			//Trace.That(info => info.TargetInstance is CompletionProvider).DisplayFor<IEnumerable<string>>(enumerable => enumerable.Join(", "));
			//Trace.That(info => info.TargetInstance is IntelDataLoader);
			return endpoint.GetIntellisenseData(model).Items;
		}

		[Test]
		public void ContainsTheMethodOfTheClass() {
			var memberNames = Result.Select(item => item.Name);
			try {
				Assert.Contains(memberNames, "B");
			}
			catch (Exception e) {
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
