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
using dotless.Core;

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
			Console.WriteLine(container.WhatDoIHave());
			
		}
	}
}
