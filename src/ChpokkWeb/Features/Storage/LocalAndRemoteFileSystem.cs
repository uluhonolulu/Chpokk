using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FubuCore;

namespace ChpokkWeb.Features.Storage {
	public class LocalAndRemoteFileSystem: IFileSystem {
		public void CreateDirectory(string path) {
			_localSystem.CreateDirectory(path);
		}

		public long FileSizeOf(string path) {
			return _localSystem.FileSizeOf(path);
		}

		public void Copy(string source, string destination) {
			_localSystem.Copy(source, destination);
			Task.Factory.StartNew(() => _remoteSystem.Copy(source, destination));
		}

		public void Copy(string source, string destination, CopyBehavior behavior) {
			_localSystem.Copy(source, destination, behavior);
			Task.Factory.StartNew(() => _remoteSystem.Copy(source, destination, behavior));
		}

		public bool IsFile(string path) {
			return _localSystem.IsFile(path);
		}

		public bool FileExists(string filename) {
			return _localSystem.FileExists(filename);
		}

		public void WriteStreamToFile(string filename, Stream stream) {
			_localSystem.WriteStreamToFile(filename, stream);
			Task.Factory.StartNew(() => _remoteSystem.UploadFile(filename, filename));
		}

		public void WriteStringToFile(string filename, string text) {
			_localSystem.WriteStringToFile(filename, text);
			Task.Factory.StartNew(() => _remoteSystem.UploadFile(filename, filename));
		}

		public void AppendStringToFile(string filename, string text) {
			_localSystem.AppendStringToFile(filename, text);
			_remoteSystem.AppendStringToFile(filename, text);
		}

		public string ReadStringFromFile(string filename) {
			return _localSystem.ReadStringFromFile(filename);
		}

		public string GetFileName(string path) {
			return _localSystem.GetFileName(path);
		}

		public void AlterFlatFile(string path, Action<List<string>> alteration) {
			_localSystem.AlterFlatFile(path, alteration);
		}

		public void DeleteDirectory(string directory) {
			_localSystem.DeleteDirectory(directory);
			_remoteSystem.DeleteDirectory(directory);
		}

		public void CleanDirectory(string directory) {
			_localSystem.CleanDirectory(directory);
			_remoteSystem.CleanDirectory(directory);
		}

		public bool DirectoryExists(string directory) {
			return _localSystem.DirectoryExists(directory);
		}

		public void WriteObjectToFile(string filename, object target) {
			_localSystem.WriteObjectToFile(filename, target);
			_remoteSystem.WriteObjectToFile(filename, target);
		}

		public T LoadFromFileOrThrow<T>(string filename) where T : new() {
			return _localSystem.LoadFromFileOrThrow<T>(filename);
		}

		public T LoadFromFile<T>(string filename) where T : new() {
			return _localSystem.LoadFromFile<T>(filename);
		}

		public void LaunchEditor(string filename) {
			_localSystem.LaunchEditor(filename);
		}

		public void DeleteFile(string filename) {
			_localSystem.DeleteFile(filename);
			_remoteSystem.DeleteFile(filename);
		}

		public void MoveFile(string @from, string to) {
			_localSystem.MoveFile(@from, to);
			_remoteSystem.MoveFile(@from, to);
		}

		public void MoveFiles(string @from, string to) {
			_localSystem.MoveFiles(@from, to);
			_remoteSystem.MoveFiles(@from, to);
		}

		public void MoveDirectory(string @from, string to) {
			_localSystem.MoveDirectory(@from, to);
			_remoteSystem.MoveDirectory(@from, to);
		}

		public IEnumerable<string> ChildDirectoriesFor(string directory) {
			return _localSystem.ChildDirectoriesFor(directory);
		}

		public IEnumerable<string> FindFiles(string directory, FileSet searchSpecification) {
			return _localSystem.FindFiles(directory, searchSpecification);
		}

		public void ReadTextFile(string path, Action<string> callback) {
			_localSystem.ReadTextFile(path, callback);
		}

		public string GetFullPath(string path) {
			return _localSystem.GetFullPath(path);
		}

		public string GetDirectory(string path) {
			return _localSystem.GetDirectory(path);
		}

		public void LaunchBrowser(string filename) {
			_localSystem.LaunchBrowser(filename);
		}

		private readonly FileSystem _localSystem;
		private RemoteFileSystem _remoteSystem;
		public LocalAndRemoteFileSystem(FileSystem localSystem, RemoteFileSystem remoteSystem) {
			_localSystem = localSystem;
			_remoteSystem = remoteSystem;
		}
	}
}