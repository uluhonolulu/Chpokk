using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Exploring;
using ICSharpCode.SharpDevelop.Dom;
using MbUnit.Framework;
using Microsoft.Build.Construction;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class LoadingProjectData : BaseQueryTest<PhysicalCodeFileContext, IntelData> {
		private const string CODE = "code";

		public override IntelData Act() {
			var loader = Context.Container.Get<IntelDataLoader>();
			return loader.CreateIntelData(Context.ProjectPath, Context.FilePath, CODE);
		}

		[Test]
		public void LoadsThisPath() {
			Result.CodeFilePath.ShouldBe(Context.FilePath);
		}

		[Test]
		public void LoadsCurrentCode() {
			Result.Code.ShouldBe(CODE);
		}

		[Test]
		public void OtherSourcesShouldNotIncludeCurrentSource() {
			Result.OtherContent.ShouldNotContain(CODE);
		}

		[Test]
		public void ReferencePathsShouldIncludeMscorlib() {
			Result.ReferencePaths.ShouldContain(s => s.EndsWith("mscorlib.dll") && File.Exists(s));
		}
	}

	public class IntelData {
		public string CodeFilePath { get; set; }
		public string Code { get; set; }
		public IEnumerable<string> OtherContent { get; set; }
		public IEnumerable<string> ReferencePaths { get; set; }
	}

	public class IntelDataLoader {
		public IntelData CreateIntelData(string projectPath, string filePath, string content) {
			var projectRoot = ProjectRootElement.Open(projectPath);
			return new IntelData() { CodeFilePath = filePath, Code = content, OtherContent = GetSources(projectRoot, filePath), ReferencePaths = GetReferencePaths(projectRoot) };
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
			Console.WriteLine("Paths:");
			foreach (var path in paths) {
				Console.WriteLine(path);
			}
			paths = paths.Except(new[] {pathToExclude});
			return from path in paths
			       select File.ReadAllText(path);
		}
	}
}
