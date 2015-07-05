using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using SharpCompress.Archive;
using SharpCompress.Common;

namespace WebSiteManagment.Core.Common
{
	public class ZipExtractor
	{
		public static void ExtractBuild(string zipFileUri, string destinationPath, IList<string> extensionsToIgnore) {
			using (var archive = ArchiveFactory.Open(zipFileUri)) {
				foreach (var entry in archive.Entries) {
					if (!entry.IsDirectory && !extensionsToIgnore.Any((extinsion) => entry.FilePath.EndsWith(extinsion))) {
						entry.WriteToDirectory(destinationPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
					}
				}
			}
		}

		public static void ExtractFileFromZip(string zipFileUri, string fileDirectory, string fileName, string destinationFilePath) {
			using (var archive = ArchiveFactory.Open(zipFileUri)) {
				var file = archive.Entries.FirstOrDefault(e => {
					var fi = new FileInfo(e.FilePath);
					return fi.DirectoryName != null && fi.DirectoryName.EndsWith(fileDirectory) && fi.Name.EndsWith(fileName);
				});
				if (file == null) {
					throw new FileNotFoundException("fileName");
				}
				file.WriteToFile(destinationFilePath);
			}
		}
	}
}