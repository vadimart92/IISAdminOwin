namespace IISAdmin.Interfaces
{
    public interface ISiteDeployProvider {
		void DeployWebApp(ISiteCreateData siteCreateData, OperationInfoBase deploySiteInfo);
		void InitDeployInfo(ISiteCreateData siteCreateData);
	    OperationInfoBase GetOperationsInfo(IJobInfoRepository jobInfoRepository);
	}

	public class DeployOperationIfo {
		public string Info { get; set; }
	}

    public interface IDeploySiteInfo
    {
        OperationStageState RestoreDbCopyFiles { get; set; }
        OperationStageState CreateWebApp { get; set; }
        OperationStageState ModifyConfigs { get; set; }
    }
}
