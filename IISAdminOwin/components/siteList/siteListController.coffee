define [
	"app"
	"../dataService/siteDataService"
	"common"
	"hub"
	"siteList/siteUtils"
	"toaster"
	"jquery"
	"mProgress"
	"text!templates/siteList/siteList.columns.siteState.html"
	"text!templates/siteList/siteList.columns.pool.html"
	"text!templates/siteList/siteList.columns.redis.html"
	"text!templates/siteList/siteList.columns.bindings.html"
], (app, SiteDataService, common, Hub, siteUtils, toaster, $, Mprogress, siteStateTpl, poolTpl, redisTpl, bindingsTpl)->
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
				do this.hub.connect

			siteDataService: null

			initSiteDataService: ->
				this.siteDataService = new SiteDataService();
				do this.getSites

			constructor: ($rootScope, $scope, $timeout, uiGridConstants) ->
				do this.initHub
				this.uiGridConstants = uiGridConstants
				this.$timeout = $timeout
				SiteList.$super.call(this, $scope, $rootScope)
				do this.initClickHandler
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

			getSites: ->
				querySucceeded = (data) =>
					gridData = do this.getGridData
					_.each data, (row)->gridData.push row
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

			setPoolRestarting: (row)->
				if (row.poolRestartInProgress)
					return
				row.poolRestartInProgress = on
				row.stateName = "Restarting..."
				this.createRowProgressBar row
				return

			setSiteState: (siteData)->
				data = this.getGridData();
				site = _.find(data, (row)->
					row.id == siteData.id
				)
				if (site)
					this.setSiteNewData site, siteData.newData
					if (siteData.newData.poolRestartState == common.enums.PoolState.Restarting.value)
						this.setPoolRestarting site
					else
						site.poolRestartInProgress = off
						this.stopRowProgressBar site
				return

			setSiteNewData: (site, newData)->
				$.extend(site, newData)
				return

			createRowProgressBar: (row)->
				this.hideAllProgressBars true
				mProgress = row.progressBar || new Mprogress(
					template: 3
					parent: ".mProgressContainerRow.progress-row" + row.id)
				do mProgress.start
				row.progressBar = mProgress
				return

			stopRowProgressBar: (row, force)->
				if (row && row.progressBar && row.progressBar.status)
					row.progressBar.end force
				return

			hideAllProgressBars: (force)->
				data = do this.getGridData
				_.each data, (row)=>
					this.stopRowProgressBar row, force
				return

			getSitePool: (site)->
				firstApp = site.applications[0]
				firstApp && firstApp.pool
				return

			getPoolDisplayName: (pool)->
				return pool && ("#{pool.name} (#{pool.state})")

			initClickHandler: ->
				this.$scope.clickHandler =
					flushRedis: (row)=>
						this.hub.FlushRedis row.id
						return false
					getSitePool: (row)=>
						pool = this.getSitePool row
						this.getPoolDisplayName pool
						return
					startSite: (row)=>
						this.hub.StartSite row.id
						return
					stopSite: (row)=>
						this.hub.StopSite row.id
						return
					restartPool: (row)=>
						this.setPoolRestarting row
						this.hub.RestartPool row.id
						return false
					siteUtils: siteUtils

			initGridOptions: ->
				this.$scope.gridOptions = {
					data: this.siteListData
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
		);
		["$rootScope", "$scope", "$timeout", "uiGridConstants", ($rootScope, $scope, $timeout, uiGridConstants)->
			return new SiteList($rootScope, $scope, $timeout, uiGridConstants)
		]