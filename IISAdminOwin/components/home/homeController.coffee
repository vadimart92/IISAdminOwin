# CoffeeScript
app.controller "homeController", ['$scope', '$state', ($scope, $state) ->
	vm = $scope
	vm.goTo = (location)->
		$state.go location
		return
	return
]
