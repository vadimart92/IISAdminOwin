app.factory('infoProvider', [
  'toaster', 'Hub', function(toaster, Hub) {
    var hub, logger;
    hub = new Hub('Information', {
      listeners: {
        'info': function(msgData) {
          toaster.info(msgData.msg, msgData.title);
        },
        'success': function(msgData) {
          debugger;
          toaster.success(msgData.msg, msgData.title);
        },
        'success': function(msgData) {
          toaster.warning(msgData.msg, msgData.title);
        },
        'success': function(msgData) {
          toaster.error(msgData.msg, msgData.title);
        }
      }
    });
    toaster.options.timeOut = 2000;
    toaster.options.positionClass = 'toast-bottom-right';
    logger = {
      hub: hub,
      toaster: toaster
    };
    return logger;
  }
]);
