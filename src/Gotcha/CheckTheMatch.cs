using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Gotcha {
	[TestFixture]
	public class CheckTheMatch {
		[Test]
		public void Test() {
			var gimapster = new Gimapster("uluhonolulu@gmail.com", "xd11SvG23");
			gimapster.SelectFolder("chpokk");
			var header = gimapster.GetBody(2);
			var regExp = new Regex(@"\* \d* FETCH .* \{(?<size>\d*)\}\r\n");
			var size = Int32.Parse(regExp.Matches(header)[0].Groups["size"].Value) ;
			var startPos = regExp.Matches(header)[0].Length;
			var value = header.Substring(startPos, size).Replace("=\r\n", "");
			Console.WriteLine(value);
		}
	}
}
