using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers;

namespace ChpokkWeb.Features.Editor.Parsing {
	public class ErrorInfo {
		public string Message { get; set; }
		public FileLinePositionSpan PositionSpan { get; set; }
	}
}
