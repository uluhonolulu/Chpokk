using Chpokk.Tests.Infrastructure;
using FubuCore;

namespace Chpokk.Tests.Blog {
	public class OneBlogPostContext : SimpleConfiguredContext {
		public string FILENAME = "filename.md";
		public override void Create() {
			base.Create();
			var blogContent = "header\n=";
			var filePath = BlogPostPath;
			Container.Get<FileSystem>().WriteStringToFile(filePath, blogContent);
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