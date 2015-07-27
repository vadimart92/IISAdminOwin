define ["app", "hub", "jquery" , "common"], (app, Hub, $, common)->
	Site = Class(
		name: null,
		workUri: null,
		db: null,
		msSqlInstances: [],
		releaseInfo: {},
		isFormDataValid: () ->
			this.name && this.workUri && this.db
	)
	AddSite = Class(common.class.StateFullController, {

		$state: null
		$timeout: null

		site: new Site()

		hub: null

		constructor: ($rootScope, $scope, $timeout, $state)->
			this.$state = $state
			AddSite.$super.call(this, $scope, $rootScope)
			do this.initHub

		initHub: ()->
			this.hub = new Hub("SiteCreateHub",
				useSharedConnection: off
				logging: on
				listeners: []
				methods: [
					"AddSite"
					"GetReleaseInfo"
					"GetStartupInfo"
				],
				this.bind ->
					do this.getSiteCreateInfo
					this.site.workUri = "f63e0379-c338-4fe0-846e-ca088acdbb5d"
			)
			return

		onWorkUriChange: (field, newValue) ->
			this.site.releaseInfo = {}
			if newValue
				this.updateReleaseInfo newValue
			return

		apply: ->
			do this.$scope.$apply

		defineScope: ()->
			AddSite.$superp.defineScope.call(this)
			do this.initSiteFields
			do this.initReleaseInfoFields
			this.$scope.site = this.site
			this.$scope.addSite = this.bind this.addSite

		onStateChangeStart: ()->
			AddSite.$superp.onStateChangeStart.call(this)
			do this.hub.disconnect
			this.$scope.hideAllProgressBars = true

		setSqlInstances: (sqlInstances)->
			this.site.msSqlInstances = sqlInstances
			return

		getSiteCreateInfo: ()->
			me = this
			this.hub.GetStartupInfo()
			.then (siteInfo)->
				me.$scope.$apply ()->
					me.setSqlInstances siteInfo.sqlServerInstances
					return

		updateReleaseInfo: (uri)->
			this.hub.GetReleaseInfo uri
			.then this.bind (data)->
				$.extend this.site.releaseInfo, data.release
				this.site.name = data.webAppName
				this.site.webAppDir = data.webAppDir
				do this.apply
			return

		addSite: ->
			this.hub.AddSite(this.site).then(=>this.$state.go("addSiteProgress"))
			return

		initSiteFields: ->
			this.$scope.siteFields = [
				{
					key: "workUri",
					type: "input",
					templateOptions: {
						label: "Build uri/id",
						placeholder: "Paste product build uri here"
						required: true
					}
					validators: uri:
						expression: (viewValue, modelValue) -> common.utils.containsGUID modelValue or viewValue
						message: '$viewValue + " is not a valid build URI"'
					watcher: listener: this.onWorkUriChange.bind this
				},
				{ key: "name", type: "input", templateOptions: {label: "Name" }}
				{ key: "webAppDir", type: "input", templateOptions: {label: "Web app directory" }}
				{ key: "db",  type: "uiSelect", templateOptions: { label: "MSSQL Instance" } }
			]
			return

		initReleaseInfoFields: ->
			this.$scope.releaseInfoFields = [
				{ key: "createdOn", type: "input", templateOptions: {label: "Created on", disabled: on }}
				{ key: "name", type: "input", templateOptions: {label: "Build name", disabled: on }}
				{ key: "zipFilePath", type: "input", templateOptions: {label: "Zip file path", disabled: on }}
				{ key: "version", type: "input", templateOptions: {label: "Version", disabled: on }}
				{ key: "release", type: "checkbox", templateOptions: {label: "Release", disabled: on }}
			]
	})
	["$rootScope", "$scope", "$timeout", "$state", ($rootScope, $scope, $timeout,$state)-> return new AddSite($rootScope, $scope, $timeout, $state)]