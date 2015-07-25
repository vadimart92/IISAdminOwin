define ["./baseClass"], (BaseClass)->
	BaseController = Class(BaseClass,
		$scope: null
		listeners: []
		constructor: ($scope) ->
			this.$scope = $scope
			do this.defineListeners
			do this.defineScope
			return
		defineListeners: ->
			this.$scope.$on "$destroy", this.bind this.destroy
			return
		defineScope: ->
			#OVERRIDE : Create all scope properties here to keep track of them.
			return
		destroy: (event) ->
			#OVERRIDE : Remove all listeners, all timeouts and intervals
			return
	)
	BaseController.$inject = [ "$scope" ]
	BaseController