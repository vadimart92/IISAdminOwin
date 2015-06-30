using System.Threading.Tasks;
using SharpCompress.Archive;
using SharpCompress.Common;

namespace WebSiteManagment.Core.Common
{
	public class BuildExtractor
	{
		public static Task ExtractBuildAsync(string zipFileUri, string destinationPath) {
			return Task.Run(()=> {
				var archive = ArchiveFactory.Open(zipFileUri);
				foreach (var entry in archive.Entries) {
					if (!entry.IsDirectory) {
						entry.WriteToDirectory(destinationPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
					}
				}
			});
		}
	}
}