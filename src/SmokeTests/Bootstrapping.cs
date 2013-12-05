using ChpokkWeb;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using NUnit.Framework;
using StructureMap;

namespace SmokeTests {
	[TestFixture]
	public class Bootstrapping {
		[Test, Ignore("Just can't make it work")]
		public void CanGetAnInstance() {
			var container = new Container();
			FubuApplication.For<ConfigureFubuMVC>()
				.StructureMap(container)
				.Bootstrap();
			////var engine = container.GetInstance<ILessEngine>();
			//Console.WriteLine(container.WhatDoIHave());

		}
	}
}
