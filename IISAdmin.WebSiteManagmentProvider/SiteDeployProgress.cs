﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using IISAdmin.Interfaces;

namespace IISAdmin.WebSiteManagmentProvider {
	[DataContract]
	public class SiteDeployProgress : ISiteDeployProgress {

		public SiteDeployProgress(IEnumerable<DeployOperationIfo> operations) {
			_operations = operations.ToList();
			TotalOperationsCount = _operations.Count;
			CurrentOperationNumber = 0;
		}

		#region Fields: Private

		private readonly List<DeployOperationIfo> _operations;

		#endregion

		#region ISiteDeployProgress Members

		[DataMember]
		public int TotalOperationsCount { get;private  set; }

		[DataMember]
		public int CurrentOperationNumber { get; private set; }

		[DataMember]
		public DeployOperationIfo CurrentOperation { get { return _operations[CurrentOperationNumber - 1]; } }

		[DataMember]
		public decimal Percentage
		{
			get { return TotalOperationsCount == 0 ? 0 : Math.Round(CurrentOperationNumber - 1*100m/TotalOperationsCount, 2); }
		}

		#region Methods:Public

		public bool SetNextOperation() {
			if (TotalOperationsCount <= CurrentOperationNumber) return false;
			CurrentOperationNumber++;
			return true;
		}

		#endregion

		#endregion
	}
}