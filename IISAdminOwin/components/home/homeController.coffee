# CoffeeScript
define ["../common"], (common)->
	Home = Class(common.class.BaseController, {
		$state: null
		constructor: ($scope, $state)->
			Home.$super.call(this, $scope)
			this.$state = $state
		defineScope: ()->
			this.$scope.goTo = this.goTo.bind this
			return
		goTo: (location)->
			this.$state.go location
			return
	})
	['$scope', '$state', ($scope, $state)-> return new Home($scope, $state)]