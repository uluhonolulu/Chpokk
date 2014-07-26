using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChpokkWeb.Infrastructure {
	public interface IAppRootProvider {
		string AppRoot { get; }
	}
}
