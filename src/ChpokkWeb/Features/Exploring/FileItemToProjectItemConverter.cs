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
			var folders = filePaths.Select(path => path.ParentDirectory()).Where(f => f.IsNotEmpty());
			var rootFolders = folders.Select(folder => folder.getPathParts()[0]).Distinct();
			var folderItems =
				rootFolders.Select(folder => CreateFolderItem(folder, filePaths.Where(path => path.StartsWith(folder + Path.DirectorySeparatorChar)).Select(path => path.PathRelativeTo(folder)), projectFolderRelativeToRepositoryRoot));//
			var fileItems = filePaths.Where(path => path.ParentDirectory().IsEmpty()).Select(item => CreateFileItem(item, projectFolderRelativeToRepositoryRoot));
			return folderItems.Concat(fileItems);
		}

		private RepositoryItem CreateFileItem(string filePath, string projectFolderRelativeToRepositoryRoot) {
			return new RepositoryItem { Name = filePath.GetFileNameUniversal(), PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, filePath), Type = "file" };
		}

		private RepositoryItem CreateFolderItem(string folder, IEnumerable<string> filePaths, string projectFolderRelativeToRepositoryRoot) { //folder -- currect piece of path (w/o a slash), files -- ones within that folder, relative to this folder
			var folderItem = new RepositoryItem {Name = folder, PathRelativeToRepositoryRoot = projectFolderRelativeToRepositoryRoot, Type = "folder"};
			projectFolderRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, folder);
			var folders = filePaths.Select(path => path.ParentDirectory()).Where(f => f.IsNotEmpty()); //subfolders relative to folder
			var rootFolders = folders.Select(f => f.getPathParts()[0]).Distinct();
			var folderItems =
				rootFolders.Select(
					rootFolder =>
					CreateFolderItem(rootFolder,
					                 filePaths.Where(path => path.StartsWith(rootFolder + Path.DirectorySeparatorChar)).Select(
					                 	path => path.PathRelativeTo(rootFolder)), projectFolderRelativeToRepositoryRoot));
			var fileItems =
				filePaths.Where(path => path.ParentDirectory().IsEmpty()).Select(
					path => CreateFileItem(path, projectFolderRelativeToRepositoryRoot));
			folderItem.Children.AddRange(folderItems);
			folderItem.Children.AddRange(fileItems);
			//foreach (var rootFolder in rootFolders) {
			//    var childFolderItem = CreateFolderItem(rootFolder, filePaths.Where(path => path.StartsWith(rootFolder + Path.DirectorySeparatorChar)).Select(path => path.PathRelativeTo(rootFolder)), projectFolderRelativeToRepositoryRoot);
			//    folderItem.Children.Add(childFolderItem);
			//}
			//foreach (var filePath in filePaths.Where(path => path.ParentDirectory().IsEmpty())) {
			//    folderItem.Children.Add(CreateFileItem(filePath, projectFolderRelativeToRepositoryRoot));
			//}
			return folderItem;
		}
	}
}
