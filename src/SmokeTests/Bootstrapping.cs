using System;
using System.Collections.Generic;
using System.Text;
using ChpokkWeb;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using StructureMap;

namespace SmokeTests {
	[TestFixture]
	public class Bootstrapping {
		[Test]
		public void CanGetAnInstance() {
			var container = new Container();
			FubuApplication.For<ConfigureFubuMVC>()
				.StructureMap(container)
				.Bootstrap();
			//var engine = container.GetInstance<ILessEngine>();
			Console.WriteLine(container.WhatDoIHave());

		}
	}
}
