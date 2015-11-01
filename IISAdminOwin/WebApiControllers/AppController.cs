using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Breeze.WebApi2;
using IISAdmin.Interfaces;
using IISAdmin.Owin.SignaRHubs;

namespace IISAdmin.Owin.WebApiControllers
{
	[BreezeController]
	public class AppController : ApiController
	{
		private readonly IWebSiteRepository _webSiteRepository;

		public AppController(IWebSiteRepository webSiteRepository) {
			if (webSiteRepository == null) {
				throw new ArgumentNullException(nameof(webSiteRepository));
			}
			_webSiteRepository = webSiteRepository;
		}

		// GET ~/breeze/App/AppList
		[HttpGet]
		public IEnumerable<IApplication> AppList() {
			try {
			    var apps = _webSiteRepository.GetAllApplications();
                var result = apps.OrderBy(app => app.Name, StringComparer.OrdinalIgnoreCase).ToList();
			    return result;
			} catch (UnauthorizedAccessException) {
				Information.ErrorFormat(HttpContext.Current.User, "Error", "Site management service must run in admin mode");
			} catch (Exception exception) {
				Information.ErrorFormat(HttpContext.Current.User, "Error", "{0}", exception.Message);
			}
			return null;
		}
	}
}