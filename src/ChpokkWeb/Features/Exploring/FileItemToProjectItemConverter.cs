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
			var folders = filePaths.Select(item => item.ParentDirectory()).Where(folder => folder.IsNotEmpty());
			var rootFolders = folders.Select(folder => folder.getPathParts()[0]).Distinct();
			var folderItems =
				rootFolders.Select(folder => CreateFolderItem(folder, filePaths.Where(path => path.StartsWith(folder + Path.DirectorySeparatorChar)), projectFolderRelativeToRepositoryRoot));//
			var fileItems = filePaths.Where(path => path.ParentDirectory().IsEmpty()).Select(item => CreateFileItem(item, projectFolderRelativeToRepositoryRoot));
			return folderItems.Concat(fileItems);
		}

		private RepositoryItem CreateFileItem(string filePath, string projectFolderRelativeToRepositoryRoot) {
			return new RepositoryItem { Name = filePath.GetFileNameUniversal(), PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, filePath), Type = "file" };
		}

		private RepositoryItem CreateFolderItem(string folder, IEnumerable<string> filePaths, string projectFolderRelativeToRepositoryRoot) { //folder -- currect piece of path (w/o a slash), files -- ones within that folder
			filePaths.Each(s => Console.WriteLine(folder + ": " + s));
			var folderItem = new RepositoryItem {Name = folder, PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, folder), Type = "folder"};
			var folders = filePaths.Select(path => path.PathRelativeTo(folder).ParentDirectory()).Where(f => f.IsNotEmpty()); //subfolders relative to folder
			var rootFolders = folders.Select(f => f.getPathParts()[0]).Distinct();
			foreach (var rootFolder in rootFolders) {
				folderItem.Children.Add(new RepositoryItem{Name = "Subfolder2", Type = "folder"});
			}
			foreach (var filePath in filePaths.Where(path => path.ParentDirectory() == folder)) {
				folderItem.Children.Add(CreateFileItem(filePath, projectFolderRelativeToRepositoryRoot));
			}
			return folderItem;
		}
	}
}
