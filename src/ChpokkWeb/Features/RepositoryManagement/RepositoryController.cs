using System.Collections.Generic;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Infrastructure;
using FubuMVC.Core;

namespace ChpokkWeb.Features.RepositoryManagement {
	public class RepositoryController {
		[UrlPattern("Repository/{Name}")]
		public RepositoryModel Get(RepositoryInputModel input) {
			return new RepositoryModel(){Name = input.Name};
		}
		[NotNull]
		private readonly RepositoryManager _manager;
		public RepositoryController([NotNull]RepositoryManager manager) {
			_manager = manager;
		}

		public RepositoryListModel GetRepositoryList([NotNull]RepositoryListInputModel model) {
			return new RepositoryListModel {RepositoryNames = _manager.GetRepositoryNames(model.PhysicalApplicationPath)};
		}
	}

	public class RepositoryListModel {
		public IEnumerable<string> RepositoryNames { get; set; }	
	}

	public class RepositoryListInputModel {
		public string PhysicalApplicationPath { get; set; }	
	}
}