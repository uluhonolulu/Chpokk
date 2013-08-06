using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using FubuMVC.Core.Ajax;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.AddItem {
	public class AddItemEndpoint {
		public AjaxContinuation DoIt(AddItemInputModel model) {
			var projectFilePath = FileSystem.Combine(model.PhysicalApplicationPath,
			                              @"UserFiles\uluhonolulu\Chpokk-SampleSol\src\ConsoleApplication1\ConsoleApplication1.csproj");
			var fileName = string.Concat("NewClass", DateTime.Now.Millisecond.ToString(), ".cs");
			var project = new Project(projectFilePath);
			project.AddItem("Compile", fileName);
			project.Save();
			return AjaxContinuation.Successful();
		}
	}

	public class AddItemInputModel: BaseFileInputModel {}
}