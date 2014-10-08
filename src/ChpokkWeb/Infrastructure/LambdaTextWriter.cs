using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ChpokkWeb.Infrastructure {
	public class LambdaTextWriter: TextWriter {
		public LambdaTextWriter(Action<string> writeAction) {
			WriteAction = writeAction;
		}

		public Action<string> WriteAction { get; private set; }
		public override void Write(char value) {
			WriteAction(value.ToString(CultureInfo.InvariantCulture));
		}
		public override void Write(string value) {
			WriteAction(value);
		}
		public override void Write(char[] buffer, int index, int count) {
			var value = new string(buffer, index, count);
			WriteAction(value);
		}
		public override Encoding Encoding {
			get { return Encoding.UTF8; }
		}
	}
}