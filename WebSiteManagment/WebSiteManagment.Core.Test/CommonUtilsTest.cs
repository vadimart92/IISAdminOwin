using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSiteManagment.Core.Common;

namespace WebSiteManagment.Core.Test {
	[TestClass]
	public class CommonUtilsTest {
		[TestMethod]
		public void GetFreePort() {
			Assert.IsTrue(CommonUtils.SafeGetFreeTcpPort() > -1);
		}
	}
}
