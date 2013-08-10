﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using FubuCore;
using FubuMVC.Core;
using HtmlTags;

namespace ChpokkWeb.Features.Exploring {
	public class SolutionContentEndpoint {
		[NotNull] private readonly RepositoryManager _repositoryManager;

		[NotNull] private readonly IFileSystem _fileSystem;

		[NotNull] private readonly SolutionFileLoader _solutionFileLoader;

		public SolutionContentEndpoint([NotNull]RepositoryManager repositoryManager, [NotNull]IFileSystem fileSystem, [NotNull
		                                                                                                                ] SolutionParser solutionParser, [NotNull] SolutionFileLoader solutionFileLoader) {
			_repositoryManager = repositoryManager;
			_fileSystem = fileSystem;
			_solutionFileLoader = solutionFileLoader;
		}

		[JsonEndpoint]
		public SolutionExplorerModel GetSolutions([NotNull]SolutionExplorerInputModel model) {
			var info = _repositoryManager.GetRepositoryInfo(model.RepositoryName);
			var folder = FileSystem.Combine(model.PhysicalApplicationPath, info.Path);
			var files = _fileSystem.FindFiles(folder, new FileSet { Include = "*.sln" });
			var items =
				files.Select(
					filePath =>
					_solutionFileLoader.CreateSolutionItem(folder, filePath));
			return new SolutionExplorerModel {Items = items.ToArray()};
		}

		//[JsonEndpoint]
		public HtmlTag GetSolutionFolders(SolutionFolderExplorerInputModel model) {
			var ul = new HtmlTag("ul").AddClass("jqueryFileTree").Hide();
			var li = ul.Add("li").AddClasses("directory", "collapsed");
			li.Add("a").Attr("rel", "somepath").Text("Root stuff");
			li = li.Add("ul").AddClass("jqueryFileTree").Hide().Add("li").AddClasses("directory", "collapsed");
			li.Add("a").Attr("rel", "childpath").Text("Child stuff");

			return ul;
			//return new SolutionExplorerModel{Items = new[] {new RepositoryItem(){Name = "Stuff", Type = "folder"}}};
		}
	}

	public class SolutionFolderExplorerInputModel: BaseRepositoryInputModel {}
}