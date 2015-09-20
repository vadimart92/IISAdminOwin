using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISAdmin.WCFWebSiteRepository;

namespace Wcf.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new WcfClientWebSiteRepository("NetNamedPipeBinding_IWebSiteRepositoryService");
            while (Console.ReadKey().Key != ConsoleKey.Escape) {
                Console.Clear();
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}");
                var sites = repo.GetAllSites();
                foreach (var site in sites) {
                    Console.WriteLine($"Name: {site.Name} State: {site.State} [Redis: {site.Redis.Db}]");
                }
                Console.Write("Press any key to refresh, esc to exit");
            }
        }
    }
}
