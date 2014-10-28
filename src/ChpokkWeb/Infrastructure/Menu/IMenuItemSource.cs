namespace ChpokkWeb.Infrastructure.Menu {
	public interface IMenuItemSource {
		MenuItem GetMenuItem(string repositoryRoot);
	}
}
