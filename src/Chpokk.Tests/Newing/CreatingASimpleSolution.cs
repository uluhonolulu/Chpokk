using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using LibGit2Sharp.Tests.TestHelpers;
using Microsoft.Build.Evaluation;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.Newing {
	[TestFixture]
	public class CreatingASimpleSolution : BaseCommandTest<SimpleConfiguredContext> {
		public const string NAME = "Carramba";
		[Test]
		public void CreatesARepository() {
			RepositoryManager.RepositoryNameIsValid(NAME, Context.AppRoot).ShouldBe(true);
		}

		[Test]
		public void CreatesASolutionFile() {
			File.Exists(SolutionPath).ShouldBe(true);
		}

		[Test, DependsOn("CreatesASolutionFile")]
		public void TheSolutionFileHasAProjectOfTheSameName() {
			var parser = new SolutionParser();
			var projects = parser.GetProjectItems(File.ReadAllText(SolutionPath), SolutionPath);
			projects.Count().ShouldBe(1);
			projects.First().Name.ShouldBe(NAME);
		}

		[Test, DependsOn("TheSolutionFileHasAProjectOfTheSameName")]
		public void CreatesAProjectFileInASubfolder() {
			var projectPath = RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, NAME + ".csproj"));
			File.Exists(projectPath).ShouldBe(true);
		}

		public override void Act() {
			var name = NAME;
			var appRoot = Context.AppRoot;
			//create a repo
			Directory.CreateDirectory(RepositoryManager.GetAbsolutePathFor(name, appRoot));

			//create a solution
			var solutionPath = RepositoryManager.GetAbsolutePathFor(name, appRoot, name + ".sln");
			var solutionContent = @"Microsoft Visual Studio Solution File, Format Version 11.00
# Visual Studio 2010
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{0}\{0}.csproj"", ""{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}""
EndProject".ToFormat(name);
			var fileSystem = Context.Container.Get<IFileSystem>();
			fileSystem.WriteStringToFile(solutionPath, solutionContent);
			//{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC} -- C#
			//{F184B08F-C81C-45F6-A57F-5ABD9991F28F} -- VB.Net
			//{349C5851-65DF-11DA-9384-00065B846F21} -- Web app
			//{E24C65DC-7377-472B-9ABA-BC803B73C61A} -- Web site


			//create a project
			var projectPath = RepositoryManager.GetAbsolutePathFor(name, appRoot, Path.Combine(name, name + ".csproj"));
			var project = new Project();
			project.Save(projectPath);
		}

		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }

		private string SolutionPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, NAME + ".sln"); }
		}
	}
}
