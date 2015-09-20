using Microsoft.Web.Administration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteManagment.Core.Common
{
    internal class ProcessUtils
    {
        public static void RestartPool(ApplicationPool pool, ServerManager srvManager)
        {
            StopPool(pool);
            StartPool(pool);
        }

        public static void StartPool(ApplicationPool pool)
        {
            pool.Start();
        }

        public static void StopPool(ApplicationPool pool, TimeSpan? waitTimeout = null)
        {
            var timeout = waitTimeout ?? TimeSpan.FromSeconds(20);
            if (pool.State == ObjectState.Stopped)
            {
                return;
            }
            var task = Task.Run(() =>
            {
                pool.Stop();
            });
            if (task.Wait(timeout))
            {
                return;
            }
            KillWorkerProcesses(pool);
        }

        private static void KillWorkerProcesses(ApplicationPool pool)
        {
            var processUIds = pool.WorkerProcesses.ToList().ConvertAll(p => p.ProcessId);
            foreach (var id in processUIds)
            {
                try
                {
                    var process = Process.GetProcessById(id);
                    process.Kill();
                }
                catch { }
            }
        }
    }
}