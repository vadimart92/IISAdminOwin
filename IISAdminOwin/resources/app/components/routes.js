define(["angularAMD", "ui_router", "ui_router_styles"], function(angularAMD) {
  return {
    config: function(app) {
      return app.config(function($stateProvider, $urlRouterProvider) {
        $stateProvider.state("siteList", angularAMD.route({
          url: "/siteList",
          templateUrl: "/app/views/siteList.html",
          controllerUrl: 'siteList/siteListController',
          data: {
            css: "app/components/siteList/siteList.css"
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
        $stateProvider.state("addSite.progress", angularAMD.route({
          url: "/addSite",
          templateUrl: "/app/views/addSite.progress.html",
          controllerUrl: "addSite/addSite.progress.Controller",
          data: {
            css: "app/components/addSite/addSite.progress.css"
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
