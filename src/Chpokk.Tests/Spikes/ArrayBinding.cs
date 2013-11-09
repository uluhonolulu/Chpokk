using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using FubuCore.Binding;
using FubuCore.Binding.Values;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Diagnostics.Runtime.Tracing;
using FubuMVC.Validation;
using FubuValidation;
using FubuValidation.Fields;
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
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is IValidator));
			session.AddAspect(new TraceAspect(info => info.TargetInstance is IValueSource));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is ValidationStep));
			session.AddAspect(new TraceAspect(info => info.TargetInstance is ConvertProblem, 10));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is IValidationRule));
			//session.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("Notification")));
			//session.AddAspect(new TraceAspect(info => info.MethodName == "RegisterMessage", 10));
			session.AddAspect(new TraceAspect(info => info.MethodName == "ForProperty"));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is ObjectDef && info.MethodName == ".ctor", 10).DisplayFor<IEnumerable<object>>(objects => "[" + (from obj in objects select obj.ToString()).ToArray().Join(", ") + "]"));
			//session.AddAspect(new CheckProblem(info => info.TargetInstance is ObjectDef && info.MethodName == ".ctor"));
			var response = session.Post("projectmanagement/addsimpleproject", new AddSimpleProjectInputModel { References = new[] { "humm" }, RepositoryName = "Chpokk-SampleSol", Language = SupportedLanguage.CSharp, OutputType = "Exe"});
			Console.WriteLine(response.BodyAsString);
		}
	}

	class CheckProblem: CommonAspect {
		public CheckProblem(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}

		public override void MethodBehavior(DuringCallbackEventArgs e) {
			if (e.ParameterValues.Length == 2 && e.ParameterValues[1] is Type[]) {
				var types = e.ParameterValues[1] as Type[];
				if (types.Length > 0 && types[0] == typeof(int)) {
					if (Debugger.IsAttached) {
						Debugger.Break();
					}
				}
			}
		}
	}
}
