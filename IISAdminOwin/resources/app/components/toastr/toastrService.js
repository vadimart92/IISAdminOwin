define(["toaster", "hub", "jquery"], function(toaster, Hub, $) {
  var Toaster;
  Toaster = Class({
    constructor: function() {
      Toaster.$super.constructor.call(this);
      return this.init();
    },
    getInfoHub: function() {
      return hub;
    },
    showInfo: function(text, title) {
      return toaster.info(text, title);
    },
    hub: new Hub('Information', {
      useSharedConnection: false,
      listeners: {
        'info': function(msgData) {
          toaster.info(msgData.msg, msgData.title, this.hubOptions);
        },
        'success': function(data) {
          toaster.success(data.msg, data.title, this.hubOptions);
        },
        'warning': function(data) {
          toaster.warning(data.msg, data.title, $.extend(this.hubOptions, {
            timeOut: 10000
          }));
        },
        'error': function(data) {
          toaster.error(data.msg, data.title, $.extend(this.hubOptions, {
            timeOut: 15000
          }));
        }
      }
    }),
    init: function() {
      toaster.options = {
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
      return this.hub.connect();
    }
  });
  return new Toaster();
});
