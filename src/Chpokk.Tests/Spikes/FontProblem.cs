using System;
using System.Collections.Generic;
using System.Text;
using CThru.BuiltInAspects;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using Shouldly;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class FontProblem {
		[Test]
		public void CanDownloadAFont() {
			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TargetInstance is Exception, 10));
			session.AddAspect(new TraceAspect(info => info.TypeName.Contains("Asset")));
			var response = session.Get("_content/styles/fonts/glyphicons-halflings-regular.woff");
			response.Status.ShouldBe(200);
		}
	}
}
FubuMVC.Core.Assets.Diagnostics.AssetLog..ctor(themes/aristo/images/progress_bar.gif)
FubuMVC.Core.Assets.Diagnostics.AssetLog.set_Name(themes/aristo/images/progress_bar.gif)
FubuMVC.Core.Assets.Diagnostics.AssetLog.set_Logs(System.Collections.Generic.List`1[FubuMVC.Core.Assets.Diagnostics.AssetLogEntry])
FubuMVC.Core.Assets.Diagnostics.AssetLogsCache.get_Entries()
FubuMVC.Core.Assets.Files.AssetPath.get_Package()
FubuMVC.Core.Assets.Files.AssetFile.get_FullPath()
FubuMVC.Core.Assets.Diagnostics.AssetLog.Add(application, Adding D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif to IAssetFileGraph)
FubuMVC.Core.Assets.Diagnostics.AssetLog.get_Logs()
FubuMVC.Core.Assets.Diagnostics.AssetLogEntry..ctor()
FubuMVC.Core.Assets.Diagnostics.AssetLogEntry.set_Provenance(application)
FubuMVC.Core.Assets.Diagnostics.AssetLogEntry.set_Message(Adding D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif to IAssetFileGraph)
FubuMVC.Core.Assets.Files.AssetFileGraph.AddFile(FubuMVC.Core.Assets.Files.AssetPath, Asset: themes/aristo/images/progress_bar.gif at D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif)
FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
FubuMVC.Core.Assets.Files.AssetFile.set_Folder(styles)
FubuMVC.Core.Assets.Files.AssetPath.get_Package()
FubuMVC.Core.Assets.Files.PackageAssets.AddFile(FubuMVC.Core.Assets.Files.AssetPath, Asset: themes/aristo/images/progress_bar.gif at D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif)
FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
FubuMVC.Core.Assets.Files.AssetFolder.GetHashCode()
FubuMVC.Core.Assets.Files.AssetFolder.Equals(styles)
FubuMVC.Core.Assets.Files.AssetFolder.GetHashCode()
FubuMVC.Core.Assets.Files.AssetFolder.Equals(styles)
FubuMVC.Core.Assets.Files.AssetFileBuilder.CreateAssetFile(D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\slider_handles.png)
FubuMVC.Core.Assets.Files.PackageAssetDirectory.get_PackageName()
FubuMVC.Core.Assets.Files.AssetPath..ctor(application, themes/aristo/images/slider_handles.png, styles)
FubuMVC.Core.Assets.Files.AssetPath.set_Name(themes/aristo/images/slider_handles.png)
FubuMVC.Core.Assets.Files.AssetPath.set_Package(application)
FubuMVC.Core.Assets.Files.AssetPath.set_Folder(styles)
FubuMVC.Core.Assets.Files.AssetFile..ctor(themes/aristo/images/slider_handles.png)
FubuMVC.Core.Assets.Files.AssetFile.get_Name()
FubuMVC.Core.Assets.Files.AssetFile.set_FullPath(D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\slider_handles.png)
FubuMVC.Core.Assets.RecordingAssetFileRegistrator.AddFile(FubuMVC.Core.Assets.Files.AssetPath, Asset: themes/aristo/images/slider_handles.png at D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\slider_handles.png)
FubuMVC.Core.Assets.Files.AssetFile.get_Name()
FubuMVC.Core.Assets.Diagnostics.AssetLogsCache.FindByName(themes/aristo/images/slider_handles.png)
