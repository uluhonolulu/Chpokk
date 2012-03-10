using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Chpokk.Tests.GitHub.Infrastructure {
	class Api {
		public static void CommitFile(string path, string content) {
//post a new tree object with that file path pointer replaced with your new blob SHA getting a tree SHA back
			var treesha = CreateTreeObject(path, content);

			//create a new commit object with the current commit SHA as the parent and the new tree SHA, getting a commit SHA back
			var headsha = GetHead();
			var commitsha = CreateCommit(treesha, headsha);

			//update the reference of your branch to point to the new commit SHA
			UpdateHead(commitsha);
		}

		public static void UpdateHead(string commitsha) {
			var jserializer = new JavaScriptSerializer();
			var body = jserializer.Serialize(new {sha = commitsha});
			var bytes = Encoding.ASCII.GetBytes(body);
			var request = CreateDataRequest(bytes, "refs/heads/master", "PATCH");
			request.GetResponse().Close();
		}

		public static string GetHead() {
			string headsha;
			var jserializer = new JavaScriptSerializer();
			var request = CreateRequest("refs/heads/master");
			using (var response = request.GetResponse() as HttpWebResponse) {
				// Get the response stream
				var reader = new StreamReader(response.GetResponseStream());
				var str = reader.ReadToEnd();
				//Console.WriteLine(str);
				var head = jserializer.Deserialize<Ref>(str);
				headsha = head.Object.Sha;
				//Console.WriteLine(sha);
			}
			return headsha;
		}

		public static string CreateCommit(string treesha, string parent) {
			string commitsha;
			var jserializer = new JavaScriptSerializer();
			var body = jserializer.Serialize(new {message = "test", tree = treesha, parents = new[] {parent}});
			var bytes = Encoding.ASCII.GetBytes(body);
			var request = CreatePostRequest(bytes, "commits");
			// Get response
			using (var response = request.GetResponse() as HttpWebResponse) {
				// Get the response stream
				var reader = new StreamReader(response.GetResponseStream());
				var str = reader.ReadToEnd();
				Console.WriteLine(str);
				var newCommit = jserializer.Deserialize<ShaObject>(str);
				commitsha = newCommit.Sha;
				//Console.WriteLine(sha);
			}
			return commitsha;
		}

		public static string CreateTreeObject(string path, string content) {
			string sha;
			var jserializer = new JavaScriptSerializer();
			var body = jserializer.Serialize(new {tree = new[] {new {path, mode = "100644", type = "blob", content}}});
			var bytes = Encoding.ASCII.GetBytes(body);
			var request = CreatePostRequest(bytes, "trees");
			// Get response
			using (var response = request.GetResponse() as HttpWebResponse) {
				// Get the response stream
				var reader = new StreamReader(response.GetResponseStream());
				var str = reader.ReadToEnd();
				//Console.WriteLine(str);
				var newTreeObject = jserializer.Deserialize<ShaObject>(str);
				sha = newTreeObject.Sha;
				//Console.WriteLine(sha);
			}
			return sha;
		}

		public static string CreateBlob(string fileContent) {
			string sha;
			var jserializer = new JavaScriptSerializer();
			var body = jserializer.Serialize(new {content = fileContent, encoding = "utf-8"});
			var bytes = Encoding.ASCII.GetBytes(body);

			// Create the web request
			var command = "blobs";
			var request = CreatePostRequest(bytes, command);

			// Get response
			using (var response = request.GetResponse() as HttpWebResponse) {
				// Get the response stream
				var reader = new StreamReader(response.GetResponseStream());
				var str = reader.ReadToEnd();
				//Console.WriteLine(str);
				var newBlob = jserializer.Deserialize<ShaObject>(str);
				sha = newBlob.Sha;
				//Console.WriteLine(sha);
			}
			return sha;
		}

		private static HttpWebRequest CreatePostRequest(byte[] bytes, string command) {
			return CreateDataRequest(bytes, command, WebRequestMethods.Http.Post);
		}

		private static HttpWebRequest CreateDataRequest(byte[] bytes, string command, string method) {
			var request = CreateRequest(command);
			request.Method = method;
			using (var requestStream = request.GetRequestStream()) requestStream.Write(bytes, 0, bytes.Length);
			return request;
		}

		private static HttpWebRequest CreateRequest(string command) {
			var request = WebRequest.Create("https://api.github.com/repos/uluhonolulu/Chpokk-Scratchpad/git/" + command) as HttpWebRequest;
			var authInfo = "uluhonolulu@gmail.com:xd11SvG23";
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			request.Headers["Authorization"] = "Basic " + authInfo;
			return request;
		}

		internal class ShaObject {
			public string Sha { get; set; } 
		}

		internal class Ref {
			public ShaObject Object { get; set; } 
		}
	}
}
