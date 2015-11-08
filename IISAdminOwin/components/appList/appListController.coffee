define [
	"app"
	"../dataService/appDataService"
	"appList/application"
	"common"
	"hub"
	"jquery"
	"text!templates/appList/appList.columns.pool.html"
	"text!templates/appList/appList.columns.redis.html"
], (app, AppDataService, ApplicationViewModel, common, Hub, $, poolTpl, redisTpl)->
		AppList = Class(common.class.StateFullController,

			uiGridConstants: null

			$timeout: null

			appListData: [],

			hub: null

			initHub: ()->
				this.hub = new Hub("SiteManagementHub",
					listeners:
						"updateAppState": (siteData) =>
							this.setAppState(siteData);
							this.$scope.$apply()
					methods: [
						"RestartPool"
						"StopPool"
						"StartPool"
						"FlushRedis"
					]
				)

			AppDataService: null

			initAppDataService: ->
				this.AppDataService = new AppDataService();
				do this.getList

			constructor: ($rootScope, $scope, $timeout, uiGridConstants) ->
				do this.initHub
				this.uiGridConstants = uiGridConstants
				this.$timeout = $timeout
				AppList.$super.call(this, $scope, $rootScope)
				do this.initAppDataService
				return

			defineScope: ->
				AppList.$superp.defineScope.call this
				do this.initGridOptions
				do this.initScope
				return

			initScope: ->
				this.listLoaded false
				return

			apply: ->
				do this.$scope.$apply

			onStateChangeStart: ()->
				AppList.$superp.onStateChangeStart.call(this)
				do this.hub.disconnect
				this.$scope.hideAllProgressBars = true
				return

			listLoaded: (value)->
				if this.$scope.listLoaded != value
					this.$scope.listLoaded = value

			getGridData: ()->
				return this.appListData

			setGridData: (data)->
				this.appListData = data
				this.$scope.gridOptions.data = this.appListData

			getList: ->
				querySucceeded = (data) =>
					this.setGridData _.map data, (row)=> new ApplicationViewModel row, this.hub, this
					return
				this.$timeout =>
					this.AppDataService.getApplications this.bind querySucceeded, ->
						common.logger.error "Web sites loading error"
						return
					.finally =>
						this.listLoaded true
						return
					return
				, 0
				return

			setAppState: (siteData)->
				data = this.getGridData();
				site = _.find data, (row)-> row.id == siteData.id
				if (site)
					site.setState siteData.newData

			hideAllProgressBars: (force)->
				data = do this.getGridData
				_.each data, (row)=>
					this.stopRowProgressBar row, force
				return

			initGridOptions: ->
				this.$scope.gridOptions = {
					enableScrollbars: 0
					enableColumnResizing: on
					enableRowSelection: off
					enableRowHeaderSelection: off
					multiSelect: off
					enableFiltering: on
					enableGridMenu: on
					columnDefs: [
						{field: "id", displayName: "Id", width: "1", enableFiltering: false}
						{
							field: "name"
							displayName: "Name"
							minWidth: 50
							width: "30%"
							filter: {
								condition: this.uiGridConstants.filter.CONTAINS
								placeholder: "contains"
							}
						},
						{
							field: "pool"
							displayName: "App pool"
							width: "30%"
							cellTemplate: poolTpl
							enableFiltering: false
						}
						{
							field: "redis"
							displayName: "Redis"
							width: "10%"
							cellTemplate: redisTpl
							enableFiltering: false
						}
					]
				}
				this.setGridData []
		);
		["$rootScope", "$scope", "$timeout", "uiGridConstants", ($rootScope, $scope, $timeout, uiGridConstants)->
			return new AppList($rootScope, $scope, $timeout, uiGridConstants)
		]