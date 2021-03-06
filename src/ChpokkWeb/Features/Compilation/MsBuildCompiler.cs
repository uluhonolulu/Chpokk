﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using Microsoft.Build.Framework;

namespace ChpokkWeb.Features.Compilation {
	public class MsBuildCompiler {
		private readonly ProjectParser _projectParser;

		public MsBuildCompiler(ProjectParser projectParser) {
			_projectParser = projectParser;
		}

		public BuildResult Compile(string projectFilePath, ILogger logger) {
			var project = _projectParser.LoadProject(projectFilePath);
			//var imports = project.Imports.Select(import => import.ImportedProject.FullPath).Where(path => path.StartsWith(_rootProvider.AppRoot));
			//Console.WriteLine("IMPORTS");
			//foreach (var import in imports) {
			//	Console.WriteLine(import + ": " + File.Exists(import));
			//}
			//var importPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\Microsoft.CSharp.targets";
			//project.CreateProjectInstance().Build("Build", new[] {logger});
			//Console.WriteLine(importPath + ": " + File.Exists(importPath));
			//Console.WriteLine("PROJECT");
			//var stringWriter = new StringWriter();
			//project.SaveLogicalProject(stringWriter);
			//Console.WriteLine(HttpUtility.HtmlEncode(stringWriter.ToString()));
			var buildResult = ProjectBuildSync.Build(project, logger);
			var outputPathProperty = project.AllEvaluatedProperties.First(property => property.Name == "OutputPath");
			var targetProperty = project.AllEvaluatedProperties.First(property => property.Name == "TargetFileName");
			var outputTypeProperty = project.AllEvaluatedProperties.First(property => property.Name == "OutputType");
			var outputFilePath = Path.Combine(projectFilePath.ParentDirectory(), outputPathProperty.EvaluatedValue,
				                                targetProperty.EvaluatedValue);
			var outputType = outputTypeProperty.EvaluatedValue;

			return new BuildResult{Success = buildResult, OutputFilePath = outputFilePath, OutputType = outputType};
		}


	}

	public class BuildResult {
		public bool Success { get; set; }
		public string OutputFilePath { get; set; }
		public string OutputType { get; set; }
	}
}