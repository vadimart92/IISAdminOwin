using System.Collections.Generic;
using IISAdmin.Interfaces;

namespace IISAdmin.WebSiteManagmentProvider {

    public class DeploySiteInfo: OperationInfoBase, IDeploySiteInfo {
        private OperationStageState _restoreDbCopyFiles;
        private OperationStageState _createWebApp;
        private OperationStageState _modifyConfigs;

        public DeploySiteInfo(IJobInfoRepository jobInfoRepository) : base(jobInfoRepository) {}

        public OperationStageState RestoreDbCopyFiles
        {
            get { return _restoreDbCopyFiles; }
            set {
                _restoreDbCopyFiles = value;
                UpdateState(value);
            }
        }
        public OperationStageState CreateWebApp
        {
            get { return _createWebApp; }
            set {
                _createWebApp = value;
                UpdateState(value);
            }
        }

        public OperationStageState ModifyConfigs
        {
            get { return _modifyConfigs; }
            set {
                _modifyConfigs = value;
                UpdateState(value);
            }
        }

        protected override Dictionary<string, OperationStageInfo> GetOperationStageInfos() {
            var baseInfos = base.GetOperationStageInfos();
            baseInfos["RestoreDbCopyFiles"] = new OperationStageInfo {
                Number = 1,
                Name = "Restore DB / Copy files",
                State = OperationStageState.Pending,
                Description = "Restoring database, unzipping web application files to server directory."
            };
            baseInfos["CreateWebApp"] = new OperationStageInfo {
                Number = 2,
                Name = "Creating Pool/WebApp in IIS",
                State = OperationStageState.Pending,
                Description = "Creating web application on Internet Information Server."
            };
            baseInfos["ModifyConfigs"] = new OperationStageInfo {
                Number = 3,
                Name = "Modifying web. app. configs",
                State = OperationStageState.Pending,
                Description = "Connection strings, web.config"
            };
            return baseInfos;
        }
    }
}