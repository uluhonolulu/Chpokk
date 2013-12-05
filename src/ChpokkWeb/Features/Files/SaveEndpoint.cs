using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Ajax;

namespace ChpokkWeb.Features.Files {
	public class SaveEndpoint {
		private readonly Savior _savior;
		public SaveEndpoint(Savior savior) {
			_savior = savior;
		}

		public AjaxContinuation DoIt(SaveFileInputModel model) {
			_savior.SaveFile(model);
			return AjaxContinuation.Successful();
		}
	}
}