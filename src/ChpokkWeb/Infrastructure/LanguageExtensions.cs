using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;

namespace ChpokkWeb.Infrastructure {
	public static class LanguageExtensions {
		public static string GetProjectExtension(this SupportedLanguage language) {
			return language == SupportedLanguage.CSharp ? ".csproj" : ".vbproj";
		}


		public static string GetProjectGuid(this SupportedLanguage language) {
			return language == SupportedLanguage.CSharp ? ProjectTypeGuids.CSharp : ProjectTypeGuids.VBNet;
		}
	}
}