using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Editor.Menu {
	public class EditorMenuInputModel { 
		[NotNull]
		public string RepositoryName { get; set; }
		[NotNull]
		public string PathRelativeToRepositoryRoot { get; set; }
	}
}