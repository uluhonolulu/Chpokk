using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using ChpokkWeb.Features.Blog.Post;
using ChpokkWeb.Features.ReadMe;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core.Http;
using FubuMVC.Core.Urls;

namespace ChpokkWeb.Features.Blog.List {
	public class ListBlogPostsEndpoint {
		private readonly IAppRootProvider _appRootProvider;
		private readonly IFileSystem _fileSystem;
		private readonly BlogPostParser<BlogPostModel> _parser;
		private readonly ICurrentHttpRequest _httpRequest;
		private readonly IUrlRegistry _urlRegistry;
		public ListBlogPostsEndpoint(IAppRootProvider appRootProvider, IFileSystem fileSystem, BlogPostParser<BlogPostModel> parser, ICurrentHttpRequest httpRequest, IUrlRegistry urlRegistry) {
			_appRootProvider = appRootProvider;
			_fileSystem = fileSystem;
			_parser = parser;
			_httpRequest = httpRequest;
			_urlRegistry = urlRegistry;
		}

		public ListBlogPostsModel DoIt() {
			var postModels = GetAllPosts().OrderByDescending(model => model.Date);
			return new ListBlogPostsModel { Posts = postModels.ToArray() };
		}

		private IEnumerable<BlogPostModel> GetAllPosts() {
			var blogRoot = _appRootProvider.AppRoot.AppendPath("Blog");
			var postPaths = _fileSystem.FindFiles(blogRoot, FileSet.Shallow("*.md"));
			var postModels = from path in postPaths select _parser.LoadAndParse(path);
			return postModels;
		}

		public XmlDocument Rss() {
			var postModels = GetAllPosts();
			return GetFeed(postModels);
		}

		public XmlDocument GetFeed(IEnumerable<BlogPostModel> postModels) {
			var source = postModels.OrderByDescending(model => model.Date).Take(10);
			var feedItems = from model in source
			                select
				                new SyndicationItem(model.Title, SyndicationContent.CreateXhtmlContent(model.Content),
													GetUriForContentModel(model), model.Slug, model.Date)
					                {
						                Summary =
											new TextSyndicationContent(model.HtmlDescription, TextSyndicationContentKind.Html),
						                PublishDate = model.Date,
						                Content =
							                SyndicationContent.CreateHtmlContent(model.Content),
						                Categories = {new SyndicationCategory("CodeProject")}
					                };
			var updatedTime = feedItems.First().PublishDate;
			var feed = new SyndicationFeed
				{
					Title = new TextSyndicationContent("Bloggin bout Chpokk"),
					Description = new TextSyndicationContent("Real world problems solved while creating an online .Net IDE"),
					Items = feedItems,
					Links =
						{
							new SyndicationLink {Uri = GetUriForAction(endpoint => endpoint.DoIt()), MediaType = "text/html"},
							new SyndicationLink
								{
									Uri = GetUriForAction(endpoint => endpoint.Rss()),
									MediaType = "text/xml",
									RelationshipType = "self"
								}
						},
					LastUpdatedTime = updatedTime,
					Language = "en"
				};
			var doc = new XmlDocument() {PreserveWhitespace = true};
			using (var writer = doc.CreateNavigator().AppendChild()) {
				feed.SaveAsRss20(writer);
				//writer.Flush();
			}

			return doc;
		}

		private Uri GetUriForContentModel(BaseContentModel model) {
			var inputModel = new BlogPostInputModel {Slug = model.Slug};
			var currentUri = new Uri(_httpRequest.FullUrl());
			return new Uri(currentUri, _urlRegistry.UrlFor(inputModel));
		}

		private Uri GetUriForAction(Expression<Action<ListBlogPostsEndpoint>> action) {
			var currentUri = new Uri(_httpRequest.FullUrl());
			return new Uri(currentUri, _urlRegistry.UrlFor<ListBlogPostsEndpoint>(action));
		}
	}

	public class ListBlogPostsModel {
		public IEnumerable<BlogPostModel> Posts { get; set; } 
	}
}