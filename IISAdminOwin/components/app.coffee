# CoffeeScript
define ["require"
				"angular"
				"routes"
				"angularAMD"
				"jsFace"
				"ngResource"
				"ngAnimate"
				"ngSanitize"
				"ui_bootstrap"
				"ui_bootstrap_tpls"
				"ui_grid"
				"ui_select"
				"loading_bar"
				"ui_formly"
				"angular_formly_templates"
], (require, angular, routes, angularAMD)->
	#require [ "domReady!" ], (document) ->
	app = angular.module("IISAdmin", [
		"ui.router"
		"ngResource"
		"ui.bootstrap"
		"ui.grid"
		"ui.grid.autoResize"
		"ui.grid.resizeColumns"
		"ui.grid.selection"
		"angular-loading-bar"
		"ngAnimate"
		"uiRouterStyles"
		"ui.select"
		"ngSanitize"
		"formly"
		"formlyBootstrap"
	])
	routes.config(app);
	app.run (formlyConfig)->
		formlyConfig.setType {
			name: "uiSelect",
			templateUrl: "app/views/custom-formly-fields-ui-select.html"
		}
	return angularAMD.bootstrap app