using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace SmokeTests {
	[TestFixture]
	public class CanWriteToTempFolder {
		[Test]
		public void WithoutThrowingAnException() {
			var path = Path.GetTempFileName();
			Console.WriteLine(path);
			File.WriteAllText(path, string.Empty);
			Console.WriteLine("Wrote a file");
			File.Delete(path);
		}
	}
}
