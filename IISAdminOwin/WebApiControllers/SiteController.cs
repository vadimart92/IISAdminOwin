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
	public class SiteController : ApiController
	{
		private readonly IWebSiteRepository _webSiteRepository;

		public SiteController(IWebSiteRepository webSiteRepository)
		{
			if (webSiteRepository == null)
			{
				throw new ArgumentNullException("webSiteRepository");
			}
			_webSiteRepository = webSiteRepository;
		}

		// GET ~/breeze/Site/SiteList
		[HttpGet]
		public IEnumerable<ISite> SiteList()
		{
			try
			{
				var sites = _webSiteRepository.GetAllSites()
					.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase);
				return sites;
			}
			catch (UnauthorizedAccessException)
			{
				Information.ErrorFormat(HttpContext.Current.User, "Error", "Site management service must run in admin mode");
			}
			catch (Exception exception)
			{
				Information.ErrorFormat(HttpContext.Current.User, "Error", "{0}", exception.Message);
			}
			return null;
		}
	}
}