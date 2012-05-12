using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Assets.Content;
using FubuMVC.Core.Assets.Files;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Ivonna.Framework.Generic;
using StructureMap.Pipeline;
using dotless.Core.Input;

namespace Chpokk.Tests {
	[TestFixture, RunOnWeb]
	public class LessProblem {
		public LessProblem() {
			CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("AppDomain"), @"F:\tmp\" + AppDomain.CurrentDomain.FriendlyName + ".txt"));
			CThruEngine.StartListening();
		}

		[Test]
		public void CanUseTheLessConfig() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Asset")));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Exception")));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PackageLog && info.MethodName == "MarkFailure", 5));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is AssetPath));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is PackageAssets));
			//CThruEngine.AddAspect(new DebugAspect(info => info.MethodName == "matchingType"));
			Assert.DoesNotThrow(() => new TestSession().Get("/"));
		}


		[FixtureSetUp]
		public void CopyBottles() {
			//if (Directory.Exists(@"F:\Projects\Fubu\Chpokk\src\ChpokkWeb\fubu-content\fubumvc.less"))
			//    Directory.Delete(@"F:\Projects\Fubu\Chpokk\src\ChpokkWeb\fubu-content\fubumvc.less", true);
			//File.Copy(@"F:\Projects\Fubu\FubuMVC.AssetTransforms\build\fubumvc.less.zip", @"F:\Projects\Fubu\Chpokk\src\ChpokkWeb\fubu-content\fubumvc.less.zip", true);
			
		}

		[Test]
		public void CanHandleImport() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.StartsWith("FubuMVC.Less")));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Exception")));
			CThruEngine.AddAspect(new NewTraceAspect(info => info.TargetInstance is AssetPipeline)); 
			CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "DoesFileExist"));
			CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is IPathResolver, 5));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is ConstructorInstance && info.MethodName == "Get", 6));
			//CThruEngine.AddAspect(new AssemblyLocationAspect(info => info.TargetInstance is ITransformer));
			//CThruEngine.AddAspect(new AssemblyLocationAspect(info => info.MethodName == "Import"));
			//CThruEngine.AddAspect(new AssetPathAspect());
			string output = null;
			Assert.DoesNotThrow(() => output = new TestSession().Get("/_content/styles/lib/bootstrap.less").BodyAsString);
			Console.WriteLine(output);
			// IAssetPipeline to ctor
			// pipeline.Find(path).FullPath
		}

		public class AssetPathAspect : CommonAspect {
			public AssetPathAspect() : base(info => info.TargetInstance is ITransformer && info.MethodName == "Transform") { }

			public override void MethodBehavior(DuringCallbackEventArgs e) {
				var files = e.ParameterValues[1] as IEnumerable<AssetFile>;
				foreach (var assetFile in files) {
					Console.WriteLine(assetFile.ToString());
					Console.WriteLine(assetFile.ContentFolder());
				}
			}
		}

		public class NewTraceAspect : TraceAspect {
			public NewTraceAspect(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}

			protected override string GetParameters(DuringCallbackEventArgs e) {
				return GetParameters(e.ParameterValues);
			}

			private string GetParameters(IEnumerable parameterValues) {
				if (parameterValues == null) return string.Empty;
				List<string> list = new List<string>();
				foreach (var obj in parameterValues) {
					var str = GetStringRepresentation(obj);
					list.Add(str);
				}
				return string.Join(", ", list.ToArray());
			}

			protected virtual string GetStringRepresentation(object obj) {
				if (obj == null) return "null";
				if (obj is string) return obj as string;
				if (obj is IEnumerable) return GetParameters(obj as IEnumerable);
				if (obj is AssetFile)
					return (obj as AssetFile).ToString() + ", ContentFolder=" + (obj as AssetFile).ContentFolder();
				if (obj is AssetPath)
					return "AssetPath: " + (obj as AssetPath).Name;
				return obj.ToString();
			}
		}

		public class AssemblyLocationAspect : CommonAspect {
			public AssemblyLocationAspect(Predicate<InterceptInfo> shouldIntercept) : base(shouldIntercept) {}

			public override void MethodBehavior(DuringCallbackEventArgs e) {
				Console.WriteLine("Assembly: " + e.TargetInstance.GetType().Assembly.CodeBase);
			}
		}

	}
}
