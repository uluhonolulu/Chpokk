using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChpokkWeb.Features {
	public class BitnessEndpoint {
		public string DisplayBitness() {
			return Environment.Is64BitProcess.ToString() + " " + Environment.Is64BitOperatingSystem.ToString(); 
		}
	}
}