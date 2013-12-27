using System;
using System.Collections.Generic;
using System.Text;
using Fubu.Running;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class FubuRun {
		[Test]
		public void Test() {
			new RunCommand().Execute(new ApplicationRequest()
				{
					ApplicationFlag = "ChpokkApplication",
					DirectoryFlag = @"D:\Projects\Chpokk\src\ChpokkWeb"
				});
		}
	}
}
