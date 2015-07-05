using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSiteManagment.Core.Common;

namespace WebSiteManagment.Core.Test {
	[TestClass]
	public class ZipExtractorTest {

		private static class Constants {
			public static string ExtractedFileName = "extracted.txt";
		}

		[TestMethod]
		public void ExtractFileFromZip() {
			ZipExtractor.ExtractFileFromZip("test.zip", "testFolder", @"test.txt", Constants.ExtractedFileName);
			Assert.AreEqual(@"test", File.ReadAllText(Constants.ExtractedFileName));
			File.Delete(Constants.ExtractedFileName);
		}
		
		[TestMethod]
		public void ExtractFileFromZip_byFileExtension() {
			ZipExtractor.ExtractFileFromZip("test.zip", "testFolder", @".txt", "extracted.txt");
			Assert.AreEqual(@"test", File.ReadAllText("extracted.txt"));
			File.Delete(Constants.ExtractedFileName);
		}

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ExtractFileFromZip_FileNotFoundIfDirNotRight() {
			ZipExtractor.ExtractFileFromZip("test.zip", "testFolder1", @"test.txt", "extracted.txt");
		}

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ExtractFileFromZip_FileNotFoundIfFileNameNotCorrect() {
			ZipExtractor.ExtractFileFromZip("test.zip", "testFolder", @"test1.txt", "extracted.txt");
		}
	}
}
