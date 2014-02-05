using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpSvn;

namespace ChpokkWeb.Features.Remotes.SaveCommit {
	public interface ICommitter: IVersionControlDetectionPolicy {
		void Commit(string filePath, string commitMessage);
	}
}
