using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Win32;

namespace ChpokkWeb.Infrastructure.Windows {
	public class RegistryUtils {
		public static RegistryKey GetRegistryKey(string keyPath) {
			RegistryKey localMachineRegistry
				= RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
										  Environment.Is64BitOperatingSystem
											  ? RegistryView.Registry64
											  : RegistryView.Registry32);

			return string.IsNullOrEmpty(keyPath)
				? localMachineRegistry
				: localMachineRegistry.OpenSubKey(keyPath);
		}

		public static object GetRegistryValue(string keyPath, string keyName) {
			RegistryKey registry = GetRegistryKey(keyPath);
			return registry.GetValue(keyName);
		}
	}
}