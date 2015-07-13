using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSiteManagment.Core.Common;

namespace WebSiteManagment.Core.Test {
	[TestClass]
	public class WebConfigUtilsTest {
		[TestMethod]
		public void ChangeDb() {
			var cs = Guid.NewGuid().ToString();
			WebConfigUtils.SetConnectionString(Environment.CurrentDirectory, "db", cs);
			Assert.IsTrue(File.ReadAllText("ConnectionStrings.config").Contains(cs));
		}
		
		[TestMethod]
		public void ChangeRedis() {
			var cs = Guid.NewGuid().ToString();
			WebConfigUtils.SetConnectionString(Environment.CurrentDirectory, "redis", cs);
			Assert.IsTrue(File.ReadAllText("ConnectionStrings.config").Contains(cs));
		}
	}
}
