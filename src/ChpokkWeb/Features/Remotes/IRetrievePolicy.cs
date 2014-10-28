using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Remotes {
	public interface IRetrievePolicy {
		bool Matches(string repositoryRoot);
	}
}
