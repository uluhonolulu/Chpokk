using Chpokk.Tests.Exploring;
using FubuCore;

namespace Chpokk.Tests.ProjectLoading {
	public class ProjectFileWithProjectReferenceContent : ProjectFileContext {
		private const string referencedProjectFileName = "ClassLibrary1.csproj";

		public override string ProjectFileContent {
			get {
				return @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
						<ProjectReference Include=""{0}"">
						  <Project>{{6fea811b-aabb-465f-932f-d0fb930aaab5}}</Project>
						  <Name>ClassLibrary1</Name>
						</ProjectReference>
					</ItemGroup>
				</Project>".ToFormat(referencedProjectFileName);
			}
		}
		public override void Create() {
			base.Create();
			var referencedProjectFilePath = FileSystem.Combine(this.ProjectPath.ParentDirectory(), referencedProjectFileName);
			new FileSystem().WriteStringToFile(referencedProjectFilePath, string.Empty);
		}
	}
}