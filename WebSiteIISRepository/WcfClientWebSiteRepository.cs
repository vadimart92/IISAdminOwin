using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using IISAdmin.Interfaces;
using IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService;
using WebSiteManagment.Core.Models;

namespace IISAdmin.WCFWebSiteRepository
{
	public class WcfClientWebSiteRepository : IWebSiteRepository
	{
		private readonly WebSiteRepositoryServiceClient _siteRepositoryServiceClient;
		private IWebSiteRepositoryService _repositoryService;

		public WcfClientWebSiteRepository(string bindingName) {
			_siteRepositoryServiceClient = new WebSiteRepositoryServiceClient(bindingName);
		}

		public WcfClientWebSiteRepository(Binding binding, EndpointAddress remoteAddress) {
			_siteRepositoryServiceClient = new WebSiteRepositoryServiceClient(binding, remoteAddress);
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
				var res = _repositoryService.GetAllSites().ConvertAll(s => (ISite)new IisSite(s));
				return res;
			} catch (FaultException ex) {
				if (ex.Reason.GetMatchingTranslation(CultureInfo.GetCultureInfo("en-US")).Text == "AccessDenied") {
					throw new UnauthorizedAccessException(ex.Message, ex);
				}
				throw;
			} finally {
				CloseChannel();
			}
		}

	    public List<IApplication> GetAllApplications() {
	        throw new NotImplementedException();
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

		public long CreateSite(SiteCreateData data, int appCount) {
			try {
				SiteInfo param = new SiteInfo {
					Name = data.WebAppName,
					WebAppDir = data.DestinationWebAppRoot,
					AppCount = appCount,
					CreateNewSite = data.CreateNewSite,
					SiteName = data.WebAppName
				};
				OpenChannel();
				return _repositoryService.AddSite(param);
			} finally {
				CloseChannel();
			}
		}

		public void ModifyConnectionStrings(long siteId, Dictionary<string, string> config) {
			OpenChannel();
			_repositoryService.ModifyConnectionStrings(siteId, config);
			CloseChannel();
		}

		public int GetFreePortNumber() {
			OpenChannel();
			var res = _repositoryService.GetNexFreePort();
			CloseChannel();
			return res;
		}

		#endregion Члены IWebSiteRepository
	}
}