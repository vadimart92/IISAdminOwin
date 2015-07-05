using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISAdmin.Interfaces
{
	interface ISiteDeployProvider
	{
		void DeployWebApp(ISiteCreateData siteCreateData);
	}
}
