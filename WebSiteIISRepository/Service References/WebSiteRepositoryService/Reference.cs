﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Site", Namespace="http://schemas.datacontract.org/2004/07/WebSiteManagment.Models")]
    [System.SerializableAttribute()]
    public partial class Site : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Application> ApplicationsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<string> BindingsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Redis RedisField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Application> Applications {
            get {
                return this.ApplicationsField;
            }
            set {
                if ((object.ReferenceEquals(this.ApplicationsField, value) != true)) {
                    this.ApplicationsField = value;
                    this.RaisePropertyChanged("Applications");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<string> Bindings {
            get {
                return this.BindingsField;
            }
            set {
                if ((object.ReferenceEquals(this.BindingsField, value) != true)) {
                    this.BindingsField = value;
                    this.RaisePropertyChanged("Bindings");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Redis Redis {
            get {
                return this.RedisField;
            }
            set {
                if ((object.ReferenceEquals(this.RedisField, value) != true)) {
                    this.RedisField = value;
                    this.RaisePropertyChanged("Redis");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string State {
            get {
                return this.StateField;
            }
            set {
                if ((object.ReferenceEquals(this.StateField, value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Redis", Namespace="http://schemas.datacontract.org/2004/07/WebSiteManagment.Models")]
    [System.SerializableAttribute()]
    public partial class Redis : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ConnectionStringField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DbField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HostField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PortField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ConnectionString {
            get {
                return this.ConnectionStringField;
            }
            set {
                if ((object.ReferenceEquals(this.ConnectionStringField, value) != true)) {
                    this.ConnectionStringField = value;
                    this.RaisePropertyChanged("ConnectionString");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Db {
            get {
                return this.DbField;
            }
            set {
                if ((this.DbField.Equals(value) != true)) {
                    this.DbField = value;
                    this.RaisePropertyChanged("Db");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Host {
            get {
                return this.HostField;
            }
            set {
                if ((object.ReferenceEquals(this.HostField, value) != true)) {
                    this.HostField = value;
                    this.RaisePropertyChanged("Host");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Port {
            get {
                return this.PortField;
            }
            set {
                if ((this.PortField.Equals(value) != true)) {
                    this.PortField = value;
                    this.RaisePropertyChanged("Port");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Application", Namespace="http://schemas.datacontract.org/2004/07/WebSiteManagment.Models")]
    [System.SerializableAttribute()]
    public partial class Application : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.AppPool PoolField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Path {
            get {
                return this.PathField;
            }
            set {
                if ((object.ReferenceEquals(this.PathField, value) != true)) {
                    this.PathField = value;
                    this.RaisePropertyChanged("Path");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.AppPool Pool {
            get {
                return this.PoolField;
            }
            set {
                if ((object.ReferenceEquals(this.PoolField, value) != true)) {
                    this.PoolField = value;
                    this.RaisePropertyChanged("Pool");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AppPool", Namespace="http://schemas.datacontract.org/2004/07/WebSiteManagment.Models")]
    [System.SerializableAttribute()]
    public partial class AppPool : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string State {
            get {
                return this.StateField;
            }
            set {
                if ((object.ReferenceEquals(this.StateField, value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WebSiteRepositoryService.IWebSiteRepositoryService")]
    public interface IWebSiteRepositoryService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetAllSites", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetAllSitesResponse")]
        System.Collections.Generic.List<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site> GetAllSites();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetAllSites", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetAllSitesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site>> GetAllSitesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetSiteResponse")]
        IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site GetSite(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/GetSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/GetSiteResponse")]
        System.Threading.Tasks.Task<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site> GetSiteAsync(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/FindSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/FindSiteResponse")]
        IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site FindSite(long id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebSiteRepositoryService/FindSite", ReplyAction="http://tempuri.org/IWebSiteRepositoryService/FindSiteResponse")]
        System.Threading.Tasks.Task<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site> FindSiteAsync(long id);
        
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
        
        public System.Collections.Generic.List<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site> GetAllSites() {
            return base.Channel.GetAllSites();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site>> GetAllSitesAsync() {
            return base.Channel.GetAllSitesAsync();
        }
        
        public IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site GetSite(long id) {
            return base.Channel.GetSite(id);
        }
        
        public System.Threading.Tasks.Task<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site> GetSiteAsync(long id) {
            return base.Channel.GetSiteAsync(id);
        }
        
        public IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site FindSite(long id) {
            return base.Channel.FindSite(id);
        }
        
        public System.Threading.Tasks.Task<IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService.Site> FindSiteAsync(long id) {
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
    }
}
