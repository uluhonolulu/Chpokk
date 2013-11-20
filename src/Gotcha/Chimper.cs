using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha {
	public class Chimper {
		private string _apiKey = "c0ca38158b8a1c86d9f86421f6738c64-us2";
		private const string _urlRoot = "https://us2.api.mailchimp.com/2.0/";

		public void PrintListIDs() {
			var url = _urlRoot + "/lists/list";
			var data = new NameValueCollection() {{"apikey", _apiKey}};
			using (var wc = new WebClient()){
				wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				var response = wc.UploadValues(url, WebRequestMethods.Http.Post, data);
				var result = Encoding.ASCII.GetString(response);
			}

		}
	}
}
