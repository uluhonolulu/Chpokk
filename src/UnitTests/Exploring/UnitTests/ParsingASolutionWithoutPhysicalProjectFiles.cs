﻿using System.Collections.Generic;
using Arractas;
using ChpokkWeb.Features.Exploring;
using MbUnit.Framework;
using System.Linq;
using UnitTests.Infrastructure;

namespace UnitTests.Exploring.UnitTests {
	[TestFixture]
	public class ParsingASolutionWithoutPhysicalProjectFiles : BaseQueryTest<SimpleConfiguredContext, IEnumerable<ProjectItem>> {
		[Test]
		public void CanSeeAProjectItem() {
			Assert.AreEqual(1, Result.Count());
		}

		public override IEnumerable<ProjectItem> Act() {
			var solutionFileContent =
				@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{1}"", ""{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}""
				EndProject";
			var parser = Context.Container.Get<SolutionParser>();
			return parser.ParseSolutionContent(solutionFileContent);
		}
	}
}
