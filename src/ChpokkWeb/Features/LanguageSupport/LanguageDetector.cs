using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.LanguageSupport {
	public class LanguageDetector {
		public SupportedLanguage GetLanguage(string fileName) {
			var ext = Path.GetExtension(fileName);
			if (ext != null) {
				if (ext.Equals(".cs", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".csproj", StringComparison.InvariantCultureIgnoreCase)) {
					return SupportedLanguage.CSharp;
				}
				if (ext.Equals(".vb", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".vbproj", StringComparison.InvariantCultureIgnoreCase)) {
					return SupportedLanguage.VBNet;
				} 
			}
			throw new Exception("Language not detected for " + fileName);
		}

		public LanguageProperties GetLanguageProperties(string fileName) {
			var language = GetLanguage(fileName);
			return (language == SupportedLanguage.CSharp) ? LanguageProperties.CSharp : LanguageProperties.VBNet;
		}
	}
}