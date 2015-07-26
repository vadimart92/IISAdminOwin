define [
	"app"
	"../dataService/siteDataService"
	"common"
	"hub"
	"siteList/site"
	"toaster"
	"jquery"
	"text!templates/siteList/siteList.columns.siteState.html"
	"text!templates/siteList/siteList.columns.pool.html"
	"text!templates/siteList/siteList.columns.redis.html"
	"text!templates/siteList/siteList.columns.bindings.html"
], (app, SiteDataService, common, Hub, Site, toaster, $, siteStateTpl, poolTpl, redisTpl, bindingsTpl)->
		SiteList = Class(common.class.StateFullController,

			uiGridConstants: null

			$timeout: null

			siteListData: [],

			hub: null

			initHub: ()->
				this.hub = new Hub("SiteManagementHub",
					listeners:
						"updateSiteState": (siteData) =>
							this.setSiteState(siteData);
							this.$scope.$apply()
					methods: [
						"RestartPool"
						"StopSite"
						"StartSite"
						"FlushRedis"
					]
				)

			siteDataService: null

			initSiteDataService: ->
				this.siteDataService = new SiteDataService();
				do this.getSites

			constructor: ($rootScope, $scope, $timeout, uiGridConstants) ->
				do this.initHub
				this.uiGridConstants = uiGridConstants
				this.$timeout = $timeout
				SiteList.$super.call(this, $scope, $rootScope)
				do this.initSiteDataService
				return

			defineScope: ->
				SiteList.$superp.defineScope.call this
				do this.initGridOptions
				do this.initScope
				return

			initScope: ->
				this.siteListLoaded false
				return

			apply: ->
				do this.$scope.$apply

			onStateChangeStart: ()->
				SiteList.$superp.onStateChangeStart.call(this)
				do this.hub.disconnect
				this.$scope.hideAllProgressBars = true
				return

			siteListLoaded: (value)->
				if this.$scope.siteListLoaded != value
					this.$scope.siteListLoaded = value

			getGridData: ()->
				return this.siteListData

			setGridData: (data)->
				this.siteListData = data
				this.$scope.gridOptions.data = this.siteListData

			getSites: ->
				querySucceeded = (data) =>
					this.setGridData _.map data, (row)=> new Site row, this.hub, this
					return
				this.$timeout =>
					this.siteDataService.getSites this.bind querySucceeded, ->
						common.logger.error "Web sites loading error"
						return
					.finally =>
						this.siteListLoaded true
						return
					return
				, 0
				return

			setSiteState: (siteData)->
				data = this.getGridData();
				site = _.find data, (row)-> row.id == siteData.id
				if (site)
					site.setState siteData.newData

			#obsolete
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
							width: "20%"
							filter: {
								condition: this.uiGridConstants.filter.CONTAINS
								placeholder: "contains"
							}
						},
						{
							field: "bindings"
							displayName: "Bindings"
							minWidth: 50
							width: "5%"
							cellTemplate: bindingsTpl
							filter: {
								condition: this.uiGridConstants.filter.CONTAINS
								placeholder: "contains"
							}
						},
						{
							field: "state"
							displayName: "State"
							width: "30"
							enableFiltering: false
							cellTemplate: siteStateTpl
						},
						{
							field: "pool"
							displayName: "First app pool"
							width: "10%"
							cellTemplate: poolTpl
							enableFiltering: false
						},
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
			return new SiteList($rootScope, $scope, $timeout, uiGridConstants)
		]