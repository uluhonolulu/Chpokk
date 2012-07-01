using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChpokkWeb.Features.Exploring;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class FileItemToProjectItemConverter {
		public IEnumerable<RepositoryItem> Convert(IEnumerable<FileItem> files, string projectFolderRelativeToRepositoryRoot) {
			return files.Select(item => new RepositoryItem { Name = item.Path, PathRelativeToRepositoryRoot = FileSystem.Combine(projectFolderRelativeToRepositoryRoot, item.Path), Type = "file" });
		} 
	}
}
