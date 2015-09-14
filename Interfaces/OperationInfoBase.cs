using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using IISAdmin.Interfaces.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace IISAdmin.Interfaces
{
    public enum OperationStageState { Pending=0, InProgress=1, Completed=2, Failed=-1}

    [Serializable]
    public class OperationInfoBase:IEntity<Guid> {
        private Dictionary<string, OperationStageInfo> _stageInfos;
        private IJobInfoRepository _jobInfoRepository;
        private IHubContextProvider _hubContextProvider;

        public OperationInfoBase() {
            InitStageInfos();
        }

        public virtual void Save() {
            _jobInfoRepository.Create(this);
        }

        public void SetDependencies(IJobInfoRepository jobInfoRepository, IHubContextProvider hubContextProvider)  {
            _jobInfoRepository = jobInfoRepository;
            _hubContextProvider = hubContextProvider;
        }

        public OperationInfoBase(IJobInfoRepository jobInfoRepository, string jobId) {
            _jobInfoRepository = jobInfoRepository;
            Id = Guid.NewGuid();
            JobId = jobId;
            InitStageInfos();
        }

        public event Action<IHubContext, OperationInfoBase> OnStateChanged;
        private SerializeDelegate _onStateChangedDelegates;
        private string _signarRHubName;

        public string SignarRHubName {
            get { return _signarRHubName; }
            set {
                _signarRHubName = value;
            }
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (OnStateChanged != null) {
                _onStateChangedDelegates = new SerializeDelegate(OnStateChanged);
            }
        }

        [OnDeserialized]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            if (_onStateChangedDelegates != null) {
                OnStateChanged += (Action<IHubContext, OperationInfoBase>)_onStateChangedDelegates.Delegate;
            }
        }

        public void InitStageInfos(Dictionary<string, OperationStageInfo> stageInfos = null) {
            _stageInfos = stageInfos ?? GetOperationStageInfos();
            foreach (var info in _stageInfos) {
                info.Value.PropertyName = info.Key;
            }
        }

        protected virtual Dictionary<string, OperationStageInfo> GetOperationStageInfos() {
             return new Dictionary<string, OperationStageInfo>();
        }

        public OperationStageInfo this[string propertyName] => _stageInfos[propertyName];

        public virtual void UpdateState(OperationStageState newValue, [CallerMemberName]string propertyName = null) {
            if (propertyName!=null && _stageInfos.ContainsKey(propertyName)) {
                var state = _stageInfos[propertyName];
                state.State = newValue;
                _jobInfoRepository.Update(Id, info => {
                    info.SerializedInfo = SerializedInfo;
                });
                if (!string.IsNullOrWhiteSpace(SignarRHubName)) {
                    var hubContext = _hubContextProvider.GetConnectionContext(SignarRHubName);;
                    OnStateChanged?.Invoke(hubContext, this);
                }
            }
            else {
                throw new InvalidOperationException(
                    $"Are you miss to register stage #{propertyName} in GetOperationStageInfos?");
            }
        }
        
        public Guid Id { get; set; }
        public string JobId { get; set; }

        public string SerializedInfo {
            get {
                return JsonConvert.SerializeObject(_stageInfos.Values);
            }
            set {
                _stageInfos.Clear();
                var infos = JsonConvert.DeserializeObject<List<OperationStageInfo>>(value);
                foreach (var stageInfo in infos) {
                    _stageInfos[stageInfo.PropertyName] = stageInfo;
                }
            }
        }
    }

    [Serializable]
    public class OperationStageInfo {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OperationStageState State { get; set; }
        public string PropertyName { get; set; }
    }
}