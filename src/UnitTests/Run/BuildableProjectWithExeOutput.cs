using UnitTests.Compilation;

namespace UnitTests.Run {
	public class BuildableProjectWithExeOutput: BuildableProjectWithSingleRootFileContext {
		public override string ProjectFileContent {
			get {
				return @"<?xml version=""1.0"" encoding=""utf-8""?>

				<Project ToolsVersion=""12.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"" InitialTargets="""">
				 <PropertyGroup>
					<Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
					<Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
					<ProductVersion>8.0.30703</ProductVersion>
					<SchemaVersion>2.0</SchemaVersion>
					<ProjectGuid>{6FEA811B-AABB-465F-932F-D0FB930AAAB5}</ProjectGuid>
					<OutputType>Exe</OutputType>
					<AppDesignerFolder>Properties</AppDesignerFolder>
					<AssemblyName>Program</AssemblyName>
					<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>
					<FileAlignment>512</FileAlignment>
				  </PropertyGroup>
				  <PropertyGroup>
					<DebugSymbols>true</DebugSymbols>
					<DebugType>full</DebugType>
					<Optimize>false</Optimize>
					<OutputPath>bin\Debug\</OutputPath>
					<DefineConstants>DEBUG;TRACE</DefineConstants>
					<ErrorReport>prompt</ErrorReport>
					<WarningLevel>4</WarningLevel>
				  </PropertyGroup>
				  <Import Project=""$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets"" />
 
				<ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				  <Import Project=""C:\Windows\Microsoft.NET\Framework\v4.0.30319\Microsoft.CSharp.targets"" />
				</Project>";
			}
		}

		protected override string FileContent {
			get {
				return "class program {static void Main(){System.Console.Write(\"message\");}}";
			}
		}
	}
}
