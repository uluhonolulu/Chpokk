using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Editor.Menu {
	public class EditorMenuEndpoint {
		private readonly IEnumerable<IEditorMenuPolicy> _menuPolicies;
		private readonly RepositoryManager _repositoryManager;
		public EditorMenuEndpoint(IEnumerable<IEditorMenuPolicy> menuPolicies, RepositoryManager repositoryManager) {
			_menuPolicies = menuPolicies;
			_repositoryManager = repositoryManager;
		}

		public EditorMenuModel DoIt(EditorMenuInputModel model) {
			var repositoryPath = _repositoryManager.NewGetAbsolutePathFor(model.RepositoryName);
			var matchingPolicies = _menuPolicies.Where(policy => policy.Matches(repositoryPath));
			var menuItemTokens = matchingPolicies.SelectMany(policy => policy.GetMenuItems());
			return new EditorMenuModel(){MenuItems = menuItemTokens};
		}
	}
}