using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Shared;
using FubuMVC.Core;

namespace ChpokkWeb.Repa {
	public class RepositoryInputModel : IDontNeedActionsModel {
		public string Name { get; set; }
	}
}