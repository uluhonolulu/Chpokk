using System;
using NUnit.Framework;
using SharpSvn;
using FubuCore;
using SharpSvn.Security;
using MbUnit.Framework;

//bitness on AppHarbor: 32bit

namespace SvnTests {
	public class SvnSpike {
		[Test]//, Ignore("Appharbor doesn't like it")
		public void CanCreateARepositoryAndCheckoutFilesAndCommit() {
			var targetFolder =
				AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ParentDirectory().AppendPath("UserFiles/ulu/Repositories");
		using (var client = new SvnClient()) {
			//SvnUpdateResult result;
			//var folder = Path.GetTempPath().AppendPath("SvnTest");
			//Console.WriteLine(folder);
			//Directory.CreateDirectory(folder);
			//client.Authentication.Clear(); // prevents checking cached credentials
			//client.Authentication.ForceCredentials("-", "-");
			//client.Authentication.DefaultCredentials = new NetworkCredential("--", "---");
			client.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e) {
				e.AcceptedFailures = e.Failures;
				e.Save = true; // Save acceptance to authentication store
			};
			client.Authentication.UserNamePasswordHandlers += (sender, args) =>
			{
				args.UserName = "drzitz";
				//args.UserName = "guest";
				args.Password = "iddqd710";
				args.Save = true;
			};
			//client.Authentication.
			client.Authentication.BeforeEngineDialog += (sender, args) => {
				                                                              Console.WriteLine(args.ToString());
			};
			client.Notify += (sender, args) => {
				Console.WriteLine(args.Action + " " + args.NodeKind + ": " + args.Path);//don't display NodeKind if it is None
			};
			client.Authentication.UserNameHandlers += (sender, args) => {
				Console.WriteLine(args.UserName);
			};


			string repositoryName = "drzitz";
			//Console.WriteLine(repositoryName);
			targetFolder = targetFolder.AppendPath(repositoryName);
			//return;
			client.LoadConfiguration(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\svn", true);

			try {
				var repoUrl = "http://178.63.130.238:8080/svn/dis/trunk";
				//repoUrl = "https://sharpsvn.open.collab.net/svn/sharpsvn/trunk";
				//targetFolder = @"D:\Projects\OSS\sharpsvn_2";
				targetFolder = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\Repositories\drzitz";
				client.CheckOut(new SvnUriTarget(repoUrl), targetFolder);
			}
			catch (Exception e) {
				Console.WriteLine(e);
			}	
		}

			//using (var client = new SvnClient() ) {
			//		var newFilePath = targetFolder.AppendPath(Path.GetFileName(Path.GetTempFileName()));
			//		new FileSystem().WriteStringToFile(newFilePath, "-");
			//		client.Add(newFilePath, new SvnAddArgs() { });
			//		client.Authentication.UserNamePasswordHandlers += (sender, args) => {
			//			Console.WriteLine(args.InitialUserName);
			//		};
			//		client.Commit(targetFolder, new SvnCommitArgs() { LogMessage = "Hey new commit!" });
				
			//}
		}

		[Test]
		public void CanCreateALocalRepo() {
			var repositoryPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ParentDirectory().AppendPath("Users/ulu/Repositories");

		}

		[Test]
		public void AllAssemblies() {
			//TypeMock.ArrangeActAssert.Isolate.WhenCalled(() => Console.WriteLine()).CallOriginal();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies) {
				Console.WriteLine(assembly.FullName + ": " + assembly.GetName().ProcessorArchitecture);
			}
			var client = new SvnClient();
		}
	}
}
