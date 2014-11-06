using Chpokk.Tests.Infrastructure;
using FubuCore;

namespace Chpokk.Tests.Blog {
	public class OneBlogPostContext : SimpleConfiguredContext {
		public string FILENAME = "filename.md";
		public override void Create() {
			base.Create();
			var blogContent = "header\n=";
			var filePath = Container.Get<TestAppRootProvider>().AppRoot.AppendPath(@"Blog").AppendPath(FILENAME);
			Container.Get<IFileSystem>().WriteStringToFile(filePath, blogContent);
		}
	}
}