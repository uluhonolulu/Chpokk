using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.AspNet;
using FubuMVC.Core.Runtime;

namespace ChpokkWeb.Infrastructure {
	public class DomainRedirectionBehavior : WrappingBehavior {
		private const string OLD_HOST = "chpokk.apphb.com";
		private const string NEW_HOST = "chpokk.online";
		private readonly ICurrentHttpRequest _currentHttpRequest;
		private readonly IOutputWriter _outputWriter;
		public DomainRedirectionBehavior(ICurrentHttpRequest currentHttpRequest, IOutputWriter outputWriter) {
			_currentHttpRequest = currentHttpRequest;
			_outputWriter = outputWriter;
		}

		protected override void invoke(Action action) {
			var url = _currentHttpRequest.FullUrl();
			var uri = new Uri(url);
			_outputWriter.AppendHeader("X-Host", uri.Host);
			if (uri.Host == OLD_HOST) {
				_outputWriter.AppendHeader("X-Redirecting", true.ToString());
				uri = new UriBuilder(uri.Scheme, NEW_HOST, 80, uri.PathAndQuery).Uri;
				_outputWriter.WritePermanentRedirectTo(uri.ToString());
			}
			else {
				_outputWriter.AppendHeader("X-Redirecting", false.ToString());
				action();
			}
		}
	}

	public static class OutputWriterExtensions {
		public static void WriteHeader(this IOutputWriter outputWriter,
						string headerName, string headerValue) {
			HttpContext.Current.Response.AppendHeader(headerName, headerValue);
		}

		public static void WritePermanentRedirectTo(this IOutputWriter outputwriter, string url) {
			outputwriter.WriteHeader("Location", url);
			outputwriter.WriteResponseCode(HttpStatusCode.MovedPermanently);
		}
	}
}