var app;

app = angular.module("IISAdmin", ["ui.router", "ngResource", "ui.bootstrap", "ui.grid", "ui.grid.autoResize", "ui.grid.resizeColumns", "ui.grid.selection", "breeze.angular", "angular-loading-bar", "ngAnimate", "uiRouterStyles", "ui.select", "ngSanitize", "formly", "formlyBootstrap"]);

app.constant('toaster', toastr);

app.constant('$', jQuery);

app.config(function($stateProvider, $urlRouterProvider) {
  $stateProvider.state("siteList", {
    url: "/siteList",
    templateUrl: "/app/views/siteList.html",
    controller: "siteListController",
    data: {
      css: "app/components/siteList/siteList.css"
    }
  });
  $stateProvider.state("addSite", {
    url: "/addSite",
    templateUrl: "/app/views/addSite.html",
    controller: "addSiteController",
    data: {
      css: "app/components/addSite/addSite.css"
    }
  });
  $stateProvider.state("home", {
    url: "/home",
    templateUrl: "/app/views/home.html",
    controller: "homeController",
    data: {
      css: "app/components/home/home.css"
    }
  });
  $urlRouterProvider.otherwise('/home');
});

app.run(function(formlyConfig) {
  return formlyConfig.setType({
    name: 'uiSelect',
    templateUrl: 'app/views/custom-formly-fields-ui-select.html'
  });
});
