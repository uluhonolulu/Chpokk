using System.Collections.Generic;
using ICSharpCode.NRefactory;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectInputModel {
		public string ProjectName { get; set; }
		public string OutputType { get; set; }
		public string TemplatePath { get; set; }
		public SupportedLanguage Language { get; set; }
		public IEnumerable<string> References { get; set; }
		public IEnumerable<string> Files { get; set; }
		public IEnumerable<string> Packages { get; set; }
		public string ConnectionId { get; set; }
	}
}