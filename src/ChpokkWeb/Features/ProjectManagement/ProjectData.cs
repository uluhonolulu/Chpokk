using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Editor.Compilation;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectData {

		//takes projectpath as ctor parameter
		//responsible for 

		private string _projectFilePath;

		public IProjectContent ProjectContent { get; private set; }

		private static DefaultProjectContent projectContent;

		public ProjectData(IProjectContent projectContent) {
			ProjectContent = projectContent;
		}


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