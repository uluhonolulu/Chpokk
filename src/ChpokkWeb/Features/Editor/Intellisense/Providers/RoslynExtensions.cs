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

		public static bool IsMember(this CommonSyntaxToken token) {
			return token.Parent.AncestorsAndSelf().OfType<Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax>().Any() ||
			       token.Parent.AncestorsAndSelf().OfType<Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax>().Any();
		}

		public static bool IsCSharpModel(this ISemanticModel semanticModel) {
			return semanticModel is Roslyn.Compilers.CSharp.SemanticModel;
		}
	}
}