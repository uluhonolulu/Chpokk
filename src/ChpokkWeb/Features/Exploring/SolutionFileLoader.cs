using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;
using FubuCore;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionFileLoader {
		[NotNull]
		private readonly SolutionParser _solutionParser;
		[NotNull]
		private readonly IFileSystem _fileSystem;

		public SolutionFileLoader(SolutionParser solutionParser, IFileSystem fileSystem) {
			_solutionParser = solutionParser;
			_fileSystem = fileSystem;
		}

		public RepositoryItem CreateSolutionItem(string folder, string filePath) {
			var solutionItem = new RepositoryItem {
				Name = filePath.GetFileNameUniversal(),
				PathRelativeToRepositoryRoot = filePath.PathRelativeTo(folder),
				Type = "folder"
			};
			var content = _fileSystem.ReadStringFromFile(filePath);
			_solutionParser.FillSolutionData(solutionItem, content, filePath);

			return solutionItem;			
		}
	}
}