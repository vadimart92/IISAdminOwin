using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
=======
using System.Runtime.Serialization;
using System.Threading;
>>>>>>> 0afa09b048d35f086cc295df409d44cc9f69802d
using IISAdmin.Interfaces;
using IISAdmin.Owin.Models;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Data;

namespace IISAdmin.Owin.SignaRHubs
{
<<<<<<< HEAD
	public class SiteCreate : BaseHub<SiteManagement>
	{
		private IWebSiteRepository _siteRepository;
		private IReleaseRepository _releaseRepository;
		private ISqlServerInstanceRepository _serverInstanceRepository;

		public SiteCreate(IWebSiteRepository siteRepository, IReleaseRepository releaseRepository, ISqlServerInstanceRepository serverInstanceRepository)
		{
			_siteRepository = siteRepository;
			_releaseRepository = releaseRepository;
			_serverInstanceRepository = serverInstanceRepository;
		}

		public IRelease GetReleaseInfo(string uri)
		{
			return _releaseRepository.GetByUri(uri);
		}

		public SiteCreationInfo GetStartupInfo()
		{
			var sqlInstances = _serverInstanceRepository.GetAllInstances();
			var res = new SiteCreationInfo
			{
				FreeRedisDbNum = GetFreeRedisDb(),
				SqlServerInstances = sqlInstances
			};
			return res;
		}

		private int GetFreeRedisDb()
		{
			_siteRepository.ClearSiteCache();
			var sites = _siteRepository.GetAllSites();
			var numberFound = false;
			var number = -1;
			do
			{
				number++;
				if (sites.Any(s => s.Redis.Db == number)) continue;
				numberFound = true;
			} while (!numberFound);
			return number;
		}

		public void AddSite(SiteCreateData data)
		{
			_siteRepository.CreateSite(data);
		}
	}

	[JsonObject]
	public class SiteCreationInfo
	{
		public int FreeRedisDbNum { get; set; }

		public IList<ISqlServerInstance> SqlServerInstances { get; set; }
	}

	[JsonObject]
	public class SiteCreateData : ISiteCreateData
	{
		public string Name { get; set; }

		public int Redis { get; set; }

		public SignalrRelease ReleaseInfo { get; set; }

		public SignalrSqlServerInstance Db { get; set; }

		#region Члены ISiteCreateData

		IRelease ISiteCreateData.ReleaseInfo
		{ get { return ReleaseInfo; } }
=======

    public class SiteCreate : BaseHub<SiteManagement>
    {

        private IWebSiteRepository _siteRepository;
        private IReleaseRepository _releaseRepository;
        private ISqlServerInstanceRepository _serverInstanceRepository;

        public SiteCreate(IWebSiteRepository siteRepository, IReleaseRepository releaseRepository, ISqlServerInstanceRepository serverInstanceRepository)
        {
            _siteRepository = siteRepository;
            _releaseRepository = releaseRepository;
            _serverInstanceRepository = serverInstanceRepository;
        }

        public IRelease GetReleaseInfo(string uri)
        {
            return _releaseRepository.GetByUri(uri);
        }

        public List<string> GetMsSqlInstancesNames()
        {
            SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            DataTable dataTable = instance.GetDataSources();

            var result = (from row in dataTable.AsEnumerable()
                          select row[0] + "\\" + row[1]).ToList();

            return result;
        }

        public SiteCreationInfo GetStartupInfo()
        {
            var serverInstancesNames = GetMsSqlInstancesNames();
            var sqlInstances = _serverInstanceRepository.GetAllInstances(serverInstancesNames);
            var res = new SiteCreationInfo
            {
                FreeRedisDbNum = GetFreeRedisDb(),
                SqlServerInstances = sqlInstances
            };
            return res;
        }

        private int GetFreeRedisDb()
        {
            _siteRepository.ClearSiteCache();
            var sites = _siteRepository.GetAllSites();
            var numberFound = false;
            var number = -1;
            do
            {
                number++;
                if (sites.Any(s => s.Redis.Db == number)) continue;
                numberFound = true;
            } while (!numberFound);
            return number;
        }

        public void AddSite(SiteCreateData data)
        {
            _siteRepository.CreateSite(data);
        }
    }


    [JsonObject]
    public class SiteCreationInfo
    {
        public int FreeRedisDbNum { get; set; }
        public IList<ISqlServerInstance> SqlServerInstances { get; set; }
    }

    [JsonObject]
    public class SiteCreateData : ISiteCreateData
    {
        public string Name { get; set; }
        public int Redis { get; set; }
        public SignalrRelease ReleaseInfo { get; set; }
        public SignalrSqlServerInstance Db { get; set; }

        #region Члены ISiteCreateData

        IRelease ISiteCreateData.ReleaseInfo { get { return ReleaseInfo; } }
        ISqlServerInstance ISiteCreateData.Db { get { return Db; } }

        #endregion
    }
>>>>>>> 0afa09b048d35f086cc295df409d44cc9f69802d

		ISqlServerInstance ISiteCreateData.Db
		{ get { return Db; } }

		#endregion Члены ISiteCreateData
	}
}