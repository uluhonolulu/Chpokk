using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChpokkWeb.Features.Exploring;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class FileItemToProjectItemConverter {
		public IEnumerable<RepositoryItem> Convert(IEnumerable<FileItem> files, string projectFolderRelativeToRepositoryRoot) {
			var folders = files.Select(item => item.Path.ParentDirectory());
			var folderItems =
				folders.Where(folder => folder.IsNotEmpty()).Select(folder => CreateFolderItem(folder, files, projectFolderRelativeToRepositoryRoot));
			var fileItems = files.Where(item => item.Path.ParentDirectory().IsEmpty()).Select(item => CreateFileItem(item, projectFolderRelativeToRepositoryRoot));
			return folderItems.Concat(fileItems);
		}

		private RepositoryItem CreateFileItem(FileItem item, string projectFolderRelativeToRepositoryRoot) {
			return new RepositoryItem {Name = item.Path, PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, item.Path), Type = "file"};
		}

		private RepositoryItem CreateFolderItem(string folder, IEnumerable<FileItem> allFiles, string projectFolderRelativeToRepositoryRoot) {
			var folderItem = new RepositoryItem {Name = folder, PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, folder), Type = "folder"};
			foreach (var fileItem in allFiles.Where(item => item.Path.StartsWith(folder))) {
				folderItem.Children.Add(CreateFileItem(fileItem, projectFolderRelativeToRepositoryRoot));
			}
			return folderItem;
		}
	}
}
