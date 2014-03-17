using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace ChpokkWeb.Infrastructure {
	public class Chimper {
		private string _apiKey = "c0ca38158b8a1c86d9f86421f6738c64-us2";
		private const string _urlRoot = "https://us2.api.mailchimp.com/2.0/";
		private readonly ExceptionNotifier _exceptionNotifier;
		public Chimper(ExceptionNotifier exceptionNotifier) {
			_exceptionNotifier = exceptionNotifier;
		}


		public dynamic PrintListIDs() {
			var command = "/lists/list";
			var data = new {apikey =_apiKey};
			return ExecuteCommand(command, data);
		}

		public dynamic SubscribeUser(string email, string fullName) {
			var command = "/lists/subscribe";
			var data = new { apikey = _apiKey, id = "1eebc3e1c7", email = new { email }, merge_vars = new { FULLNAME = fullName }, double_optin = false, send_welcome = true, update_existing = true };
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
				try {
					var response = wc.UploadString(url, WebRequestMethods.Http.Post, postedString);
					return JsonConvert.DeserializeObject<dynamic>(response);
				}
				catch (WebException exception) {
					if (exception.Status == WebExceptionStatus.ProtocolError) {
						var response = exception.Response as HttpWebResponse;
						var responseStream = response.GetResponseStream() as MemoryStream;
						var responseBytes = responseStream.GetBuffer();
						var responseText = Encoding.Default.GetString(responseBytes, 0, (int) responseStream.Length);
						var message = response.StatusDescription;
						try {
							var responseObject = JsonConvert.DeserializeObject<dynamic>(responseText);
							if (responseObject.status == "error") {
								message = responseObject.name + ": " + responseObject.error;
							}
						}
						catch (Exception bindingException) {
							_exceptionNotifier.Notify(bindingException);
						}
						throw new ApplicationException(message);
						throw new HttpException((int) response.StatusCode, message, exception);
					}
					throw;
				}
			}
		}
	}
}
