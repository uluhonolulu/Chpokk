using System.Collections.Generic;
using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Editor.Intellisense {
	public class IntelOutputModel {

		public IntelModelItem[] Items { get; set; }

		public string Message { get; set; }

		public class IntelModelItem {
			public string Text { get; set; }

			public EntityType EntityType { get; set; }

			public override bool Equals(object obj) {
				var other = obj as IntelModelItem;
				if (other != null)
					return other.Text == this.Text;
				return base.Equals(obj);
			}

			public override int GetHashCode() {
				return Text.GetHashCode();
			}
		}
	}
}