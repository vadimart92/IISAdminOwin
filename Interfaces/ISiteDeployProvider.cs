using System;

namespace IISAdmin.Interfaces
{
    public interface ISiteDeployProvider {
		void DeployWebApp(SiteCreateData siteCreateData, Guid jobInfoId);
		void InitDeployInfo(SiteCreateData siteCreateData);
	    OperationInfoBase GetOperationsInfo();
	}
    
}
