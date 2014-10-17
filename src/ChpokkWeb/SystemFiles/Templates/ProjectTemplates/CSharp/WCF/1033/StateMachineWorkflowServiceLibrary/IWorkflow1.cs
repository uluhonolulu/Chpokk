using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace $safeprojectname$
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWorkflow1" in both code and config file together.
	[ServiceContract]
	public interface IWorkflow1
	{

		[OperationContract]
		string GetData(int value);

		// TODO: Add your service operations here
	}
}
