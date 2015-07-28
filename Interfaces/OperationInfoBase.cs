using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace IISAdmin.Interfaces
{
    public enum OperationStageState { Pending=0, InProgress=1, Completed=2, Failed=-1}

    public class OperationInfoBase: IJobInfo {
        private Dictionary<string, OperationStageInfo> _stageInfos;
        private readonly IJobInfoRepository _jobInfoRepository;
        public OperationInfoBase(IJobInfoRepository jobInfoRepository) {
            _jobInfoRepository = jobInfoRepository;
            Init();
            //TODO: get job id
        }

        public event Action<IJobInfo> OnStateChanged; 

        private void Init() {
            _stageInfos = GetOperationStageInfos();
        }

        protected virtual Dictionary<string, OperationStageInfo> GetOperationStageInfos() {
             return new Dictionary<string, OperationStageInfo>();
        }

        protected void UpdateState(OperationStageState newValue, [CallerMemberName]string propertyName = null) {
            if (propertyName!=null && _stageInfos.ContainsKey(propertyName)) {
                var state = _stageInfos[propertyName];
                state.State = newValue;
                _jobInfoRepository.Update(Id, info => {
                    info.SerializedInfo = SerializedInfo;
                });
                OnStateChanged?.Invoke(this);
            }
            else {
                throw new InvalidOperationException(
                    $"Are you miss to register stage #{propertyName} in GetOperationStageInfos?");
            }
        }
        
        public Guid Id { get; set; }
        public int JobId { get; set; }

        public string SerializedInfo {
            get { return JsonConvert.SerializeObject(_stageInfos.Values); }
            set { throw new InvalidOperationException(); }
        }
    }

    public class OperationStageInfo {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OperationStageState State { get; set; }
    }
}