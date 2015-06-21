using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;

namespace IISAdminOwin.DI
{
	public class UnityHubActivator : IHubActivator
	{
		private readonly UnityContainer _container;

		public UnityHubActivator(UnityContainer container)
		{
			Debug.Assert(container != null, "container != null");
			_container = container;
		}

		#region Члены IDisposable

		public void Dispose()
		{
			_container.Dispose();
		}

		#endregion Члены IDisposable

		#region Члены IHubActivator

		public IHub Create(HubDescriptor descriptor)
		{
			if (descriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}

			if (descriptor.HubType == null)
			{
				return null;
			}

			var hub = _container.Resolve(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);
			return hub as IHub;
		}

		#endregion Члены IHubActivator
	}
}