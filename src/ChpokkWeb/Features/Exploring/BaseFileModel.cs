using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Exploring {
	public class BaseFileModel {
		[NotNull]
		public string PhysicalApplicationPath { get; set; }
		[NotNull]
		public string RepositoryName { get; set; }
		[NotNull]
		public string PathRelativeToRepositoryRoot { get; set; }
	}
}