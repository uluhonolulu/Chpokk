using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using FubuCore;

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
			var other = project.AllEvaluatedProperties.First(projectProperty => projectProperty.Name == "MSBuildToolsPath");
			Console.WriteLine(other.EvaluatedValue);
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
				var builder = new StringBuilder();
				builder.AppendLine("we used: " + assemblyFolder);
				builder.AppendLine(exception.ToString());
				foreach (var projectProperty in project.AllEvaluatedProperties) {
					var value = projectProperty.EvaluatedValue;
					try {
						if (Path.IsPathRooted(value) && File.Exists(value.AppendPath("mscorlib.dll"))) {
							builder.AppendLine(projectProperty.Name + ": " + value);
						}
					}
					catch (Exception e) {
						Console.WriteLine(e);
					}
				}

				builder.AppendLine("what happened here:");
				foreach (var projectProperty in project.AllEvaluatedProperties.Where(p => p.Name == "FrameworkPathOverride")) {
					builder.AppendLine(projectProperty.Name + ": " + projectProperty.EvaluatedValue);
				}
				if (_mailer.Host != null) _mailer.Send("errors@chpokk.apphb.com", "uluhonolulu@gmail.com", "Assembly folder", builder.ToString());
				_assemblies = new string[]{};
			}
			
		}

		public IEnumerable<string> BclAssemblies {
			get { return _assemblies; }
		}
	}
}