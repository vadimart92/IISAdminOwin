using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using IISAdmin.Interfaces;
using IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService;

namespace IISAdmin.WCFWebSiteRepository {

	public class WcfClientWebSiteRepository: IWebSiteRepository {
		
		private readonly WebSiteRepositoryServiceClient _siteRepositoryServiceClient;
		private IWebSiteRepositoryService _repositoryService;

		public WcfClientWebSiteRepository(string bindingName) {
			_siteRepositoryServiceClient = new WebSiteRepositoryServiceClient(bindingName);
		}

		private void OpenChannel() {
			if (_siteRepositoryServiceClient.InnerChannel.State != CommunicationState.Opened
				&& _siteRepositoryServiceClient.InnerChannel.State != CommunicationState.Opening) {
					_repositoryService = _siteRepositoryServiceClient.ChannelFactory.CreateChannel();
			}
		}

		private void CloseChannel() {
			_siteRepositoryServiceClient.InnerChannel.Close();
		}

		#region Члены IWebSiteRepository

		public List<ISite> GetAllSites() {
			OpenChannel();
			try {
				var res = _repositoryService.GetAllSites().ConvertAll(s => (ISite) new IisSite(s));
				return res;
			}
			catch (FaultException ex) {
				if (ex.Reason.GetMatchingTranslation(CultureInfo.CurrentCulture).Text == "AccessDenied") {
					throw new UnauthorizedAccessException(ex.Message, ex);
				}
				throw;
			}
			finally {
				CloseChannel();	
			}
		}

		public void StopSite(long siteId) {
			OpenChannel();
			_repositoryService.StopSite(siteId);
			CloseChannel();
		}

		public void StartSite(long siteId) {
			OpenChannel();
			_repositoryService.StartSite(siteId);
			CloseChannel();
		}

		public ISite GetSite(long id) {
			OpenChannel();
			var site = _repositoryService.GetSite(id);
			CloseChannel();
			return new IisSite(site);
		}

		public ISite FindSite(long id) {
			OpenChannel();
			var site = _repositoryService.FindSite(id);
			CloseChannel();
			return new IisSite(site);
		}

		public void RestartSitePool(long siteId) {
			OpenChannel();
			_repositoryService.RestartSitePool(siteId);
			CloseChannel();
		}

		public void FlushSiteRedis(long siteId) {
			OpenChannel();
			_repositoryService.FlushSiteRedis(siteId);
			CloseChannel();
		}

		public void SetCacheUsage(bool type) {
			OpenChannel();
			_repositoryService.SetCacheUsage(type);
			CloseChannel();
		}

		public void ClearSiteCache() {
			OpenChannel();
			_repositoryService.ClearCache();
			CloseChannel();
		}

		public void CreateSite(ISiteCreateData data) {
			throw new NotImplementedException();
		}

		#endregion

	}
}
