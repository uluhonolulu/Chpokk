using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpSvn;
using FubuCore;
using SharpSvn.Remote;
using SharpSvn.Security;
using ChpokkWeb.Infrastructure;

//bitness on AppHarbor: 32bit

namespace SmokeTests {
	public class SvnSpike {
		[Test, Ignore("Appharbor doesn't like it")]
		public void CanCreateARepositoryAndCheckoutFilesAndCommit() {
			var targetFolder =
				AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ParentDirectory().AppendPath("UserFiles/ulu/Repositories");
		using (var client = new SvnClient()) {
			//SvnUpdateResult result;
			//var folder = Path.GetTempPath().AppendPath("SvnTest");
			//Console.WriteLine(folder);
			//Directory.CreateDirectory(folder);
			//client.Authentication.Clear(); // prevents checking cached credentials
			client.Authentication.ForceCredentials("-", "-");
			//client.Authentication.DefaultCredentials = new NetworkCredential("--", "---");
			client.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e) {
				e.AcceptedFailures = e.Failures;
				e.Save = true; // Save acceptance to authentication store
			};
			client.Authentication.UserNamePasswordHandlers += (sender, args) =>
			{
				args.UserName = "uluhonolulu";
				args.Password = "xd11SvG23";
				args.Save = true;
			};
			client.Authentication.UserNameHandlers += (sender, args) => {
				Console.WriteLine(args.UserName);
			};


			string repositoryName = "protected";
			Console.WriteLine(repositoryName);
			targetFolder = targetFolder.AppendPath(repositoryName);
			//return;
			//client.LoadConfiguration(targetFolder, true);
			try {
				client.CheckOut(new SvnUriTarget("https://iigeeksoft.svn.cloudforge.com/protected"), targetFolder);
			}
			catch (Exception e) {
				Console.WriteLine(e);
			}	
		}

			using (var client = new SvnClient() ) {
					var newFilePath = targetFolder.AppendPath(Path.GetFileName(Path.GetTempFileName()));
					new FileSystem().WriteStringToFile(newFilePath, "-");
					client.Add(newFilePath, new SvnAddArgs() { });
					client.Authentication.UserNamePasswordHandlers += (sender, args) => {
						Console.WriteLine(args.InitialUserName);
					};
					client.Commit(targetFolder, new SvnCommitArgs() { LogMessage = "Hey new commit!" });
				
			}
		}

		[Test]
		public void CanCreateALocalRepo() {
			var repositoryPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ParentDirectory().AppendPath("Users/ulu/Repositories");

		}
	}
}
