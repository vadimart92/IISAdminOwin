using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IISAdmin.Interfaces;

namespace IISAdmin.WebSiteManagmentProvider {

    public class DeploySiteInfo {
        private OperationStageState _restoreDbCopyFiles;
        private OperationStageState _createWebApp;
        private OperationStageState _modifyConfigs;

        private readonly OperationInfoBase _operationInfo;

        public DeploySiteInfo(IJobInfoRepository jobInfoRepository, IHubContextProvider hubContextProvider, Guid? id = null, string jobId = null) {
            if (id == null) {
                _operationInfo = new OperationInfoBase(jobInfoRepository, jobId);
                _operationInfo.InitStageInfos(GetOperationStageInfos());
            } else {
                _operationInfo = jobInfoRepository.Get(id.Value);
            }
            _operationInfo.SetDependencies(jobInfoRepository, hubContextProvider);
        }

        public OperationInfoBase Instance => _operationInfo;

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

        void UpdateState(OperationStageState newValue, [CallerMemberName]string propertyName = null) {
            _operationInfo.UpdateState(newValue, propertyName);
        }

        private Dictionary<string, OperationStageInfo> GetOperationStageInfos() {
            var baseInfos = new Dictionary<string, OperationStageInfo>();
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