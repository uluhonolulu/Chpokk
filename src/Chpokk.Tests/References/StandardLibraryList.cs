using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuCore;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Shouldly;

namespace Chpokk.Tests.References {
	public class StandardLibraryList {

		[Test]
		public void CanRetrieveDisplaysBclAssemblies() {
			var assemblies = new BclAssembliesProvider().GetBclAssemblies();

			assemblies.ShouldContain("System.Data");
			//also use AdditionalExplicitAssemblyReferences to add references and maybe TargetFrameworkDirectory
		}
	}
}
