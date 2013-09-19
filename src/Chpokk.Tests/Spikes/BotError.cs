using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using WebRequest = Ivonna.Framework.Generic.WebRequest;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb, Ignore]
	public class BotError {
		[Test]
		public void Test() {
			var session = new TestSession();
			var request = new WebRequest("/");
			request.Headers.Add(HttpRequestHeader.Accept, "*/*");
			request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
			request.Headers.Add(HttpRequestHeader.Connection, "close");
			request.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");
			request.Headers.Add(HttpRequestHeader.From, "googlebot(at)googlebot.com");
			request.Headers.Add(HttpRequestHeader.Host, "chpokk.apphb.com");
			var response = session.ProcessBaseRequest(request);
			Console.WriteLine(response.BodyAsString);
		}
	}
}
