﻿using System;
using System.Linq;
using ChpokkWeb;
using ChpokkWeb.Features.Editor.Intellisense;
using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Urls;
using FubuMVC.StructureMap;
using Ivonna.Framework.Generic;
using Ivonna.Framework;
using MbUnit.Framework;
using StructureMap;

namespace Chpokk.Tests.Intellisense {
	[TestFixture, RunOnWeb]
	public class RequestForIntelData {

		[Test]
		public void ShouldReturnValidIntelForStrings() {
			// throws here: input.PhysicalApplicationPath, repositoryRoot, input.ProjectPath -- smth not defined
			// ChpokkWeb.Features.Editor.Intellisense.IntelController.GetIntellisenseData(IntelInputModel input) in D:\Projects\Chpokk\src\ChpokkWeb\Features\Editor\Intellisense\IntelController.cs:28
			var text = "using System;\r\nclass AClass\r\n{\r\n void B()\r\n {\r\n  string x;\r\n  x\r\n }\r\n}\r\n";
			var session = new TestSession();
			//TODO: until we implement single file scripts, it is absolutely necessary to provide a project file here;
			// maybe a hint that we need to isolate it somehow
			throw new NotImplementedException("Need a project file here, need refactoring");
			var inputModel = new IntelInputModel {Text = text, Position = 58, NewChar = '.'};
			var url = Registry.UrlFor<IntelInputModel>();
			var output = session.PostJson<IntelOutputModel>(url, inputModel, encodeRequest:true);
			var members = from item in output.Items select item.Name;
			Assert.Contains<string>(members.ToArray(), "ToString");
			var toStrings = from member in members where member == "ToString" select member;
			Assert.AreEqual(1, toStrings.Count());
		}

		IUrlRegistry Registry {
			get { return Container.Get<IUrlRegistry>(); }
		}

		IContainerFacility Container {
			get {
				var container = new Container();
				container.Configure(expr => expr.For<IUrlRegistry>().Use<UrlRegistry>());
				var runtime = FubuApplication.For<ConfigureFubuMVC>()
					.StructureMap(container)
					.Bootstrap()
					;
				return runtime.Facility;				
			}
		}

		//[Test]
		//public void EnumeratingMemberTypes() {
		//    var types = new[] {EntityType.Class, EntityType.Field, EntityType.Property, EntityType.Method, EntityType.Event};
		//    foreach (var entityType in types) {
		//        Console.WriteLine("entityTypes[{0}] = '{1}';".ToFormat((int)entityType, entityType));
		//    }
		//    //Console.WriteLine((int)(EntityType.Method) );
		//}

	}
}
