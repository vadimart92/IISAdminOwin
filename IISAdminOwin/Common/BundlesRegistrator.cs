using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace IISAdminOwin.Common {
	public class BundlesRegistrator {
		public static void RegisterBundles(IAppBuilder appBuilder) {
			appBuilder.UseFileServer(new FileServerOptions {
				FileSystem = new PhysicalFileSystem(GetAssetsDirectory()),
				EnableDirectoryBrowsing = true,
				RequestPath = new Microsoft.Owin.PathString("/assets")
			});

			appBuilder.UseFileServer(new FileServerOptions {
				FileSystem = new PhysicalFileSystem(GetAppDirectory()),
				EnableDirectoryBrowsing = true,
				RequestPath = new Microsoft.Owin.PathString("/app")
			});

			appBuilder.UseFileServer(new FileServerOptions {
				FileSystem = new PhysicalFileSystem(GetImgDirectory()),
				EnableDirectoryBrowsing = true,
				RequestPath = new Microsoft.Owin.PathString("/img")
			});
		}

		public static string GetResourceRootDir() {
			var rootDirectory = HostingEnvironment.ApplicationPhysicalPath;
			return Path.Combine(rootDirectory, "resources");
		}
		public static string GetAssetsDirectory() {
			return Path.Combine(GetResourceRootDir(), "assets");
		}
		public static string GetImgDirectory() {
			return Path.Combine(GetResourceRootDir(), "img");
		}
		public static string GetAppDirectory() {
			return Path.Combine(GetResourceRootDir(), "app");
		}
	}
}