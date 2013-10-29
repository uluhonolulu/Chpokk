using System;
using System.IO;
using System.Linq;
using FubuCore;
using ICSharpCode.NRefactory;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace Chpokk.Tests.References {
	public class StandardLibraryList {
		[Test]
		public void CanRetrieveDisplaysBclAssemblies() {
			var language = SupportedLanguage.CSharp;
			var rootElement = ProjectRootElement.Create();
			var targetImport = @"$(MSBuildToolsPath)\Microsoft.{0}.targets".ToFormat(language == SupportedLanguage.CSharp ? "CSharp" : "VisualBasic");
			rootElement.AddImport(targetImport);
			var project = new Project(rootElement);
			var property = project.AllEvaluatedProperties.First(projectProperty => projectProperty.Name == "FrameworkPathOverride");
			Console.WriteLine(property.EvaluatedValue);
			var assemblyFolder = property.EvaluatedValue;
			var assemblyPaths = Directory.EnumerateFiles(assemblyFolder, "*.dll");
			var assemblies = from path in assemblyPaths select Path.GetFileNameWithoutExtension(path);
			assemblies = assemblies.Except(new[] {"mscorlib", "sysglobl"}).OrderBy(s => s);
			foreach (var assembly in assemblies) {
				Console.WriteLine(assembly);
			}
			//also use AdditionalExplicitAssemblyReferences to add references and maybe TargetFrameworkDirectory
		}
	}
}
