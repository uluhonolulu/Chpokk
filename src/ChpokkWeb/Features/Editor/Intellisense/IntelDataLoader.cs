using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChpokkWeb.Infrastructure.FileSystem;
using ICSharpCode.SharpDevelop.Dom;
using Microsoft.Build.Construction;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntelDataLoader {
		private FileExistenceChecker _existenceChecker;
		public IntelDataLoader(FileExistenceChecker existenceChecker) {
			_existenceChecker = existenceChecker;
		}

		public IntelData CreateIntelData(string projectPath, string filePath, string content) {
			_existenceChecker.VerifyFileExists(projectPath);
			var projectRoot = ProjectRootElement.Open(projectPath);
			return new IntelData { CodeFilePath = filePath, Code = content, OtherContent = GetSources(projectRoot, filePath), ReferencePaths = GetReferencePaths(projectRoot) };
		}

		public IEnumerable<string> GetReferencePaths(ProjectRootElement root) {
			var referenceItems = from item in root.Items where item.ItemType == "Reference" select item;
			var assemblyLocations = from referenceItem in referenceItems select FindAssembly(referenceItem);
			return assemblyLocations.Where(s => s!=null).Union(new[] { FindAssemblyInNetGac("mscorlib") });
		}
		private string FindAssembly(ProjectItemElement referenceItem) {
			if (referenceItem.Metadata.Any(element => element.Name == "HintPath")) {
				return FindLocalAssembly(referenceItem);
			}
			return FindAssemblyInNetGac(referenceItem.Include);
		}

		private string FindLocalAssembly(ProjectItemElement referenceItem) {
			var hintElement = referenceItem.Metadata.First(element => element.Name == "HintPath");
			return Path.GetFullPath(Path.Combine(referenceItem.ContainingProject.DirectoryPath, hintElement.Value)) ;
		}

		private string FindAssemblyInNetGac(string reference) {
			var assemblyName = GacInterop.FindBestMatchingAssemblyName(reference);
			if (assemblyName == null) {
				return null;
			}
			return GacInterop.FindAssemblyInNetGac(assemblyName);
		}

		public IEnumerable<string> GetSources(ProjectRootElement root, string pathToExclude) {
			var compileItems = from item in root.Items 
			                   where item.ItemType == "Compile" 
			                   select item;
			var paths = from item in compileItems select Path.Combine(root.DirectoryPath, item.Include);
			//Console.WriteLine("Paths:");
			foreach (var path in paths) {
				//Console.WriteLine(path);
			}
			paths = paths.Except(new[] {pathToExclude});
			return from path in paths
			       select File.ReadAllText(path);
		}
	}
}