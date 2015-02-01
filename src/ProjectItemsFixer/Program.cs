using System;
using System.IO;
using System.Xml;
using FubuCore;
using Microsoft.Build.Construction;
using System.Linq;

namespace ProjectItemsFixer {
	class Program {
		static void Main(string[] args) {
			var projectPath = @"D:\Projects\Chpokk\src\ChpokkWeb\ChpokkWeb.csproj";
			var project = ProjectRootElement.Open(projectPath);
			foreach (var item in project.Items) {
				if (item.Include.StartsWith(@"SystemFiles\Templates")) {
					var templatePath = projectPath.ParentDirectory().AppendPath(item.Include);
					if (!File.Exists(templatePath)) {
						Console.WriteLine(templatePath);
						item.Parent.RemoveChild(item);
					}
					else {
						item.ItemType = "Content";
					}
				}
			}
			project.Save();
		}
	}
}
