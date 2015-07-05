app.factory('Hub', [
  '$rootScope', 'Hub', 'logger', function($rootScope, Hub, logger) {
    var getState, hub;
    hub = new Hub('SiteManagementHub', {
      listeners: {
        'updateSiteState': function(siteData) {
          debugger;
          $rootScope.$apply();
        }
      },
      methods: ['GetRestartingState'],
      errorHandler: function(error) {
        logger.error(error);
      }
    });
    getState = function(siteId) {
      hub.GetRestartingState();
    };
    return {
      getState: getState
    };
  }
]);
