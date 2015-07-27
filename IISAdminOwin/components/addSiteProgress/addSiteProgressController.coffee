define ["app", "hub", "jquery" , "common"], (app, Hub, $, common)->
	SiteInfo = Class(common.class.BaseClass,
		id: null
	)
	QueueSiteInfo = Class(common.class.BaseClass,
		id: null
	)
	AddSiteProgress = Class(common.class.StateFullController, {

		queue: []

		hub: null

		constructor: ($scope, $rootScope)->
			AddSiteProgress.$super.apply this, arguments
			do this.initHub

		initHub: ()->
			this.hub = new Hub("SiteCreateHub",
				useSharedConnection: off
				logging: on
				listeners: {
					"refreshDeployStatus": this.bind this.refreshDeployStatus
				}
				methods: [
					"CancelDeploy"
				],
				this.bind ->
					console.log
			)
			return

		apply: ->
			do this.$scope.$apply

		defineScope: ()->
			AddSiteProgress.$superp.defineScope.call(this)

		onStateChangeStart: ()->
			AddSiteProgress.$superp.onStateChangeStart.call(this)
			do this.hub.disconnect

		refreshDeployStatus: (data)->
			alert data

	})
	["$rootScope", "$scope", ($rootScope, $scope)-> return new AddSiteProgress($rootScope, $scope)]