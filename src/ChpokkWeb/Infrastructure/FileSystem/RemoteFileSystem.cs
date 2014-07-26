using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChpokkWeb.Infrastructure;
using Emkay.S3;
using FubuCore;

namespace ChpokkWeb.Features.Storage {
	public class RemoteFileSystem: IFileSystem {
		const string bucketName = "chpokk";
		private readonly IS3Client _client;
		private readonly IAppRootProvider _rootProvider;
		private FileSystem _localFileSystem;
		public RemoteFileSystem(IS3Client client, IAppRootProvider rootProvider, FileSystem localFileSystem) {
			_client = client;
			_rootProvider = rootProvider;
			_localFileSystem = localFileSystem;
		}

		public bool FileExists(string filename) {
			return _client.EnumerateChildren(bucketName, filename.ToRemoteFileName(AppRoot)).Any();
		}

		public void DeleteFile(string filename) {
			var trashPath = filename.Replace(AppRoot, Trash);
			MoveFile(filename, trashPath);
		}
		public void MoveFile(string @from, string to) {
			Copy(@from, to);
			_client.DeleteObject(bucketName, @from.ToRemoteFileName(AppRoot));

		}
		public void MoveDirectory(string @from, string to) {
			foreach (var sourcePath in Directory.EnumerateFiles(@from)) {
				var destinationPath = to.AppendPath(sourcePath.RemotePathRelativeTo(@from));
				MoveFile(sourcePath, destinationPath);
			}
		}

		public bool IsFile(string path) {
			return false;
		}

		public string GetFullPath(string path) {
			return null;
		}

		public long FileSizeOf(string path) {
			return 0;
		}

		public void Copy(string source, string destination) {
			//question: shall we upload the file if the remote doesn't exist? probably we should if we go multiple
			if (!FileExists(source)) {
				return;
			}
			var tempFilename = Path.GetTempFileName();
			var remoteSource = source.ToRemoteFileName(AppRoot);
			_client.DownloadFile(bucketName, remoteSource, tempFilename, 0);
			Task.Factory.StartNew(() =>
			{
				UploadFile(destination, tempFilename);
				File.Delete(tempFilename);
			});
		}
		public void WriteStreamToFile(string filename, Stream stream) {
			var tempFilename = Path.GetTempFileName();
			_localFileSystem.WriteStreamToFile(tempFilename, stream);
			Task.Factory.StartNew(() =>
			{
				UploadFile(filename, tempFilename);
				File.Delete(tempFilename);
			});
		}
		public void WriteStringToFile(string filename, string text) {
			var tempFilename = Path.GetTempFileName();
			_localFileSystem.WriteStringToFile(tempFilename, text);
			UploadFile(filename, tempFilename);
			File.Delete(tempFilename);

		}

		public void UploadFile(string filename, string sourcePath) {
			_client.PutFile(bucketName, filename.ToRemoteFileName(AppRoot), sourcePath, true, 0);
		}

		public void AppendStringToFile(string filename, string text) {
			throw new NotImplementedException("AppendStringToFile");
		}
		public string ReadStringFromFile(string filename) {
			return null;
		}

		public void WriteObjectToFile(string filename, object target) {
			throw new NotImplementedException("WriteObjectToFile");
		}
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
		public void MoveFiles(string @from, string to) {
			@from = @from.ToRemoteFileName(AppRoot);
			var children = _client.EnumerateChildren(bucketName, @from);
			foreach (var child in children) {
				var destinationFile = to.ToRemoteFileName(AppRoot) + child.RemotePathRelativeTo(@from);
				MoveFile(child, destinationFile);
				
			}
		}
		public string GetDirectory(string path) {
			return null;
		}

		public string GetFileName(string path) {
			return null;
		}

		public void AlterFlatFile(string path, Action<List<string>> alteration) {}
		public void Copy(string source, string destination, CopyBehavior behavior) {
			throw new NotImplementedException("Copy");
		}

		string AppRoot { get { return _rootProvider.AppRoot; } }
		string Trash { get { return AppRoot.AppendPath("Trash"); } }

	}

	static class RemoteFilenameExtensions {
		 public static string ToRemoteFileName(this string localFileName, string appRoot){
			 //if we already have a remote path, just return it
			 if (!Path.IsPathRooted(localFileName)) {
				 return localFileName;
			 }
			 return localFileName.PathRelativeTo(appRoot).Replace(Path.DirectorySeparatorChar, '/');
		}

		public static string RemotePathRelativeTo(this string path, string root) {
			if (path.StartsWith(root)) {
				return path.Substring(root.Length);
			}
			throw new ArgumentException("Total failure calculating a relative path", "path");
		}
	}
}