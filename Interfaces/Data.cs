using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISAdmin.Interfaces
{
	#region ISiteCreateData

	public interface ISiteCreateData
	{
		string Name {
			get;
		}

		string RedisConnectionString {
			get;
		}

		IRelease ReleaseInfo {
			get;
		}

		ISqlServerInstance Db {
			get;
		}

		string DestinationPath { get; }
	}

	#endregion ISiteCreateData
}
