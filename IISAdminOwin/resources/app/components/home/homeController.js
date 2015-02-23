app.controller("homeController", [
  '$scope', '$state', function($scope, $state) {
    var vm;
    vm = $scope;
    vm.goTo = function(location) {
      $state.go(location);
    };
  }
]);
