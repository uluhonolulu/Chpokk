using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Infrastructure;

namespace ChpokkWeb.Features.Remotes.DownloadZip {
	public class DownloadZipMenuItem: MenuItem {
		public DownloadZipMenuItem() {
			Id = "zipper";
			Caption = "Download .zip";
		}
	}
}