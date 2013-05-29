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
			if (ext.Equals(".cs",StringComparison.InvariantCultureIgnoreCase)) {
				return SupportedLanguage.CSharp;
			}
			if (ext.Equals(".vb", StringComparison.InvariantCultureIgnoreCase)) {
				return SupportedLanguage.VBNet;
			}
			throw new Exception("Language not detected for " + fileName);
		}

		public LanguageProperties GetLanguageProperties(string fileName) {
			var ext = Path.GetExtension(fileName);
			if (ext.Equals(".cs",StringComparison.InvariantCultureIgnoreCase)) {
				return LanguageProperties.CSharp;
			}
			if (ext.Equals(".vb", StringComparison.InvariantCultureIgnoreCase)) {
				return LanguageProperties.VBNet;
			}
			throw new Exception("Language not detected for " + fileName);			
		}
	}
}