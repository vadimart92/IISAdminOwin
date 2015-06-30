using RedisBoost;

namespace WebSiteManagment.Core.Common
{
	public static class RedisUtils
	{
		public static void FlushRedisDb(string host, int port, int dbIndex) {
			var connectionTask = RedisClient.ConnectAsync(host, port, dbIndex);
			connectionTask.Wait();
			IRedisClient client = connectionTask.Result;
			client.FlushDbAsync().Wait();
			client.Dispose();
		}
	}
}