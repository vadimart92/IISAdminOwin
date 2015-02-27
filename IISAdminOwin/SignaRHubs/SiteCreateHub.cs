﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IISAdmin.Interfaces;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs {
	
	public class SiteCreate: BaseHub<SiteManagement> {
	
		private IWebSiteRepository _siteRepository;
		private IReleaseRepository _releaseRepository;
		private ISqlServerInstanceRepository _serverInstanceRepository;
		
		public SiteCreate(IWebSiteRepository siteRepository, IReleaseRepository releaseRepository, ISqlServerInstanceRepository serverInstanceRepository) {
			_siteRepository = siteRepository;
			_releaseRepository = releaseRepository;
			_serverInstanceRepository = serverInstanceRepository;
		}

		public IRelease GetReleaseInfo(string uri) {
			return _releaseRepository.GetByUri(uri);
		}

		public SiteCreationInfo GetStartupInfo() {
			var sqlInstances = _serverInstanceRepository.GetAllInstances();
			var res = new SiteCreationInfo {
				FreeRedisDbNum = GetFreeRedisDb(),
				SqlServerInstances = sqlInstances
			};
			return res;
		}

		private int GetFreeRedisDb() {
			_siteRepository.ClearSiteCache();
			var sites = _siteRepository.GetAllSites();
			var numberFound = false;
			var number = -1;
			do {
				number++;
				if (sites.Any(s => s.Redis.Db == number)) continue;
				numberFound = true;
			} while (!numberFound);
			return number;
		}
	}


	[JsonObject]
	public class SiteCreationInfo {
		public int FreeRedisDbNum { get; set; }
		public IList<ISqlServerInstance> SqlServerInstances { get; set; }
	}

}