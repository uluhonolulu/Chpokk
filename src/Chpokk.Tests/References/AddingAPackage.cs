using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using Mono.Collections.Generic;
using NuGet;
using NuGet.Commands;
using NuGet.Common;
using Shouldly;
using Console = System.Console;

namespace Chpokk.Tests.References {
	[TestFixture]
	public class AddingAPackage : BaseCommandTest<ProjectFileContext> {
		[Test]
		public void CreatesThePackageFolder() {
			var targetFolder = TargetFolder;
			foreach (var directory in Directory.EnumerateDirectories(targetFolder)) {
				Console.WriteLine(directory);
			}
			Directory.EnumerateDirectories(targetFolder).ShouldContain(s => s.Contains("elmah"));
		}

		[Test]
		public void MaybeConsoleCommandWillWork() {
			CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.StartsWith("NuGet")));
			CThruEngine.StartListening();
			Program.Main(new[] { "install", "elmah", "-o", @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\name1" });
		}

		[Test]
		public void AddsPackageContent() {
			
		}

		[Test]
		public void AddsAssemblyReference() {
			
		}

		public override void Act() {
			//CThruEngine.AddAspect(new DebugAspect(info => info.MethodName == "CreateAggregateRepositoryFromSources"));
			var initializer = Context.Container.Get<NuGetInitializer>();
			var command = initializer.CreateObject<InstallCommand>();
			command.OutputDirectory = TargetFolder;
			command.Source.Add(NuGetConstants.DefaultFeedUrl);
			command.Arguments.Add("elmah");
			command.ExecuteCommand();
		}

		private string TargetFolder {
			get { return Context.SolutionFolder.AppendPath("packages"); }
		}
	}
}
