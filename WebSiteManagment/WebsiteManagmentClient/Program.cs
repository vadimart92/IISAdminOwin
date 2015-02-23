using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSiteManagment;


namespace WebsiteManagmentClient {
    class Program {
        static void Main(string[] args) {
            var mgr = new WebSiteManager();
            var sites = mgr.GetWebsites();
	        mgr.StopPool(sites.First().Applications.First().PoolName);
            //mgr.StopPool(Int16.Parse(Console.ReadLine()));
            Console.ReadKey();
        }
    }
}
