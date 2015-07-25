define(["./baseClass"], function(BaseClass) {
  var BaseController;
  BaseController = Class(BaseClass, {
    $scope: null,
    listeners: [],
    constructor: function($scope) {
      this.$scope = $scope;
      this.defineListeners();
      this.defineScope();
    },
    defineListeners: function() {
      this.$scope.$on("$destroy", this.bind(this.destroy));
    },
    defineScope: function() {},
    destroy: function(event) {}
  });
  BaseController.$inject = ["$scope"];
  return BaseController;
});
