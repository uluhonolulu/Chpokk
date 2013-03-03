using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibGit2Sharp;

namespace ChpokkWeb.Features.Remotes.Push {
	public class PushController {
		public void Push(string path) {
			using (Repository repo = new Repository(path)) {
				//Remote remote = repo.Remotes["origin"];
				//PushResult pushResult = repo.Network.Push(remote, "HEAD", "refs/heads/destination_branch");
				//if (pushResult.HasErrors) {
				//    Console.WriteLine("Errors:");
				//    foreach (PushStatusError error in pushResult.FailedPushUpdates) {
				//        Console.WriteLine("\t{0} : {1}", error.Reference, error.Message);
				//    }
				//}
			}
		}
	}
}