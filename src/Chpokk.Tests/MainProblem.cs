using System;
using System.Web;
using CThru;
using Ivonna.Framework;
using NUnit.Framework;
using Ivonna.Framework.Generic;
using CThru.BuiltInAspects;
using StructureMap;
using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Parser;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Stylizers;

namespace Chpokk.Tests {
	[TestFixture ,RunOnWeb]
	public class MainProblem { 
		[Test]
		public void Test() { //trace GetFullPath calls
			//should call dotLess directly
			//CThru.CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Import") || info.MethodName.Contains("Import"), 3));
			//CThru.CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "Url", 3));
			//CThruEngine.AddAspect(new DebugAspect(info => info.TypeName.Contains("LessEngine")));
			//ObjectFactory.Inject<ILessEngine>(new LessEngine(new Parser(new PlainStylizer(), new Importer(new FileReader(new AspServerPathResolver()))){}));
			//CThruEngine.AddAspect(new TraceResultAspect(info => info.TargetInstance is IPathResolver && info.MethodName == "GetFullPath"));
			new TestSession().Get("_content/styles/lib/bootstrap.less");

		}
	}

	public class UnitTest {
		[Test]
		public void CheckThisOut() {
			CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "Url", 3));
			CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "Quoted", 3));
			CThruEngine.AddAspect(new DebugAspect(info => info.TargetInstance is INodeProvider && info.MethodName == "Import"));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("Import") || info.MethodName.Contains("Import"), 3));
			CThruEngine.StartListening();
			const string content = "@import \"reset.less\";\r\n";
			var engine = new LessEngine();
			engine.Parser.Importer.Paths.Add(@"F:\Projects\Fubu\Chpokk\src\ChpokkWeb\Content\styles\lib\");
			var result = engine.TransformToCss(content, null); //@"~/Content/styles/lib/bootstrap.less"
			Console.WriteLine(result);
		}
	}


}
