using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SmokeTests {
	public class Bitness {
		[Test]
		public void CheckBitness() {
			Console.WriteLine(Environment.Is64BitProcess);
			Console.WriteLine("=========================================================================================");
		}
	}
}
