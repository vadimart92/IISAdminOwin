using System.Collections.Generic;
using System.ServiceModel;
using WebSiteManagment.Core.Models;

namespace WebSiteManagment.Wcf {

	[ServiceContract]
	public interface IWebSiteRepositoryService {

		[OperationContract]
		List<Site> GetAllSites();

		[OperationContract]
		Site GetSite(long id);

		[OperationContract]
		Site FindSite(long id);

		[OperationContract]
		void StopSite(long id);

		[OperationContract]
		void StartSite(long id);

		[OperationContract]
		void RestartSitePool(long siteId);

		[OperationContract]
		void FlushSiteRedis(long siteId);

		[OperationContract]
		void SetCacheUsage(bool type);

		[OperationContract]
		void ClearCache();

		[OperationContract]
		void AddSite(object siteData);
		//todo: issue 9 create info for add site
	}

}