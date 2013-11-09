using System;
using System.Collections.Generic;
using System.Text;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Diagnostics.Runtime.Tracing;
using Gallio.Framework;
using ICSharpCode.NRefactory;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class ArrayBinding {
		[Test]
		public void Test() {
			var session = new TestSession();
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is UnhandledFubuException, 10));
			session.AddAspect(new TraceAspect(info => info.TargetInstance is ObjectDef));
			var response = session.Post("projectmanagement/addsimpleproject", new AddSimpleProjectInputModel { References = new[] { "humm" }, RepositoryName = "Chpokk-SampleSol", Language = SupportedLanguage.CSharp, OutputType = "Exe"});
			Console.WriteLine(response.BodyAsString);
		}
	}
}
