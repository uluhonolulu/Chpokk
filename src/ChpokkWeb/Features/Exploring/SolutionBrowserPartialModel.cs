﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionBrowserPartialModel : IDontNeedActionsModel {
		public string RepositoryName { get; set; }
		public MenuItem[] RetrieveActions { get; set; }
	}
}