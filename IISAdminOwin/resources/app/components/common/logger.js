app.factory('logger', [
  '$log', 'toaster', function($log, toaster) {
    var error, info, log, logger, success, warning;
    error = function(message, title) {
      toaster.error(message, title);
      $log.error('Error: ' + message);
    };
    info = function(message, title) {
      toaster.info(message, title);
      $log.info('Info: ' + message);
    };
    log = function(message) {
      $log.log(message);
    };
    success = function(message, title) {
      toaster.success(message, title);
      $log.info('Success: ' + message);
    };
    warning = function(message, title) {
      toaster.warning(message, title);
      $log.warn('Warning: ' + message);
    };
    toaster.options.timeOut = 2000;
    toaster.options.positionClass = 'toast-bottom-right';
    logger = {
      error: error,
      info: info,
      log: log,
      success: success,
      warning: warning
    };
    return logger;
  }
]);
