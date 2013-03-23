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
			CThruEngine.AddAspect(new TraceAspect(info => info.MethodName.StartsWith("Load")));
			CThruEngine.StartListening();
			var registry = new ProjectContentRegistry();
			var assembly = Assembly.LoadWithPartialName("System.Core");
			Console.WriteLine(assembly.FullName);
			var projectContent = registry.GetProjectContentForReference(assembly.FullName, assembly.Location);
			//Assert.IsNotNull(projectContent);

			var other = GacInterop.FindBestMatchingAssemblyName("System.Core");
			Assert.IsNotNull(other);
		}
	}
}
