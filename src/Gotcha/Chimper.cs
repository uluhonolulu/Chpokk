using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gotcha {
	public class Chimper {
		private string _apiKey = "c0ca38158b8a1c86d9f86421f6738c64-us2";
		private const string _urlRoot = "https://us2.api.mailchimp.com/2.0/";

		public dynamic PrintListIDs() {
			var command = "/lists/list";
			var data = new {apikey =_apiKey};
			return ExecuteCommand(command, data);
		}

		public dynamic SubscribeUser(string email, string fullName) {
			var command = "/lists/subscribe";
			var data = new { apikey = _apiKey, id = "749dced33c", email = new { email = "uluhonolulu@gmail.com" }, merge_vars = new { FULLNAME = "Ulu Honolulu" }, double_optin = false, send_welcome = true, update_existing = true };
			return ExecuteCommand(command, data);			
		}

		private dynamic ExecuteCommand(string command, NameValueCollection data) {
			var url = _urlRoot + command;
			using (var wc = new WebClient()) {
				wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				var response = wc.UploadValues(url, WebRequestMethods.Http.Post, data);
				return JsonConvert.DeserializeObject<dynamic>(Encoding.ASCII.GetString(response));
			}
		}

		private dynamic ExecuteCommand(string command, object data) {
			var url = _urlRoot + command;
			var postedString = JsonConvert.SerializeObject(data);
			using (var wc = new WebClient()) {
				wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				var response = wc.UploadString(url, WebRequestMethods.Http.Post, postedString);
				return JsonConvert.DeserializeObject<dynamic>(response);
			}
		}
	}
}
