﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace ChpokkWeb.Features.ProjectManagement.References.Bcl {
	public class BclAssembliesProvider {
		private readonly IEnumerable<string> _assemblies;
		private readonly SmtpClient _mailer;

		public BclAssembliesProvider(SmtpClient mailer) {
			_mailer = mailer;
			var rootElement = ProjectRootElement.Create();
			var targetImport = @"$(MSBuildToolsPath)\Microsoft.CSharp.targets";
			rootElement.AddImport(targetImport);
			var project = new Project(rootElement);
			var property = project.AllEvaluatedProperties.First(projectProperty => projectProperty.Name == "FrameworkPathOverride");
			Console.WriteLine(property.EvaluatedValue);
			var assemblyFolder = property.EvaluatedValue;
			try {
				var assemblyPaths = Directory.EnumerateFiles(assemblyFolder, "*.dll");
				var assemblies = from path in assemblyPaths select Path.GetFileNameWithoutExtension(path);
				_assemblies = assemblies.Except(new[] {"mscorlib", "sysglobl"}).OrderBy(s => s);
			}
			catch (ArgumentException exception) {
				var message = "Invalid path: " + assemblyFolder;
				var wrapper = new ApplicationException(message, exception);
				Elmah.ErrorSignal.FromCurrentContext().Raise(wrapper);
				if (_mailer.Host != null) _mailer.Send("features@chpokk.apphb.com", "uluhonolulu@gmail.com", "Assembly error", wrapper);
				_assemblies = new string[]{};
			}
			
		}

		public IEnumerable<string> BclAssemblies {
			get { return _assemblies; }
		}
	}
}