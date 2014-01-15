using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Emkay.S3;
using FubuCore;

namespace ChpokkWeb.Features.Storage {
	public class RemoteFileSystem: IFileSystem {
		private readonly IS3Client _client;
		public RemoteFileSystem(IS3Client client) {
			_client = client;
		}

		public bool FileExists(string filename) {
			return false;
		}

		public void DeleteFile(string filename) {}
		public void MoveFile(string @from, string to) {}
		public void MoveDirectory(string @from, string to) {}
		public bool IsFile(string path) {
			return false;
		}

		public string GetFullPath(string path) {
			return null;
		}

		public long FileSizeOf(string path) {
			return 0;
		}

		public void Copy(string source, string destination) {}
		public void WriteStreamToFile(string filename, Stream stream) {}
		public void WriteStringToFile(string filename, string text) {}
		public void AppendStringToFile(string filename, string text) {}
		public string ReadStringFromFile(string filename) {
			return null;
		}

		public void WriteObjectToFile(string filename, object target) {}
		public T LoadFromFile<T>(string filename) where T : new() {
			return default(T);
		}

		public T LoadFromFileOrThrow<T>(string filename) where T : new() {
			return default(T);
		}

		public void CreateDirectory(string directory) {}
		public void DeleteDirectory(string directory) {}
		public void CleanDirectory(string directory) {}
		public bool DirectoryExists(string directory) {
			return false;
		}

		public void LaunchEditor(string filename) {}
		public IEnumerable<string> ChildDirectoriesFor(string directory) {
			yield break;
		}

		public IEnumerable<string> FindFiles(string directory, FileSet searchSpecification) {
			yield break;
		}

		public void ReadTextFile(string path, Action<string> reader) {}
		public void MoveFiles(string @from, string to) {}
		public string GetDirectory(string path) {
			return null;
		}

		public string GetFileName(string path) {
			return null;
		}

		public void AlterFlatFile(string path, Action<List<string>> alteration) {}
		public void Copy(string source, string destination, CopyBehavior behavior) {}
	}
}