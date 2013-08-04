using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes {
	public class RetrieveButtonsEndpoint {
		public RetrieveButtonsModel DoIt(RetrieveButtonsInputModel model) {
			var info = _manager.GetRepositoryInfo(model.RepositoryName);
			return new RetrieveButtonsModel() { RetrieveActions = _manager.GetRetrieveActions(info, model.PhysicalApplicationPath) };
		}

		[NotNull]
		private readonly RepositoryManager _manager;

		public RetrieveButtonsEndpoint(RepositoryManager manager) {
			_manager = manager;
		}
	}

}