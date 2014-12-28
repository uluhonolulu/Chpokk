using System;
using System.Collections.Generic;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.ProjectManagement.References.NuGet;
using MbUnit.Framework;
using NuGet;
using System.Linq;
using Shouldly;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class SearchingForNugetPackages: BaseQueryTest<SimpleConfiguredContext, IEnumerable<IPackage> > {
		[Test]
		public void ShouldReturnSomeResults() {
			Result.Any().ShouldBe(true);
			Console.WriteLine(Result.Count());
			//foreach (var package in Result) {
			//	Console.WriteLine(package);
			//}
		}

		public override IEnumerable<IPackage> Act() {
			var packageFinder = Context.Container.Get<PackageFinder>();
			//Context.Container.Get<FileSystem>().WriteStringToFile(@"C:\log.txt", "");
			//CThruEngine.AddAspect(new GoodTracer(info => info.MethodName != "Finalize", @"C:\log.txt"));
			//CThruEngine.StartListening();
			return packageFinder.FindPackages("mvc");
		}
	}

	//class GoodTracer: TraceAspect {
	//	private DateTime _started = DateTime.Now;
	//	protected override string GetMessage(DuringCallbackEventArgs e) {
	//		var timeSpan = DateTime.Now - _started;
	//		return Convert.ToInt32(timeSpan.TotalMilliseconds).ToString().PadLeft(6) + " | " + Thread.CurrentThread.ManagedThreadId.ToString() + " | " + base.GetMessage(e);
	//	}

	//	public override bool ShouldIntercept(InterceptInfo info) {
	//		return base.ShouldIntercept(info) && Thread.CurrentThread.Name == "Simple Test Driver";
	//	}

	//	public GoodTracer(Predicate<InterceptInfo> shouldIntercept, TextWriter writer) : base(shouldIntercept, writer) {}
	//	public GoodTracer(Predicate<InterceptInfo> shouldIntercept, string logPath) : base(shouldIntercept, logPath) {}
	//	public GoodTracer(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}
	//	public GoodTracer(Predicate<InterceptInfo> shouldIntercept, int depth) : base(shouldIntercept, depth) {}
	//	public GoodTracer(Predicate<InterceptInfo> shouldIntercept, TextWriter writer, int depth) : base(shouldIntercept, writer, depth) {}
	//}
}
