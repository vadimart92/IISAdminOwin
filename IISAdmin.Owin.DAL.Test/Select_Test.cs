using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISAdmin.Owin.DAL.Test {
	[TestClass]
	public class SelectTest {
		[TestMethod]
		public void SelectToString() {
			var select = new Select().Top("TOP 1")
				.Columns(@"vr.ID, 
						vr.Name, 
						tb.Name as [Version], 
						vr.IsPublished as Release, 
						vr.CreatedOn as CreatedOn, 
						vr.BuildFolderLink as ZipFilePath")
				.From(@"vw_Release vr
						LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID")
				.Where("vr.ID = @releaseId");
			var s = select.ToString();
			Assert.IsNotNull(s);
		}
	}
}
