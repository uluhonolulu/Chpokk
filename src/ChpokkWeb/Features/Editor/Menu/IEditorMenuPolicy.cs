using System.Collections.Generic;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuMVC.Navigation;

namespace ChpokkWeb.Features.Editor.Menu {
	public interface IEditorMenuPolicy {
		bool Matches(string repositoryPath);
		IEnumerable<MenuItem> GetMenuItems(string repositoryPath);
	}
}
