using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure.Treeview;

namespace ChpokkWeb.Features.Repa {
	public class RepositoryItem : ICanHasChildren {
		public RepositoryItem() { Children = new List<RepositoryItem>(); }
		public string Name { get; set; }
		public string PathRelativeToRepositoryRoot { get; set; }
		IEnumerable<ICanHasChildren> ICanHasChildren.Children {
			get { return this.Children; }
		}

		public IList<RepositoryItem> Children { get; private set; }

		public string Type { get; set; }
	}
}