using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.GitHub;
using Chpokk.Tests.Infrastructure;
using FubuCore;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Security;
using LibGit2Sharp;
using LibGit2Sharp.Tests.TestHelpers;
using MbUnit.Framework;
using StructureMap;

namespace Chpokk.Tests.Authentication.Context {
	public class ClonedPrivateRepoContext : RemoteRepositoryContext {

		public override void Create() {
			//CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "CreateInstance"));
			//CThruEngine.StartListening();
			//var securityStub = Stub.For<ISecurityContext>("get_CurrentIdentity").Return(new GenericIdentity("name1"));

			FakeSecurityContext = new FakeSecurityContext {UserName = "name1"};
				
			base.Create();


			if (Directory.Exists(RepositoryPath.ParentDirectory()) ) 
				DirectoryHelper.DeleteSubdirectories(RepositoryPath.ParentDirectory());
			Repository.Clone(REPO_URL, RepositoryPath);

			//now let's see what a different user has
			FakeSecurityContext.UserName = "name2";
		}




		protected override void ConfigureContainer(Container container) {
			base.ConfigureContainer(container);
			//first user clones a repo
			//FakeSecurityContext = new FakeSecurityContext() {UserName = "name1"};
			//Console.WriteLine(container.WhatDoIHave());

			//container.Configure(expression => expression.For<ISecurityContext>().Use(FakeSecurityContext));
		}
	}
}
