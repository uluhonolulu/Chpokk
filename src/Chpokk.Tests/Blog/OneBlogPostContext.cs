using Chpokk.Tests.Infrastructure;
using FubuCore;

namespace Chpokk.Tests.Blog {
	public class OneBlogPostContext : SimpleConfiguredContext {
		public string FILENAME = "filename.md";
		public string BLOGPOST_CONTENT = "blog post content";
		private const string MetadataBoundary = "\r\n==\r\n";
		public override void Create() {
			base.Create();
			var filePath = BlogPostPath;
			Container.Get<FileSystem>().WriteStringToFile(filePath, MetadataBoundary + BLOGPOST_CONTENT);
		}

		public override void Dispose() {
			Container.Get<FileSystem>().DeleteFile(this.BlogPostPath);
			base.Dispose();
		}

		public string BlogPostPath {
			get { return Container.Get<TestAppRootProvider>().AppRoot.AppendPath(@"Blog").AppendPath(FILENAME); }
		}
	}
}