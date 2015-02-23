app.service("toasterService", [
  'toaster', 'Hub', '$', function(toaster, Hub, $) {
    var hub, hubOptions;
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
    hubOptions = {
      "positionClass": "toast-bottom-right",
      "timeOut": 5000,
      "preventDuplicates": false
    };
    hub = new Hub('Information', {
      useSharedConnection: false,
      methods: ["GetDate"],
      listeners: {
        'info': function(msgData) {
          toaster.info(msgData.msg, msgData.title, hubOptions);
        },
        'success': function(msgData) {
          toaster.success(msgData.msg, msgData.title, hubOptions);
        },
        'warning': function(msgData) {
          toaster.warning(msgData.msg, msgData.title, $.extend(hubOptions, {
            timeOut: 10000
          }));
        },
        'error': function(msgData) {
          toaster.error(msgData.msg, msgData.title, $.extend(hubOptions, {
            timeOut: 15000
          }));
        }
      }
    });
    hub.connect();
    this.getInfoHub = function() {
      return hub;
    };
    this.getDate = function(callBack) {
      return hub.GetDate();
    };
    this.showInfo = function(text, title) {
      return toaster.info(text, title);
    };
  }
]);
