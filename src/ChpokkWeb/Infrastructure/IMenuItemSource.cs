using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChpokkWeb.Infrastructure {
	public interface IMenuItemSource {
		MenuItem GetMenuItem();
	}
}
