using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Blog.Post;
using ChpokkWeb.Infrastructure;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using Shouldly;

namespace Chpokk.Tests.Blog.Parser {
	[TestFixture]
	public class CanParseFromFile : BaseQueryTest<OneBlogPostContext, BlogPostModel> {
		[Test]
		public void SlugShouldMatchFilenameWithoutExtension() {
			Result.Slug.ShouldBe(Path.GetFileNameWithoutExtension(Context.FILENAME));
		}

		public override BlogPostModel Act() {
			var filePath = Context.Container.Get<IAppRootProvider>().AppRoot.AppendPath("Blog", Context.FILENAME);
			return Context.Container.Get<BlogPostParser>().LoadAndParse(filePath);
		}
	}
}
