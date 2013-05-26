using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ICSharpCode.NRefactory;

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
	}
}