using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;
using FubuMVC.Core.Assets.Content;
using FubuMVC.Core.Assets.Files;

namespace ChpokkWeb.Infrastructure {
	public class UrlTransformer : ITransformer {
		readonly IModelUrlResolver _urlResolver;

		public UrlTransformer(IModelUrlResolver urlResolver) {
			_urlResolver = urlResolver;
		}

		public string Transform(string contents, IEnumerable<AssetFile> files) {
			var regex = new Regex(@"url::(?<InputModel>[A-Za-z\.]+)");
			var matches = regex.Matches(contents);
			var replacements = findReplacements(matches).Distinct();
			var replacedContents = contents;
			replacements.Each(r => {
				var url = _urlResolver.GetUrlForInputModelName(r);

				var alteredUrl = @"url::{0}".ToFormat(r);
				replacedContents = replacedContents.Replace(alteredUrl, url);
			});

			if (replacedContents.Contains("url::"))
				throw new UrlTransformationException(contents, files);

			return replacedContents;
		}

		private IEnumerable<string> findReplacements(MatchCollection matchCollection) {
			return from Match match in matchCollection select match.Groups["InputModel"].Value;
		}
	}

}