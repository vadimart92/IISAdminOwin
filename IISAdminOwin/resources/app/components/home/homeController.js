define(["../common"], function(common) {
  var Home;
  Home = Class(common["class"].BaseController, {
    $state: null,
    constructor: function($scope, $state) {
      Home.$super.call(this, $scope);
      return this.$state = $state;
    },
    defineScope: function() {
      this.$scope.goTo = this.goTo.bind(this);
    },
    goTo: function(location) {
      this.$state.go(location);
    }
  });
  return [
    '$scope', '$state', function($scope, $state) {
      return new Home($scope, $state);
    }
  ];
});
