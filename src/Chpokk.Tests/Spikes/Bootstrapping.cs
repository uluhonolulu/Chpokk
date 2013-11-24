using System;
using System.Diagnostics;
using ChpokkWeb;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using MbUnit.Framework;
using StructureMap;

namespace Chpokk.Tests {
	[TestFixture]
	public class Bootstrapping {
		[Test]
		public void CanGetAnInstance() {
			var watch = new Stopwatch();
			watch.Start();
			Console.WriteLine("Container");
			var container = new Container();
			Console.WriteLine(watch.Elapsed);
			var facilityExpression = FubuApplication.For<ConfigureFubuMVC>();
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("StructureMap");
			var fubuApplication = facilityExpression.StructureMap(container);
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Bootstrap");
			fubuApplication.Bootstrap();
			Console.WriteLine(watch.Elapsed);

			watch.Restart();
			Console.WriteLine("GetNestedContainer");
			var child = container.GetNestedContainer();
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("StructureMap");
			fubuApplication = facilityExpression.StructureMap(child);
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Bootstrap");
			fubuApplication.Bootstrap();
			Console.WriteLine(watch.Elapsed);
			//var engine = container.GetInstance<ILessEngine>();
			//Console.WriteLine(container.WhatDoIHave());

		}
	}
}
