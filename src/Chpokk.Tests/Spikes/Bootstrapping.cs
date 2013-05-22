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
			var container = new Container();
// ReSharper disable PossibleNullReferenceException
			FubuApplication.For<ConfigureFubuMVC>() 
				.StructureMap(container)
// ReSharper restore PossibleNullReferenceException
				.Bootstrap();
			//var engine = container.GetInstance<ILessEngine>();
			//Console.WriteLine(container.WhatDoIHave());
			
		}
	}
}
