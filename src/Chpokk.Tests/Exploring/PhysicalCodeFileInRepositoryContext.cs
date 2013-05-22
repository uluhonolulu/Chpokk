using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGit2Sharp;

namespace Chpokk.Tests.Exploring {
	public class PhysicalCodeFileInRepositoryContext : PhysicalCodeFileContext {
		public override void Create() {
			base.Create();
			Repository.Init(RepositoryRoot).Dispose();
		}
	}
}
