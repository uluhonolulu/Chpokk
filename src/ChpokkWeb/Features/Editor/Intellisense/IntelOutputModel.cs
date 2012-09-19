using ICSharpCode.SharpDevelop.Dom;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class IntelOutputModel {
		public IntelModelItem[] Items { get; set; }

		public string Message { get; set; }

		public IntelOutputModel() {Items = new IntelModelItem[0];}

		public class IntelModelItem {
			public string Name { get; set; }

			public string EntityType { get; set; }

			public override bool Equals(object obj) {
				var other = obj as IntelModelItem;
				if (other != null)
					return other.Name == this.Name;
				return base.Equals(obj);
			}

			public override int GetHashCode() {
				return Name.GetHashCode();
			}
		}
	}
}