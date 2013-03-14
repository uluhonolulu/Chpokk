using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectData {
		private static DefaultProjectContent _projectContent;

		public static DefaultProjectContent DefaultProjectContent {
			get {
				if (_projectContent == null) {
					var pcRegistry = new ProjectContentRegistry();

					_projectContent = new DefaultProjectContent() {Language = LanguageProperties.CSharp};
					_projectContent.AddReferencedContent(pcRegistry.Mscorlib);
					
				}
				return _projectContent;
			}
		}

		public static void WarmUp() {
			var x = DefaultProjectContent;
		}
	}
}