using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectData {

		//takes projectpath as ctor parameter
		//responsible for 

		public IProjectContent ProjectContent {
			get { return DefaultProjectContent; }
		}

		private static DefaultProjectContent projectContent;
		private IProjectContent _projectContent;

		public static DefaultProjectContent DefaultProjectContent {
			get {
				if (projectContent == null) {
					var pcRegistry = new ProjectContentRegistry();

					projectContent = new DefaultProjectContent() {Language = LanguageProperties.CSharp};
					projectContent.AddReferencedContent(pcRegistry.Mscorlib);
					
				}
				return projectContent;
			}
		}

		public static void WarmUp() {
			var x = DefaultProjectContent;
		}
	}
}