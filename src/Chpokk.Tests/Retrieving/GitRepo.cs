﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Arractas;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Remotes.DownloadZip;
using ChpokkWeb.Features.Remotes.Push;
using ChpokkWeb.Features.RepositoryManagement;
using ChpokkWeb.Infrastructure;
using LibGit2Sharp;
using MbUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Chpokk.Tests.Retrieving {
	public class GitRepo : BaseQueryTest<GitRepositoryContext, IEnumerable<MenuItem>> {
		[Test]
		public void CanPush() {
			Result.ShouldContainItemOfType<PushMenuItem>();
		}
		[Test]
		public void CanDownloadZip() {
			Result.ShouldContainItemOfType<DownloadZipMenuItem>();
		}

		public override IEnumerable<MenuItem> Act() {
			var controller = Context.Container.Get<RepositoryEndpoint>();
			var model = new RepositoryInputModel {RepositoryName = Context.REPO_NAME};
			return controller.Get(model).RetrieveActions;
		}
	}
}
