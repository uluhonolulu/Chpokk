using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement {
	public class ProjectTypeGuid {
		public static readonly ProjectTypeGuid CSharpWindows = new ProjectTypeGuid(){Guid = Guid.Parse("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")};
		public static readonly ProjectTypeGuid VbNetWindows = new ProjectTypeGuid() { Guid = Guid.Parse("{F184B08F-C81C-45F6-A57F-5ABD9991F28F}") };
		public static readonly ProjectTypeGuid WebApp = new ProjectTypeGuid() { Guid = Guid.Parse("{349C5851-65DF-11DA-9384-00065B846F21}") };
		public static readonly ProjectTypeGuid WebSite = new ProjectTypeGuid() { Guid = Guid.Parse("{E24C65DC-7377-472B-9ABA-BC803B73C61A}") };


		public Guid Guid { get; private set; }
	}
}