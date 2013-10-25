using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Editor.Intellisense;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class CallingIntelEndpoint : BaseQueryTest<PhysicalCodeFileContext, IEnumerable<IntelOutputModel.IntelModelItem>> {
		public override IEnumerable<IntelOutputModel.IntelModelItem> Act() {
			var endpoint = Context.Container.Get<IntellisenseEndpoint>();
			return endpoint.GetIntellisenseData(new IntelInputModel()).Items;
		}
	}

	public struct CompletionOptions {
		public string Name { get; set; }
	}

	public class IntellisenseEndpoint {
		private CompletionProvider _completionProvider;
		public IntellisenseEndpoint(CompletionProvider completionProvider) {
			_completionProvider = completionProvider;
		}

		public IntelOutputModel GetIntellisenseData(IntelInputModel input) {
			_completionProvider.GetSymbols(input.Text, input.Position, null, null, null)
			return null;
		}
	}
}
