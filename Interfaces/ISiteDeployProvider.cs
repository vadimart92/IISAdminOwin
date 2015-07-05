using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IISAdmin.Interfaces
{
	public interface ISiteDeployProvider
	{
		void DeployWebApp(ISiteCreateData siteCreateData, IProgress<ISiteDeployProgress> progress, ISiteDeployProgress initialProgress);
		ISiteDeployProgress GetInitDeployProgress(IEnumerable<DeployOperationIfo> extraOperations);
	}

	public class DeployOperationIfo {
		public string Info { get; set; }
	}

	public interface ISiteDeployProgress {
		int TotalOperationsCount { get; }
		int CurrentOperationNumber { get; }
		DeployOperationIfo CurrentOperation { get; }
		decimal Percentage { get; }
		
		/// <summary>
		/// Set next operation as Active.
		/// </summary>
		/// <returns>True if next operation was set.
		/// If current operation is last returns false.
		/// </returns>
		bool SetNextOperation();
	}
}
