using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.References {
	public class BclAssembliesProvider {
		private readonly IEnumerable<string> _assemblies;

		public BclAssembliesProvider() {
			var rootElement = ProjectRootElement.Create();
			var targetImport = @"$(MSBuildToolsPath)\Microsoft.CSharp.targets";
			rootElement.AddImport(targetImport);
			var project = new Project(rootElement);
			var property = project.AllEvaluatedProperties.First(projectProperty => projectProperty.Name == "FrameworkPathOverride");
			Console.WriteLine(property.EvaluatedValue);
			var assemblyFolder = property.EvaluatedValue;
			var assemblyPaths = Directory.EnumerateFiles(assemblyFolder, "*.dll");
			var assemblies = from path in assemblyPaths select Path.GetFileNameWithoutExtension(path);
			_assemblies = assemblies.Except(new[] {"mscorlib", "sysglobl"}).OrderBy(s => s);
			
		}

		public IEnumerable<string> GetBclAssemblies() {
			return _assemblies;
		}
	}
}