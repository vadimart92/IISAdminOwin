require.config({
  paths: {
    jsFace: "../../assets/jsface/jsface",
    jsFaceProps: "../../assets/jsface/props.plugin",
    angular: "../../assets/angular/angular.1.4.3",
    angularAMD: "../../assets/angular/angularAMD",
    ngload: "../../assets/angular/ngload",
    bootstrap: "../../assets/bootstrap/bootstrap",
    underscore: "../../assets/underscore/underscore",
    signalR: "../../assets/signalR/jquery.signalR-2.2.0",
    toaster: "../../assets/toaster/toastr",
    mProgress: "../../assets/mProgress/mprogress",
    Q: "../../assets/q/q",
    ngSanitize: "../../assets/angular/angular-sanitize",
    ui_formly: "../../assets/angular/ui-formly",
    angular_formly_templates: "../../assets/angular/angular-formly-templates-bootstrap",
    ngAnimate: "../../assets/angular/angular-animate",
    ngResource: "../../assets/angular/angular-resource",
    ui_router: "../../assets/angular/angular-ui-router",
    ui_router_styles: "../../assets/angular/ui-router-styles",
    ui_bootstrap: "../../assets/angular/ui-bootstrap",
    ui_bootstrap_tpls: "../../assets/angular/ui-bootstrap-tpls",
    loading_bar: "../../assets/angular/loading-bar",
    ui_grid: "../../assets/ui-grid/ui-grid-stable",
    ui_select: "../../assets/ui-select/select",
    breeze: "../../assets/breeze/breeze.debug",
    breeze_ng: "../../assets/breeze/breeze.bridge.angular",
    breeze_ajax_angular: "../../assets/breeze/breeze.ajax.angular",
    breeze_dataService_webApi: "../../assets/breeze/breeze.dataService.webApi",
    breeze_savequeuing: "../../assets/breeze/breeze.savequeuing",
    jquery: "../../assets/jquery/jquery",
    hub: "common/signalRHub",
    templates: "../../app/views"
  },
  shim: {
    jsFace: {
      exports: "jsFace"
    },
    jsFaceProps: {
      deps: ["jsFace"]
    },
    underscore: {
      exports: '_'
    },
    angular: {
      exports: "angular",
      deps: ["jquery"]
    },
    angularAMD: ["angular"],
    ngload: ["angularAMD"],
    ngSanitize: {
      deps: ["angular"]
    },
    ui_formly: {
      deps: ["angular"]
    },
    angular_formly_templates: {
      deps: ["angular"]
    },
    ngAnimate: {
      deps: ["angular"]
    },
    ngResource: {
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
    breeze: {
      exports: "breeze",
      deps: ["Q"]
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
    breeze_ng: {
      deps: ["breeze", "breeze_ajax_angular", "breeze_dataService_webApi", "breeze_savequeuing"]
    }
  },
  deps: ["bootstrap", "text", "jsFaceProps", "app"]
});
