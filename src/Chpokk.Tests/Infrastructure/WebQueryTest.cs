using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arractas;
using Ivonna.Framework;

namespace Chpokk.Tests.Infrastructure {
	//[RunOnWeb]
	public abstract class WebQueryTest<TContext, TResult> : BaseQueryTest<TContext, TResult> where TContext : IContext, new() {
		[RunOnWeb]
		public override void Arrange() {
			base.Arrange();
		}
	}
}
