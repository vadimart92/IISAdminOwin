define(["angularAMD", "ui_router", "ui_router_styles"], function(angularAMD) {
  return {
    config: function(app) {
      return app.config(function($stateProvider, $urlRouterProvider) {
        $stateProvider.state("siteList", angularAMD.route({
          url: "/siteList",
          templateUrl: "/app/views/siteList/siteList.html",
          controllerUrl: 'siteList/siteListController',
          data: {
            css: "app/components/siteList/siteList.css"
          }
        }));
        $stateProvider.state("appList", angularAMD.route({
          url: "/appList",
          templateUrl: "/app/views/appList/appList.html",
          controllerUrl: 'appList/appListController',
          data: {
            css: "app/components/appList/appList.css"
          }
        }));
        $stateProvider.state("addSite", angularAMD.route({
          url: "/addSite",
          templateUrl: "/app/views/addSite.html",
          controllerUrl: "addSite/addSiteController",
          data: {
            css: "app/components/addSite/addSite.css"
          }
        }));
        $stateProvider.state("addSiteProgress", angularAMD.route({
          url: "/addSiteProgress",
          templateUrl: "/app/views/addSiteProgress/addSiteProgress.html",
          controllerUrl: "addSiteProgress/addSiteProgressController",
          data: {
            css: "app/components/addSite/addSiteProgress/addSiteProgress.css"
          }
        }));
        $stateProvider.state("home", angularAMD.route({
          url: "/home",
          templateUrl: "/app/views/home.html",
          controllerUrl: "home/homeController",
          data: {
            css: "app/components/home/home.css"
          }
        }));
        return $urlRouterProvider.otherwise('/home');
      });
    }
  };
});
