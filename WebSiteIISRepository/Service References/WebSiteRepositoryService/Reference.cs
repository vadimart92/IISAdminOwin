﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.0
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WebSiteRepositoryService.IWebSiteRepositoryService")]
    public interface IWebSiteRepositoryService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetAllSites", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetAllSitesResponse")]
        System.Collections.Generic.List<WebSiteManagment.Core.Models.Site> GetAllSites();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetAllSites", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetAllSitesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<WebSiteManagment.Core.Models.Site>> GetAllSitesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetSiteResponse")]
        WebSiteManagment.Core.Models.Site GetSite(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetSiteResponse")]
        System.Threading.Tasks.Task<WebSiteManagment.Core.Models.Site> GetSiteAsync(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/FindSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/FindSiteResponse")]
        WebSiteManagment.Core.Models.Site FindSite(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/FindSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/FindSiteResponse")]
        System.Threading.Tasks.Task<WebSiteManagment.Core.Models.Site> FindSiteAsync(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/StopSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/StopSiteResponse")]
        void StopSite(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/StopSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/StopSiteResponse")]
        System.Threading.Tasks.Task StopSiteAsync(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/StartSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/StartSiteResponse")]
        void StartSite(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/StartSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/StartSiteResponse")]
        System.Threading.Tasks.Task StartSiteAsync(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/RestartSitePool", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/RestartSitePoolResponse")]
        void RestartSitePool(long siteId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/RestartSitePool", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/RestartSitePoolResponse")]
        System.Threading.Tasks.Task RestartSitePoolAsync(long siteId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/FlushSiteRedis", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/FlushSiteRedisResponse")]
        void FlushSiteRedis(long siteId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/FlushSiteRedis", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/FlushSiteRedisResponse")]
        System.Threading.Tasks.Task FlushSiteRedisAsync(long siteId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/SetCacheUsage", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/SetCacheUsageResponse")]
        void SetCacheUsage(bool type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/SetCacheUsage", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/SetCacheUsageResponse")]
        System.Threading.Tasks.Task SetCacheUsageAsync(bool type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/ClearCache", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/ClearCacheResponse")]
        void ClearCache();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/ClearCache", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/ClearCacheResponse")]
        System.Threading.Tasks.Task ClearCacheAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/AddSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/AddSiteResponse")]
        long AddSite(WebSiteManagment.Core.Models.SiteInfo siteInformation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/AddSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/AddSiteResponse")]
        System.Threading.Tasks.Task<long> AddSiteAsync(WebSiteManagment.Core.Models.SiteInfo siteInformation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/ModifyConnectionStrings", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/ModifyConnectionStringsResponse")]
        void ModifyConnectionStrings(long siteId, System.Collections.Generic.Dictionary<string, string> config);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/ModifyConnectionStrings", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/ModifyConnectionStringsResponse")]
        System.Threading.Tasks.Task ModifyConnectionStringsAsync(long siteId, System.Collections.Generic.Dictionary<string, string> config);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetNexFreePort", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetNexFreePortResponse")]
        int GetNexFreePort();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetNexFreePort", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetNexFreePortResponse")]
        System.Threading.Tasks.Task<int> GetNexFreePortAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebSiteRepositoryServiceChannel : IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.IWebSiteRepositoryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebSiteRepositoryServiceClient : System.ServiceModel.ClientBase<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.IWebSiteRepositoryService>, IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.IWebSiteRepositoryService {
        
        public WebSiteRepositoryServiceClient() {
        }
        
        public WebSiteRepositoryServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebSiteRepositoryServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebSiteRepositoryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebSiteRepositoryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<WebSiteManagment.Core.Models.Site> GetAllSites() {
            return base.Channel.GetAllSites();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<WebSiteManagment.Core.Models.Site>> GetAllSitesAsync() {
            return base.Channel.GetAllSitesAsync();
        }
        
        public WebSiteManagment.Core.Models.Site GetSite(long id) {
            return base.Channel.GetSite(id);
        }
        
        public System.Threading.Tasks.Task<WebSiteManagment.Core.Models.Site> GetSiteAsync(long id) {
            return base.Channel.GetSiteAsync(id);
        }
        
        public WebSiteManagment.Core.Models.Site FindSite(long id) {
            return base.Channel.FindSite(id);
        }
        
        public System.Threading.Tasks.Task<WebSiteManagment.Core.Models.Site> FindSiteAsync(long id) {
            return base.Channel.FindSiteAsync(id);
        }
        
        public void StopSite(long id) {
            base.Channel.StopSite(id);
        }
        
        public System.Threading.Tasks.Task StopSiteAsync(long id) {
            return base.Channel.StopSiteAsync(id);
        }
        
        public void StartSite(long id) {
            base.Channel.StartSite(id);
        }
        
        public System.Threading.Tasks.Task StartSiteAsync(long id) {
            return base.Channel.StartSiteAsync(id);
        }
        
        public void RestartSitePool(long siteId) {
            base.Channel.RestartSitePool(siteId);
        }
        
        public System.Threading.Tasks.Task RestartSitePoolAsync(long siteId) {
            return base.Channel.RestartSitePoolAsync(siteId);
        }
        
        public void FlushSiteRedis(long siteId) {
            base.Channel.FlushSiteRedis(siteId);
        }
        
        public System.Threading.Tasks.Task FlushSiteRedisAsync(long siteId) {
            return base.Channel.FlushSiteRedisAsync(siteId);
        }
        
        public void SetCacheUsage(bool type) {
            base.Channel.SetCacheUsage(type);
        }
        
        public System.Threading.Tasks.Task SetCacheUsageAsync(bool type) {
            return base.Channel.SetCacheUsageAsync(type);
        }
        
        public void ClearCache() {
            base.Channel.ClearCache();
        }
        
        public System.Threading.Tasks.Task ClearCacheAsync() {
            return base.Channel.ClearCacheAsync();
        }
        
        public long AddSite(WebSiteManagment.Core.Models.SiteInfo siteInformation) {
            return base.Channel.AddSite(siteInformation);
        }
        
        public System.Threading.Tasks.Task<long> AddSiteAsync(WebSiteManagment.Core.Models.SiteInfo siteInformation) {
            return base.Channel.AddSiteAsync(siteInformation);
        }
        
        public void ModifyConnectionStrings(long siteId, System.Collections.Generic.Dictionary<string, string> config) {
            base.Channel.ModifyConnectionStrings(siteId, config);
        }
        
        public System.Threading.Tasks.Task ModifyConnectionStringsAsync(long siteId, System.Collections.Generic.Dictionary<string, string> config) {
            return base.Channel.ModifyConnectionStringsAsync(siteId, config);
        }
        
        public int GetNexFreePort() {
            return base.Channel.GetNexFreePort();
        }
        
        public System.Threading.Tasks.Task<int> GetNexFreePortAsync() {
            return base.Channel.GetNexFreePortAsync();
        }
    }
}
