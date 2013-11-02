﻿using System;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class ProjectFileContext : RepositoryFolderContext {
		public readonly string SOLUTION_FOLDER = "src";
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";
		public readonly string FILE_NAME = "Class1.cs";
		public string SolutionFolder { get; set; }
		public string ProjectPath { get; private set; }

		public override void Create() {
			base.Create();
			SolutionFolder = FileSystem.Combine(RepositoryRoot, SOLUTION_FOLDER);
			ProjectPath = FileSystem.Combine(SolutionFolder, PROJECT_PATH);
			Console.WriteLine("Writing to " + ProjectPath);
			Container.Get<IFileSystem>().WriteStringToFile(ProjectPath, ProjectFileContent);
		}

		public virtual string ProjectFileContent {
			get {
				return @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				</Project>";
			}
		}
	}
}