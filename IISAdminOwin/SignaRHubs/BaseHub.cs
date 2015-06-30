using System;
using System.Security.Principal;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace IISAdmin.Owin.SignaRHubs
{
	public abstract class BaseHub<T> : Hub where T : IHub
	{
		private static readonly Lazy<IHubContext> LazyHubContext = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<T>());

		public static IHubContext HubContext
		{
			get
			{
				return LazyHubContext.Value;
			}
		}

		public static dynamic GetClientByUser(IPrincipal user)
		{
			return HubContext.Clients.User(user.Identity.Name);
		}
	}
}