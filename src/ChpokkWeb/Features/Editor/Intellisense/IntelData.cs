using System.Collections.Generic;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class IntelData {
		public string CodeFilePath { get; set; }
		public string Code { get; set; }
		public IEnumerable<string> OtherContent { get; set; }
		public IEnumerable<string> ReferencePaths { get; set; }
	}
}