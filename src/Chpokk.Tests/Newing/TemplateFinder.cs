using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using FubuCore;
using Gallio.Framework;
using Ionic.Zip;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateProviders;
using Microsoft.Win32;
using System.Linq;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class TemplateFinder : BaseQueryTest<SimpleConfiguredContext, IList<ProjectTemplateData>> {
		readonly IDictionary<string, IntPtr> _handlers = new Dictionary<string, IntPtr>();
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);
		[Test]
		public void CanFindSimpleNamedProject() {
			
		}

		public override IList<ProjectTemplateData> Act() {
			//var zipFiles = Context.Container.Get<FileSystem>().FindFiles(@"D:\Projects\Chpokk\src\ChpokkWeb\SystemFiles\Templates\ProjectTemplates\", new FileSet() {Include = "*.zip", DeepSearch = true});
			//foreach (var zipFilePath in zipFiles) {
			//	using (ZipFile zip = ZipFile.Read(zipFilePath)) {
			//		foreach (var zipEntry in zip) {
			//			zipEntry.Extract(zipFilePath.ParentDirectory(), ExtractExistingFileAction.OverwriteSilently);  
			//		}
			//		Console.WriteLine("Processed " + zipFilePath);
			//	}
			//	File.Delete(zipFilePath);
			//}
			//ListManagedResources(@"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\Microsoft.VisualStudio.Web.Mvc.4.0.dll");
			//return null;
			var packages = new List<string>();
			var fileSystem = Context.Container.Get<FileSystem>();
			var templateFiles = fileSystem.FindFiles(@"D:\Projects\Chpokk\src\ChpokkWeb\SystemFiles\Templates\ProjectTemplates\", new FileSet() { Include = "*.vstemplate", DeepSearch = true });
			foreach (var templateFile in templateFiles) {
				Console.WriteLine();logk("");
				Console.WriteLine(templateFile); logk(templateFile);
				var xmlDocument = new XmlDocument();
				xmlDocument.Load(templateFile);
				var ns = new XmlNamespaceManager(xmlDocument.NameTable);
				ns.AddNamespace("d", "http://schemas.microsoft.com/developer/vstemplate/2005");
				var nameNode = xmlDocument.SelectSingleNode("//d:TemplateData/d:Name", ns);
				if (nameNode.FirstChild != null) {
					Console.WriteLine("Explicit name: " + nameNode.FirstChild.Value);
				}
				else {
					string templateName = null;
					var packageIdNode = xmlDocument.DocumentElement.SelectSingleNode("//d:TemplateData/d:Name/@Package", ns);
					if (packageIdNode != null) {
						var packageId = packageIdNode.Value;
						var id = Int32.Parse(xmlDocument.DocumentElement.SelectSingleNode("//d:TemplateData/d:Name/@ID", ns).Value);
						var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\12.0_Config\Packages\" + packageId + @"\SatelliteDll");
						//for managed, find kinda HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\12.0_Config\Packages\{141DA93E-88A7-45D5-B15B-89117F514D64}\Assembly
						if (registryKey != null) {
							var path = Path.Combine(((string) registryKey.GetValue("Path")), "1033", ((string) registryKey.GetValue("DllName")));
							Console.WriteLine(path);logk(path);
							if (IsManaged(path))
								templateName = ListManagedResource(path, id);
							else
								try {
									templateName = ListUnmanagedResource(path, id);
								}
								catch (Exception e) {
									Console.WriteLine(e); logk(e.ToString());
								}
						}
						else {
							registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\12.0_Config\Packages\" + packageId);
							var assemblyName = ((string) registryKey.GetValue("Assembly"));
							Console.WriteLine(assemblyName ?? "no assembly name");
							if (assemblyName != null) {
								var assemblyFileName = new AssemblyName(assemblyName).Name + ".dll";
								var assemblyPath = fileSystem.FindFiles(@"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\", new FileSet() {Include = assemblyFileName}).FirstOrDefault();
								Console.WriteLine(assemblyPath ?? "no assembly path");
								if (assemblyPath != null) {
									templateName = ListManagedResource(assemblyPath, id);
								}
							}
							else {
								var assemblyPath = ((string) registryKey.GetValue("Codebase"));
								Console.WriteLine(assemblyPath ?? "no assembly path");
								if (assemblyPath != null) {
									templateName = ListManagedResource(assemblyPath, id);
								}
							}

						}
						//if (!packages.Contains(packageId)) {
						//	packages.Add(packageId);
						//}
					}
					if (templateName != null) {
						try {
							nameNode.InnerText = templateName;
						}
						catch (Exception e) {
							Console.WriteLine(e);
						}
						var newXml = nameNode.OuterXml;
					}
				}
				xmlDocument.Save(templateFile);
			}

			
			//foreach (var package in packages) {
			//	Console.WriteLine(package);
			//}

			var templates = new List<ProjectTemplateData>();
			templates.Add(new ProjectTemplateData(){Name = ""});
			return templates;
		}

		public void logk(string message) {
			Context.Container.Get<FileSystem>().AppendStringToFile(@"C:\logk.txt", message +  Environment.NewLine);
		}

		public string ListUnmanagedResource(string dllPath, int id) {
			//return;
			logk(id.ToString());
			var hMod = _handlers.ContainsKey(dllPath)? _handlers[dllPath] : LoadLibraryEx(dllPath, IntPtr.Zero, 0x00000002);
			_handlers[dllPath] = hMod;
			var sb = new StringBuilder(40960);
			int ln = LoadString(hMod, id, sb, 40960);
			if (ln > 0) {
				Console.WriteLine("- " + sb);
				logk(sb.ToString());
				return sb.ToString();
			}
			return null;
		}

		//public void ListUnmanagedResources(string dllPath) {
		//	//return;
		//	IntPtr hMod = LoadLibraryEx(dllPath, IntPtr.Zero, 0x00000002);
		//	for (int i = 0; i <= 9999; i++) {
		//		//Console.WriteLine(i);
		//		StringBuilder sb = new StringBuilder();
		//		int ln = LoadString(hMod, i, sb, 255);
		//		if (ln > 0) 
		//			Console.WriteLine(sb);
		//	}
		//}

		public string ListManagedResource(string dllPath, int id) {
			var assembly = Assembly.LoadFile(dllPath);
			foreach (var resourceName in assembly.GetManifestResourceNames()) {
				var set = new ResourceSet(assembly.GetManifestResourceStream(resourceName));
				if (set.Cast<DictionaryEntry>().Any(entry => entry.Key.ToString() == id.ToString())) {
					Console.WriteLine("+ " + set.GetString(id.ToString()));
					return set.GetString(id.ToString());
				}
				
			}
			Console.WriteLine("not found");
			return null;
		}

		private bool IsManaged(string dllPath) {
			try {
				logk("Am I managed?");
				if (_handlers.ContainsKey(dllPath)) {
					logk("I told you, no!");
					return false;
				}
				var _ = AssemblyName.GetAssemblyName(dllPath);
				logk("yes!");
				return true;
			}
			catch (BadImageFormatException) {
				logk("no!");
				return false;
			}
		}
	}

	public class ProjectTemplateData {
		public string Name { get; set; }
		public string FolderPath { get; set; }
		public string DisplayPath { get; set; }
	}
}
