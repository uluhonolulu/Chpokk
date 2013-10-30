using ChpokkWeb.Features.Exploring;
using ICSharpCode.NRefactory;

namespace ChpokkWeb.Features.ProjectManagement.AddSimpleProject {
	public class AddSimpleProjectInputModel : BaseRepositoryInputModel {
		public string OutputType { get; set; }
		public SupportedLanguage Language { get; set; }
		public string[] References { get; set; }
	}
}