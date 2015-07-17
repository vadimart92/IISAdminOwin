require.config({
  paths: {
    jsFace: "../../assets/jsface/jsface",
    angular: "../../assets/angular/angular",
    bootstrap: "../../assets/bootstrap/bootstrap",
    underscore: "../../assets/underscore/underscore",
    signalR: "../../assets/signalR/jquery.signalR-2.2.0",
    toaster: "../../assets/toaster/toastr",
    mProgress: "../../assets/mProgress/mprogress",
    q: "../../assets/q/q",
    angular_sanitize: "../../assets/angular/angular-sanitize",
    ui_formly: "../../assets/angular/ui-formly",
    angular_formly_templates: "../../assets/angular/angular-formly-templates-bootstrap",
    angular_animate: "../../assets/angular/angular-animate",
    angular_resource: "../../assets/angular/angular-resource",
    ui_router: "../../assets/angular/angular-ui-router",
    ui_router_styles: "../../assets/angular/ui-router-styles",
    ui_bootstrap: "../../assets/angular/ui-bootstrap",
    ui_bootstrap_tpls: "../../assets/angular/ui-bootstrap-tpls",
    loading_bar: "../../assets/angular/loading-bar",
    ui_grid: "../../assets/ui-grid/ui-grid-stable",
    ui_select: "../../assets/ui-select/select",
    breeze: "../../assets/breeze/breeze.debug",
    breeze_bridge_angular: "../../assets/breeze/breeze.bridge.angular",
    breeze_ajax_angular: "../../assets/breeze/breeze.ajax.angular",
    breeze_dataService_webApi: "../../assets/breeze/breeze.dataService.webApi",
    breeze_savequeuing: "../../assets/breeze/breeze.savequeuing",
    jquery: "../../assets/jquery/jquery"
  },
  shim: {
    jsFace: {
      exports: "jsFace"
    },
    underscore: {
      exports: '_'
    },
    angular: {
      exports: "angular",
      deps: ["jquery"]
    },
    angular_sanitize: {
      deps: ["angular"]
    },
    ui_formly: {
      deps: ["angular"]
    },
    angular_formly_templates: {
      deps: ["angular"]
    },
    angular_animate: {
      deps: ["angular"]
    },
    angular_resource: {
      deps: ["angular"]
    },
    ui_router: {
      deps: ["angular"]
    },
    ui_router_styles: {
      deps: ["angular"]
    },
    ui_bootstrap: {
      deps: ["angular"]
    },
    ui_bootstrap_tpls: {
      deps: ["angular"]
    },
    loading_bar: {
      deps: ["angular"]
    },
    ui_grid: {
      deps: ["angular"]
    },
    ui_select: {
      deps: ["angular"]
    },
    breeze_ajax_angular: {
      deps: ["breeze"]
    },
    breeze_dataService_webApi: {
      deps: ["breeze"]
    },
    breeze_savequeuing: {
      deps: ["breeze"]
    },
    breeze_bridge_angular: {
      deps: ["breeze", "breeze_ajax_angular", "breeze_dataService_webApi", "breeze_savequeuing"]
    }
  }
});

define(["require", "angular", "jsFace", "bootstrap", "underscore", "angular_animate", "angular_resource", "angular_sanitize", "app", "routes"], function(require, ng) {
  require(["domReady!"], function(document) {
    ng.bootstrap(document, ['app']);
  });
});
