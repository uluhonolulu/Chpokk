using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;

namespace ChpokkWeb.Infrastructure {
	public class DownloadDataConvention : Policy {
		public DownloadDataConvention() {
			Where.ResourceTypeImplements<DownloadDataModel>();
			Add.NodeToEnd<DownloadDataNode>();
		}
	}

	public class DownloadDataModel {
		public byte[] Data { get; set; }
		public string Filename { get; set; }
		public string MimeType { get; set; }
	}

	public class DownloadDataNode : Process {
		public DownloadDataNode() : base(typeof(DownloadDataBehavior)) { }
	}

	public class DownloadDataBehavior : BasicBehavior {
		private readonly IFubuRequest _request;
		private readonly IOutputWriter _writer;

		public DownloadDataBehavior(IOutputWriter writer, IFubuRequest request)
			: base(PartialBehavior.Ignored) {
			_writer = writer;
			_request = request;
		}

		protected override DoNext performInvoke() {
			var output = _request.Get<DownloadDataModel>();
			_writer.Write(output.MimeType, stream => stream.Write(output.Data, 0, output.Data.Length));
			_writer.AppendHeader("Content-Disposition", "attachment; filename=" + output.Filename + ";");

			return DoNext.Continue;
		}
	}
}