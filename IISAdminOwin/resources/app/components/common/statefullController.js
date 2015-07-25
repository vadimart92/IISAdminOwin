define(["./baseController"], function(BaseController) {
  var StateFullController;
  StateFullController = Class(BaseController, {
    $scope: null,
    $rootScope: null,
    constructor: function($scope, $rootScope) {
      this.$rootScope = $rootScope;
      StateFullController.$super.call(this, $scope);
    },
    defineListeners: function() {
      StateFullController.$superp.defineListeners.call(this);
    },
    defineScope: function() {
      var onStateChange;
      StateFullController.$superp.defineScope.call(this);
      onStateChange = this.$rootScope.$on("$stateChangeStart", (function(event, toState, toParams, fromState, fromParams) {
        this.onStateChangeStart();
      }).bind(this));
      this.listeners.push(onStateChange);
    },
    destroy: function() {
      StateFullController.$superp.destroy.call(this);
      _.each(this.listeners, function(listener) {
        return listener();
      });
    },
    onStateChangeStart: function() {}
  });
  StateFullController.$inject = ["$scope"];
  return StateFullController;
});
