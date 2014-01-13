using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public static class RoslynExtensions {
		public static bool IsDot(this CommonSyntaxToken token) {
			return token.ValueText == ".";
		}

		public static bool IsCSharpModel(this ISemanticModel semanticModel) {
			return semanticModel is Roslyn.Compilers.CSharp.SemanticModel;
		}
	}
}