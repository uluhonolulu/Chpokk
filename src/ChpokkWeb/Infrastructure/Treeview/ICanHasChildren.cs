using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChpokkWeb.Infrastructure.Treeview {
	public interface ICanHasChildren {
		IEnumerable<ICanHasChildren> Children { get; } 
	}
}
