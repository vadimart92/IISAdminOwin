using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Breeze.WebApi2;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.WebApiControllers {

	[BreezeController]
	public class SiteController : ApiController {
		private readonly IWebSiteRepository _webSiteRepository;

		public SiteController(IWebSiteRepository webSiteRepository){
			if (webSiteRepository == null) {
				throw new ArgumentNullException("webSiteRepository");
			}
			_webSiteRepository = webSiteRepository;
		}

		// GET ~/breeze/Site/SiteList
		[HttpGet]
		public IEnumerable<ISite> SiteList() {
			var sites =  _webSiteRepository.GetAllSites()
				.OrderBy(s=>s.Name, StringComparer.OrdinalIgnoreCase);
			return sites;
		}
	}
}
