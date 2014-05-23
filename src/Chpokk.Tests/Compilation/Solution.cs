using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class Solution {
		[Test]
		public void Test() {
			var projectFile =
				@"D:\Projects\Arractas\Arractas.sln";
			var loggers = new ILogger[]{};
			var targets = new string[] { "Build" };
			var globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var projectCollection = new ProjectCollection(globalProperties, loggers, null, ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry, 1, false);
			var requestData = new BuildRequestData(projectFile, globalProperties, null, targets, null);
			var parameters = new BuildParameters(projectCollection);
			parameters.ToolsetDefinitionLocations = ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry;
			BuildManager.DefaultBuildManager.Build(parameters, requestData);
		}
	}
}
