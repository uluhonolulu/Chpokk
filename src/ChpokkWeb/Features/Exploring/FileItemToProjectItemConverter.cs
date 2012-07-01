using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChpokkWeb.Features.Exploring;
using FubuCore;
using ChpokkWeb.Infrastructure;

namespace Chpokk.Tests.Exploring {
	public class FileItemToProjectItemConverter {
		public IEnumerable<RepositoryItem> Convert(IEnumerable<FileItem> files, string projectFolderRelativeToRepositoryRoot) {
			var filePaths = files.Select(item => item.Path);
			return GetChildItems(string.Empty, filePaths, projectFolderRelativeToRepositoryRoot);
		}

		private RepositoryItem CreateFileItem(string filePath, string projectFolderRelativeToRepositoryRoot) {
			return new RepositoryItem { Name = filePath.GetFileNameUniversal(), PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, filePath), Type = "file" };
		}

		private RepositoryItem CreateFolderItem(string folder, IEnumerable<string> filePaths, string projectFolderRelativeToRepositoryRoot) { //folder -- currect piece of path (w/o a slash), files -- ones within that folder, relative to this folder
			var folderItem = new RepositoryItem {Name = folder, PathRelativeToRepositoryRoot = projectFolderRelativeToRepositoryRoot, Type = "folder"};
			var childItems = GetChildItems(folder, filePaths, projectFolderRelativeToRepositoryRoot);
			folderItem.Children.AddRange(childItems);
			return folderItem;
		}

		private IEnumerable<RepositoryItem> GetChildItems(string folder, IEnumerable<string> filePaths, string projectFolderRelativeToRepositoryRoot) {
			projectFolderRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, folder);
			var folders = filePaths.Select(path => path.ParentDirectory()).Where(f => f.IsNotEmpty());//subfolders relative to folder
			var rootFolders = folders.Select(f => f.getPathParts()[0]).Distinct();
			var folderItems =
				rootFolders.Select(
					rootFolder =>
					CreateFolderItem(rootFolder,
					                 GetFilesFromSubfolder(filePaths, rootFolder), projectFolderRelativeToRepositoryRoot));
			var fileItems =
				filePaths.Where(path => path.ParentDirectory().IsEmpty()).Select(
					path => CreateFileItem(path, projectFolderRelativeToRepositoryRoot));
			var childItems = folderItems.Concat(fileItems);
			return childItems;
		}

		private static IEnumerable<string> GetFilesFromSubfolder(IEnumerable<string> filePaths, string rootFolder) { //select all files from this subfolder and take paths relative to it
			return filePaths.Where(path => path.StartsWith(rootFolder + Path.DirectorySeparatorChar)).Select(
				path => path.PathRelativeTo(rootFolder));
		}
	}
}
