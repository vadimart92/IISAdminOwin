using System;
using System.Security.Principal;

namespace IISAdmin.Owin.SignaRHubs
{
	public enum InformationType
	{
		Info, Success, Warning, Error
	}

	public class Information : BaseHub<Information>
	{

		public static void InfoFormat(string title, string format, params object[] values) {
			InnerFormat(HubContext.Clients.All, InformationType.Info, format, title, values);
		}

		public static void InfoFormat(IPrincipal user, string title, string format, params object[] values) {
			var client = GetClientByUser(user);
			InnerFormat(client, InformationType.Info, format, title, values);
		}

		public static void SuccessFormat(string title, string format, params object[] values) {
			InnerFormat(HubContext.Clients.All, InformationType.Success, format, title, values);
		}

		public static void SuccessFormat(IPrincipal user, string title, string format, params object[] values) {
			var client = GetClientByUser(user);
			InnerFormat(client, InformationType.Success, format, title, values);
		}

		public static void WarningFormat(string title, string format, params object[] values) {
			InnerFormat(HubContext.Clients.All, InformationType.Warning, format, title, values);
		}

		public static void WarningFormat(IPrincipal user, string title, string format, params object[] values) {
			var client = GetClientByUser(user);
			InnerFormat(client, InformationType.Warning, format, title, values);
		}

		public static void ErrorFormat(string title, string format, params object[] values) {
			InnerFormat(HubContext.Clients.All, InformationType.Error, format, title, values);
		}

		public static void ErrorFormat(IPrincipal user, string title, string format, params object[] values) {
			var client = GetClientByUser(user);
			InnerFormat(client, InformationType.Error, format, title, values);
		}

		private static void InnerFormat(dynamic client, InformationType type, string format, string title = null, params object[] values) {
			var data = new {
				title,
				msg = string.Format(format, values)
			};
			switch (type) {
				case InformationType.Info:
					client.info(data);
					break;

				case InformationType.Success:
					client.success(data);
					break;

				case InformationType.Warning:
					client.warning(data);
					break;

				case InformationType.Error:
					client.error(data);
					break;
			}
		}
	}
}