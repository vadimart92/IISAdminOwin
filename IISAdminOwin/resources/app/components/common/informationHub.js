define(["common", "hub", "toaster", "jquery"], function(common, Hub, toaster, $) {
  var InformationHub;
  InformationHub = Class(common["class"].BaseClass, {
    constructor: function() {
      InformationHub.$super.constructor.call(this);
      return this.init();
    },
    init: function() {
      this.initToasterOptions();
      return this.hub.connect();
    },
    hub: new Hub("Information", {
      connectionName: "information",
      logging: true,
      listeners: {
        "info": function(data) {
          common.logger.info(data.msg, data.title);
          return toaster.info(data.msg, data.title, this.hubOptions);
        },
        "success": function(data) {
          common.logger.success(data.msg, data.title);
          return toaster.success(data.msg, data.title, this.hubOptions);
        },
        "warning": function(data) {
          common.logger.warning(data.msg, data.title);
          return toaster.warning(data.msg, data.title, $.extend(this.hubOptions, {
            timeOut: 10000
          }));
        },
        "error": function(data) {
          common.logger.error(data.msg, data.title);
          return toaster.error(data.msg, data.title, $.extend(this.hubOptions, {
            timeOut: 15000
          }));
        }
      },
      methods: []
    }),
    hubOptions: {
      "positionClass": "toast-bottom-right",
      "timeOut": 5000,
      "preventDuplicates": false
    },
    initToasterOptions: function() {
      return toaster.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "200",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
      };
    }
  });
  return new InformationHub();
});
