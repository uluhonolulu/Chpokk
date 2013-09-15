using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Compilation;
using FubuMVC.Core.Ajax;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;
using LibGit2Sharp.Tests.TestHelpers;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Framework;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class EmptyCode : BaseQueryTest<BuildableProjectWithSingleRootFileContext, AjaxContinuation> {
		[Test]
		public void ShouldBeSuccessfull() {
			Result.Success.ShouldBeTrue();
		}

		public override AjaxContinuation Act() {
			var endpoint = Context.Container.Get<CompilerEndpoint>();
			return endpoint.DoIt(new CompileInputModel(){PhysicalApplicationPath = Context.AppRoot, ProjectPath = Context.ProjectPath.PathRelativeTo(Context.RepositoryRoot), RepositoryName = Context.REPO_NAME});
		}
	}

	public class BuildableProjectWithSingleRootFileContext : PhysicalCodeFileContext {
		public override string ProjectFileContent {
			get {
				return @"<?xml version=""1.0"" encoding=""utf-8""?>

				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				 <PropertyGroup>
					<Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
					<Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
					<ProductVersion>8.0.30703</ProductVersion>
					<SchemaVersion>2.0</SchemaVersion>
					<ProjectGuid>{6FEA811B-AABB-465F-932F-D0FB930AAAB5}</ProjectGuid>
					<OutputType>Library</OutputType>
					<AppDesignerFolder>Properties</AppDesignerFolder>
					<RootNamespace>ClassLibrary1</RootNamespace>
					<AssemblyName>ClassLibrary1</AssemblyName>
					<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>
					<FileAlignment>512</FileAlignment>
				  </PropertyGroup>
				  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
					<DebugSymbols>true</DebugSymbols>
					<DebugType>full</DebugType>
					<Optimize>false</Optimize>
					<OutputPath>bin\Debug\</OutputPath>
					<DefineConstants>DEBUG;TRACE</DefineConstants>
					<ErrorReport>prompt</ErrorReport>
					<WarningLevel>4</WarningLevel>
				  </PropertyGroup>
 
				<ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
				</Project>";
			}
		}
	}
}
