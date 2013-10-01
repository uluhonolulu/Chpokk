using System;
using System.Collections.Generic;
using System.Text;
using Bottles.Diagnostics;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.Assets.Http;
using FubuMVC.Core.Runtime;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class FontProblem : WebQueryTest<SimpleConfiguredContext, WebResponse> {
		[Test]
		public void CanDownloadAFont() {
			//var log = Context.Container.Get<IPackageLog>();
			//Console.WriteLine(log.FullTraceText());
			//var graph = Context.Container.Get<AssetFileGraph>();
			//var file = graph.Find("fonts/glyphicons-halflings-regular.woff");
			//Console.WriteLine(file);
			Result.Status.ShouldBe(200);
		}

		public override WebResponse Act() {

			var session = new TestSession();
			session.AddAspect(new TraceAspect(info => info.TargetInstance is Exception, 10));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is AssetFileBuilder && info.MethodName == "CreateAssetFile"));
			//session.AddAspect(new TraceAspect(info => info.TypeName.Contains("Asset")));
			//session.AddAspect(new TraceAspect(info => info.TargetInstance is IAssetFileRegistration));
			session.Get("/");
			Console.WriteLine();
			Console.WriteLine("--------- Prewarming finished ------------------");
			Console.WriteLine();
			//session.AddAspect(new TraceResultAspect(info => info.TargetInstance is IAssetFileRegistration));
			session.Get("_content/styles/fonts/glyphicons-halflings-regular.ttf");
			session.AddAspect(new AssetTracer(info => info.TargetInstance is IAssetFileRegistration && info.MethodName == "Find"));
			session.AddAspect(new AssetTracer(info => info.TargetInstance is ContentWriter && info.MethodName == "Write"));
			Console.WriteLine("now...");
			var request = new WebRequest("_content/styles/fonts/glyphicons-halflings-regular.woff") {ThrowOnError = false};
			return session.ProcessRequest(request);
		}
	}

	class AssetTracer : CommonAspect {
		public AssetTracer(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}

		public override void MethodBehavior(DuringCallbackEventArgs e) {
			var path = e.ParameterValues[0];
			var assetPath = path as AssetPath;
			if (assetPath != null) {
				try {
					//var graph = e.TargetInstance as AssetFileGraph;
					//var all = graph.AllFiles().ToArray();
					Console.WriteLine(e.TypeName + "." + e.MethodName);
					Console.WriteLine(assetPath.Name + ": " + assetPath.Folder + ", " + MimeType.MimeTypeByFileName(assetPath.Name) + ", " + (assetPath.IsBinary()? "binary" : "text"));
				}
				catch (Exception exception) {
					Console.WriteLine(exception);
				}				//foreach (var assetFile in all) {
				//    //Console.WriteLine(assetFile.FullPath + " - " + assetFile.Folder);
				//}
				//Console.WriteLine(all);
			}
		}
	}

}
//FubuMVC.Core.Assets.Files.AssetFileBuilder.CreateAssetFile -- called


//FubuMVC.Core.Assets.Diagnostics.AssetLog..ctor(themes/aristo/images/progress_bar.gif)
//FubuMVC.Core.Assets.Diagnostics.AssetLog.set_Name(themes/aristo/images/progress_bar.gif)
//FubuMVC.Core.Assets.Diagnostics.AssetLog.set_Logs(System.Collections.Generic.List`1[FubuMVC.Core.Assets.Diagnostics.AssetLogEntry])
//FubuMVC.Core.Assets.Diagnostics.AssetLogsCache.get_Entries()
//FubuMVC.Core.Assets.Files.AssetPath.get_Package()
//FubuMVC.Core.Assets.Files.AssetFile.get_FullPath()
//FubuMVC.Core.Assets.Diagnostics.AssetLog.Add(application, Adding D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif to IAssetFileGraph)
//FubuMVC.Core.Assets.Diagnostics.AssetLog.get_Logs()
//FubuMVC.Core.Assets.Diagnostics.AssetLogEntry..ctor()
//FubuMVC.Core.Assets.Diagnostics.AssetLogEntry.set_Provenance(application)
//FubuMVC.Core.Assets.Diagnostics.AssetLogEntry.set_Message(Adding D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif to IAssetFileGraph)
//FubuMVC.Core.Assets.Files.AssetFileGraph.AddFile(FubuMVC.Core.Assets.Files.AssetPath, Asset: themes/aristo/images/progress_bar.gif at D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif)
//FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
//FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
//FubuMVC.Core.Assets.Files.AssetFile.set_Folder(styles)
//FubuMVC.Core.Assets.Files.AssetPath.get_Package()
//FubuMVC.Core.Assets.Files.PackageAssets.AddFile(FubuMVC.Core.Assets.Files.AssetPath, Asset: themes/aristo/images/progress_bar.gif at D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\progress_bar.gif)
//FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
//FubuMVC.Core.Assets.Files.AssetPath.get_Folder()
//FubuMVC.Core.Assets.Files.AssetFolder.GetHashCode()
//FubuMVC.Core.Assets.Files.AssetFolder.Equals(styles)
//FubuMVC.Core.Assets.Files.AssetFolder.GetHashCode()
//FubuMVC.Core.Assets.Files.AssetFolder.Equals(styles)
//FubuMVC.Core.Assets.Files.AssetFileBuilder.CreateAssetFile(D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\slider_handles.png)
//FubuMVC.Core.Assets.Files.PackageAssetDirectory.get_PackageName()
//FubuMVC.Core.Assets.Files.AssetPath..ctor(application, themes/aristo/images/slider_handles.png, styles)
//FubuMVC.Core.Assets.Files.AssetPath.set_Name(themes/aristo/images/slider_handles.png)
//FubuMVC.Core.Assets.Files.AssetPath.set_Package(application)
//FubuMVC.Core.Assets.Files.AssetPath.set_Folder(styles)
//FubuMVC.Core.Assets.Files.AssetFile..ctor(themes/aristo/images/slider_handles.png)
//FubuMVC.Core.Assets.Files.AssetFile.get_Name()
//FubuMVC.Core.Assets.Files.AssetFile.set_FullPath(D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\slider_handles.png)
//FubuMVC.Core.Assets.RecordingAssetFileRegistrator.AddFile(FubuMVC.Core.Assets.Files.AssetPath, Asset: themes/aristo/images/slider_handles.png at D:\Projects\Chpokk\src\ChpokkWeb\content\styles\themes\aristo\images\slider_handles.png)
//FubuMVC.Core.Assets.Files.AssetFile.get_Name()
//FubuMVC.Core.Assets.Diagnostics.AssetLogsCache.FindByName(themes/aristo/images/slider_handles.png)
