using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.Common {

	public class SqlServerInstance : ISqlServerInstance {
		public string Name { get; set; }
		public string Version { get; set; }
	}

	public class LocalSqlServerInstanceRepository : ISqlServerInstanceRepository {
		#region Члены ISqlServerInstanceRepository

		public IList<ISqlServerInstance> GetAllInstances() {
			return new ISqlServerInstance [] {
				new SqlServerInstance{Name = "Tsc-distr ms 2008"},
				new SqlServerInstance{Name = "Tsc-distr ms 2012"}
			};
		}

		#endregion
	}
}
