using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.Remotes.SVN {
	public class SvnDetectionPolicy: IVersionControlDetectionPolicy {
		public bool Matches(string repositoryPath) {
			var path = repositoryPath.AppendPath(".svn");
			return Directory.Exists(path);
		}
	}
}