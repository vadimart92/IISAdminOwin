define ["./module", "Hub", "$" , "utils", "jsFace"], (controllers, Hub, $, utils) ->
	controller = ($rootScope, $scope, $timeout) ->
		Site = Class.create(
			name: null,
			workUri: null,
			db: null,
			msSqlInstances: [],
			releaseInfo: {},
			isFormDataValid: () ->
				this.name && this.workUri && this.db
		)
		vm = $scope
		vm.site = new Site();

		vm.hub = new Hub("SiteCreateHub",
			logging: on
			listeners: []
			methods: [
				"AddSite"
				"GetReleaseInfo"
				"GetStartupInfo"
			]
		)

		vm.releaseInfoFields = [
			{ key: "createdOn", type: "input", templateOptions: {label: "Created on", disabled: on }}
			{ key: "name", type: "input", templateOptions: {label: "Build name", disabled: on }}
			{ key: "zipFilePath", type: "input", templateOptions: {label: "Zip file path", disabled: on }}
			{ key: "version", type: "input", templateOptions: {label: "Version", disabled: on }}
			{ key: "release", type: "checkbox", templateOptions: {label: "Release", disabled: on }}
		]

		vm.siteFields = [
			{
				key: "workUri",
				type: "input",
				templateOptions: {
					label: "Build uri/id",
					placeholder: "Paste product build uri here"
					required: true
				}
				validators: uri:
					expression: (viewValue, modelValue) ->
						utils.containsGUID modelValue or viewValue
					message: '$viewValue + " is not a valid build URI"'
				watcher: listener: (field, newValue, oldValue, scope, stopWatching) ->
					vm.site.releaseInfo = {}
					if newValue
						vm.updateReleaseInfo newValue
					return
			},
			{ key: "name", type: "input", templateOptions: {label: "Name" }}
		{ key: "webAppDir", type: "input", templateOptions: {label: "Web app directory" }}
		{
		key: "db", type: "uiSelect",
		templateOptions: {
			label: "MSSQL Instance"
		}
		}
		]

		vm.setSqlInstances = (sqlInstances)->
			vm.site.msSqlInstances = sqlInstances
			return

		getSiteCreateInfo = ()->
			vm.hub.GetStartupInfo()
			.then (siteInfo)->
				vm.$apply ()->
					vm.setSqlInstances siteInfo.sqlServerInstances
					return

		vm.updateReleaseInfo = (uri)->
			vm.hub.GetReleaseInfo uri
			.then (data)-> vm.$apply ()->
				$.extend vm.site.releaseInfo, data.release
				vm.site.name = data.webAppName
				vm.site.webAppDir = data.webAppDir
				return
			return

		vm.addSite = ()->
			vm.hub.AddSite vm.site
			return

		$timeout ()->
			do getSiteCreateInfo
			vm.site.workUri = "f63e0379-c338-4fe0-846e-ca088acdbb5d"
		,1000

		offFunc = $rootScope.$on "$stateChangeStart", (event, toState, toParams, fromState, fromParams) ->
			selfDestruct = offFunc;
			do vm.hub.disconnect
			do selfDestruct
			hideAllProgressBars true
			return
		return
	controller.$inject = ["$rootScope", "$scope", "$timeout"]
	controllers.controller "addSiteController", [controller]
	return