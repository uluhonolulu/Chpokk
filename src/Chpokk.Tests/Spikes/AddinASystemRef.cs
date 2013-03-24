using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Gallio.Framework;
using ICSharpCode.SharpDevelop.Dom;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class AddinASystemRef {
		[Test]
		public void Test() {
			//TODO: Add Mono.Cecil

			
			var registry = new ProjectContentRegistry();
			var assemblyName = "System.Core";
			//registry.GetProjectContentForReference(assemblyName, assemblyName);
			//var assembly = Assembly.LoadWithPartialName(assemblyName);
			//Console.WriteLine(assembly.FullName);
			//Console.WriteLine();
			//var projectContent = registry.GetProjectContentForReference(assembly.FullName, assembly.Location);
			//Assert.IsNotNull(projectContent);

			//var other = GacInterop.FindBestMatchingAssemblyName(assemblyName);
			//Assert.IsNotNull(other);

			var reference = GacInterop.FindBestMatchingAssemblyName(assemblyName);
			var fileName = GacInterop.FindAssemblyInNetGac(reference);
			var pc = registry.GetProjectContentForReference(assemblyName, fileName);
			Assert.IsNotNull(pc);
		}
	}
}
