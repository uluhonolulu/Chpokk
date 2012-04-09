using System.Linq;
using ChpokkWeb.Editor.Intellisense;
using Ivonna.Framework.Generic;
using Ivonna.Framework;
using MbUnit.Framework;

namespace Chpokk.Tests.Intellisense {
	[TestFixture, RunOnWeb]
	public class RequestForIntelData {

		[Test]
		public void ShouldReturnValidIntelForStrings() {
			var text = "using System;\r\nclass A\r\n{\r\n void B()\r\n {\r\n  string x;\r\n  x\r\n }\r\n}\r\n";
			var session = new TestSession();
			var inputModel = new IntelInputModel {Text = text, Position = 58, NewChar = '.'};
			var output = session.PostJson<IntelOutputModel>("editor/intellisense/getintellisensedata", inputModel, encodeRequest:true);
			var members = from item in output.Items select item.Text;
			Assert.Contains<string>(members.ToArray(), "ToString");
			var toStrings = from member in members where member == "ToString" select member;
			Assert.AreEqual(1, toStrings.Count());
		}

		//[Test]
		//public void EnumeratingMemberTypes() {
		//    var types = new[] {EntityType.Class, EntityType.Field, EntityType.Property, EntityType.Method, EntityType.Event};
		//    foreach (var entityType in types) {
		//        Console.WriteLine("entityTypes[{0}] = '{1}';".ToFormat((int)entityType, entityType));
		//    }
		//    //Console.WriteLine((int)(EntityType.Method) );
		//}

	}
}
