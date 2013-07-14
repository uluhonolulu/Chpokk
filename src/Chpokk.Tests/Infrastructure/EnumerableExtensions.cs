using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;

namespace Chpokk.Tests.Infrastructure {
	public static class EnumerableExtensions {
		public static void ShouldContainItemOfType<TExpected>(this IEnumerable<object> source) {
			source.ShouldContain(item => item is TExpected);
		}
		public static void ShouldNotContainItemOfType<TExpected>(this IEnumerable<object> source) {
			source.ShouldNotContain(item => item is TExpected);
		}
	}
}
