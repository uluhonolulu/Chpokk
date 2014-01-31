using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpSvn;
using FubuCore;
using SharpSvn.Remote;
using SharpSvn.Security;

namespace SmokeTests {
	public class SvnSpike {
		[Test]
		public void CanCreateARepositoryAndCheckoutFilesAndCommit() {
			using (var client = new SharpSvn.SvnClient()) {
				//SvnUpdateResult result;
				SvnRepositoryClient repositoryClient = new SvnRepositoryClient();
				//var folder = Path.GetTempPath().AppendPath("SvnTest");
				//Console.WriteLine(folder);
				//Directory.CreateDirectory(folder);
				client.Authentication.Clear(); // prevents checking cached credentials
				client.Authentication.DefaultCredentials = new System.Net.NetworkCredential("ulu", "tudasuda");
				client.Authentication.SslServerTrustHandlers += delegate(object sender, SvnSslServerTrustEventArgs e) {
					e.AcceptedFailures = e.Failures;
					e.Save = true; // Save acceptance to authentication store
				};

				Console.WriteLine(AppDomain.CurrentDomain.DynamicDirectory);
				Console.WriteLine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
 
				try {
					//client.Progress += (sender, args) => Console.WriteLine(args.Progress.ToString() + "/" + args.TotalProgress);
					client.Processing += (sender, args) => Console.WriteLine(args.CommandType);
					Uri uri;
					new SvnRemoteSession(new Uri("https://subversion.assembla.com/svn/unyan/branches/Bunny")).GetRepositoryRoot(out uri);
					//Console.WriteLine(uri);
					var directoryName = Path.GetDirectoryName(uri.ToString());
					Console.WriteLine(directoryName.PathRelativeTo(directoryName.ParentDirectory()));
					var targetFolder = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\Repositories\unyan";
					client.CheckOut(new SvnUriTarget("https://subversion.assembla.com/svn/unyan/branches/Bunny"),
					                targetFolder);
					var newFilePath = targetFolder.AppendPath(Path.GetFileName(Path.GetTempFileName()));
					new FileSystem().WriteStringToFile(newFilePath, "-");
					client.Add(newFilePath, new SvnAddArgs() {});
					client.Commit(targetFolder, new SvnCommitArgs(){LogMessage = "Hey new commit!"});
				}
				finally {
					//Directory.Delete(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\Repositories\unyan", true);
				}
			}
		}
	}
}
