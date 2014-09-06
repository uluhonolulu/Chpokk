using System;
using NUnit.Framework;

namespace UnitTests.Infrastructure {
	public class Bitness {
		[Test]
		public void CheckBitness() {
			Console.WriteLine("Is64BitProcess " + Environment.Is64BitProcess);
			Console.WriteLine("=========================================================================================");
		}
	}
}
