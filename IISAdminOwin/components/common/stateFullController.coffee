define ["./baseController"], (BaseController)->
	StateFullController = Class(BaseController,
		$scope: null
		$rootScope: null
		constructor: ($scope, $rootScope) ->
			this.$rootScope = $rootScope
			StateFullController.$super.call this, $scope
			return
		defineListeners: ->
			StateFullController.$superp.defineListeners.call this
			return
		defineScope: ->
			StateFullController.$superp.defineScope.call this
			onStateChange = this.$rootScope.$on "$stateChangeStart", ((event, toState, toParams, fromState, fromParams) ->
				do this.onStateChangeStart
				return
			).bind(this)
			this.listeners.push(onStateChange)
			return
		destroy: () ->
			StateFullController.$superp.destroy.call this
			_.each this.listeners, (listener)-> do listener
			return
		onStateChangeStart: ()->
			return
	)
	StateFullController.$inject = [ "$scope" ]
	StateFullController