using System;
using System.Collections.Generic;
using System.Text;
using ChpokkWeb.Features.CustomerDevelopment;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class IAmImportant {
		[Test]
		public void Test() {
			var log = new List<TrackerInputModel>();
			//log.Add(new TrackerInputModel() { What = \"AJAX: http://kopchik.rpxnow.com/no_redirect?loc=4c4361f353c7cc05651e3f33860d65\", When = DateTime.Parse(\"23:48:02\") });
			log.Add(new TrackerInputModel() { What = "Creating a project: {\"ConnectionId\":\"8738628f-234b-41f6-8d86-a8a242a24861\",\"ProjectName\":\"\",\"OutputType\":\"Exe\",\"Language\":\"CSharp\",\"References\":[\"System\",\"System.Core\"],\"SolutionPath\":\"\",\"TemplatePath\":null}", When = DateTime.Parse("23:48:08") });
			var message = new ProjectCreatedRule().GetMessage(log);
			Console.WriteLine(message);
		}
	}
}
