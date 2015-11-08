using System;

namespace IISAdmin.Interfaces
{
    #region SiteCreateData

    [Serializable]
    public class SiteCreateData {
        public virtual string Name { get; set; }
        public virtual string WebAppName { get; set; }
        public virtual Release ReleaseInfo { get; set; }
        public virtual SqlServerInstance Db { get; set; }
        public virtual Redis RedisInfo { get; set; }
        public virtual string DestinationWebAppRoot { get; set; }
        public virtual bool SeparateFolder { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool CreateNewSite { get; set; }
    }

    #endregion SiteCreateData
}
