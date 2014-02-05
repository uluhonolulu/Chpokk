using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChpokkWeb.Features.Remotes {
	public interface IVersionControlDetectionPolicy {
		bool Matches(string repositoryName);
	}
}
