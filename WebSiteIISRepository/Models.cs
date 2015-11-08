using System.Collections.Generic;
using System.Runtime.Serialization;
using AutoMapper;
using IISAdmin.Interfaces;
using Redis = IISAdmin.Interfaces.Redis;

namespace IISAdmin.WCFWebSiteRepository
{
    public static class Common {
        static Common () {
            Mapper.CreateMap<WebSiteManagment.Core.Models.Site, Site>();
            Mapper.CreateMap<WebSiteManagment.Core.Models.Application, Application>();
            Mapper.CreateMap<WebSiteManagment.Core.Models.AppPool, ApplicationPool>();
            Mapper.CreateMap<WebSiteManagment.Core.Models.Redis, Redis>();
        }

        public static Site Site(WebSiteManagment.Core.Models.Site site) {
            return Mapper.Map<Site>(site);
        }

        public static WebSiteManagment.Core.Models.Site Site(Site site) {
            return Mapper.Map<WebSiteManagment.Core.Models.Site>(site);
        }

        public static Application Application(WebSiteManagment.Core.Models.Application application) {
            return Mapper.Map<Application>(application);
        }

        public static WebSiteManagment.Core.Models.Application Application(Application application) {
            return Mapper.Map<WebSiteManagment.Core.Models.Application>(application);
        }
        
        public static Redis Redis(WebSiteManagment.Core.Models.Redis redis) {
            return Mapper.Map<Redis>(redis);
        }

        public static WebSiteManagment.Core.Models.Redis Redis(Redis redis) {
            return Mapper.Map<WebSiteManagment.Core.Models.Redis>(redis);
        }
        
        public static ApplicationPool ApplicationPool(WebSiteManagment.Core.Models.AppPool appPool) {
            return Mapper.Map<ApplicationPool>(appPool);
        }

        public static WebSiteManagment.Core.Models.AppPool ApplicationPool(ApplicationPool appPool) {
            return Mapper.Map<WebSiteManagment.Core.Models.AppPool>(appPool);
        }
    }
}