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
			var nodes = token.Parent.AncestorsAndSelf().Take(2); //take only to to avoid false positives coming from parent nodes
			return nodes.OfType<Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax>().Any() ||
			       nodes.OfType<Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax>().Any();
		}

		public static bool IsCSharpModel(this ISemanticModel semanticModel) {
			return semanticModel is Roslyn.Compilers.CSharp.SemanticModel;
		}
	}
}