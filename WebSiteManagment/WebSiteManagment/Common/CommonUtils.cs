using System.Net;
using System.Net.Sockets;

namespace WebSiteManagment.Core.Common {

	public static class CommonUtils {

		public static int SafeGetFreeTcpPort() {
			int port = -1;
			for (int i = 0; i < 3; i++) {
				try {
					port = FreeTcpPort();
					i = 3;
				} catch {
				}
			}
			return port;
		}

		static int FreeTcpPort() {
			TcpListener l = new TcpListener(IPAddress.Loopback, 0);
			l.Start();
			int port = ((IPEndPoint)l.LocalEndpoint).Port;
			l.Stop();
			return port;
		}
	}
}
