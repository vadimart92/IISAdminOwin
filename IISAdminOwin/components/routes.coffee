# CoffeeScript
define ["app"
	"ui_router"
	"ui_router_styles"
	], (app)->
		app.config ($stateProvider, $urlRouterProvider) ->
			$stateProvider.state "siteList",
				url: "/siteList",
				templateUrl: "/app/views/siteList.html"
				controller: "siteListController"
				data: {css: "app/components/siteList/siteList.css"}

			$stateProvider.state "addSite",
				url: "/addSite",
				templateUrl: "/app/views/addSite.html"
				controller: "addSiteController"
				data: {css: "app/components/addSite/addSite.css"}

			$stateProvider.state "addSite.progress",
				url: "/addSite",
				templateUrl: "/app/views/addSite.progress.html"
				controller: "addSite.progress.Controller"
				data: {css: "app/components/addSite/addSite.progress.css"}

			$stateProvider.state "home",
				url: "/home",
				templateUrl: "/app/views/home.html"
				controller: "homeController"
				data: {css: "app/components/home/home.css"}

			$urlRouterProvider.otherwise('/home');
			return
		app.run (formlyConfig)->
			formlyConfig.setType {
				name: 'uiSelect',
				templateUrl: 'app/views/custom-formly-fields-ui-select.html'
			}
		return app