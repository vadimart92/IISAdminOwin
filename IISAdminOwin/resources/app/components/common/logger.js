define(["toaster", "app", "angular"], function(toaster, app, angular) {
  var $injector, Logger, ngLog;
  Logger = Class({
    toaster: null,
    $log: null,
    constructor: function($log, toaster) {
      this.toaster = toaster;
      this.initToaster();
    },
    initToaster: function() {
      toaster.options.timeOut = 2000;
      return toaster.options.positionClass = 'toast-bottom-right';
    },
    error: function(message, title) {
      this.toaster.error(message, title);
      this.$log.error('Error: ' + message);
    },
    info: function(message, title) {
      this.toaster.info(message, title);
      this.$log.info('Info: ' + message);
    },
    log: function(message) {
      this.$log.log(message);
    },
    success: function(message, title) {
      this.toaster.success(message, title);
      this.$log.info('Success: ' + message);
    },
    warning: function(message, title) {
      this.toaster.warning(message, title);
      this.$log.warn('Warning: ' + message);
    }
  });
  $injector = angular.injector(["ng"]);
  ngLog = $injector.get("$log");
  return new Logger(ngLog, toaster);
});
