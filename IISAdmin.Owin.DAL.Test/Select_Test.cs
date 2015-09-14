using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISAdmin.Owin.DAL.Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISAdmin.Owin.DAL.Test {
	[TestClass]
	public class SelectTest {
		[TestMethod]
		public void SelectTopDistinctToString() {
			var select = new Select().Top(1).Distinct()
				.Columns(@"vr.ID, 
						vr.Name, 
						tb.Name as [Version], 
						vr.IsPublished as IsRelease, 
						vr.CreatedOn as CreatedOn, 
						vr.BuildFolderLink as ZipFilePath")
				.From(@"vw_Release vr
						LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID")
				.Where("vr.ID = @releaseId");
			Assert.AreEqual(@"SELECT DISTINCT TOP 1
vr.ID, 
	vr.Name, 
	tb.Name as [Version], 
	vr.IsPublished as IsRelease, 
	vr.CreatedOn as CreatedOn, 
	vr.BuildFolderLink as ZipFilePath
FROM vw_Release vr
	LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID
WHERE vr.ID = @releaseId", select);
		}
		[TestMethod]
		public void SelectToString() {
			var select = new Select()
				.Columns(@"vr.ID, 
						vr.Name, 
						tb.Name as [Version], 
						vr.IsPublished as IsRelease, 
						vr.CreatedOn as CreatedOn, 
						vr.BuildFolderLink as ZipFilePath")
				.From(@"vw_Release vr
						LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID")
				.Where("vr.ID = @releaseId");
			Assert.AreEqual(@"SELECT 
vr.ID, 
	vr.Name, 
	tb.Name as [Version], 
	vr.IsPublished as IsRelease, 
	vr.CreatedOn as CreatedOn, 
	vr.BuildFolderLink as ZipFilePath
FROM vw_Release vr
	LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID
WHERE vr.ID = @releaseId", select);
		}
	}
}
