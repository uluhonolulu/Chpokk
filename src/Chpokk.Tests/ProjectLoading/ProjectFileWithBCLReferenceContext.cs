using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Exploring.UnitTests;

namespace Chpokk.Tests.ProjectLoading {
	public class ProjectFileWithBCLReferenceContext : ProjectFileContext {
		public override string ProjectFileContent {
			get {
				return ProjectContentWithOneBclReferenceContext.PROJECT_FILE_CONTENT;
			}
		}
	}
}
