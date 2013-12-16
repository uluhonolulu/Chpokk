using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ChpokkWeb.Infrastructure {
	public class LambdaTextWriter: TextWriter {
		public LambdaTextWriter(Action<char> writeAction) {
			WriteAction = writeAction;
		}

		public Action<char> WriteAction { get; private set; }
		public override void Write(char value) {
			WriteAction(value);
		}
		public override Encoding Encoding {
			get { return Encoding.UTF8; }
		}
	}
}