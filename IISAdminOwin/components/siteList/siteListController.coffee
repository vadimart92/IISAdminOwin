# CoffeeScript
app.controller "siteListController", [
  '$rootScope', '$scope', '$timeout', 'uiGridConstants'
  'toasterService', 'siteDataService'
  'logger', 'Hub', 'Enums', 'siteUtils', '$', ($rootScope, $scope, $timeout, uiGridConstants, toasterService, siteDataService, logger, Hub, Enums, siteUtils, $) ->
    vm = $scope;

    hub = new Hub('SiteManagement',
      listeners:
        'updateSiteState': (siteData) ->
          setSiteState(siteData);
          $scope.$apply()
          return
      methods: [
        'RestartPool'
        'StopSite'
        'StartSite'
        'FlushRedis'
      ]
    )

    vm.siteListLoaded = false

    getGridData = ()->
      return vm.gridOptions.data

    vm.gridOptions = {
      enableScrollbars: 0,
      enableColumnResizing: on,
      enableRowSelection: off,
      enableRowHeaderSelection: off,
      multiSelect: off,
      enableFiltering: on,
      enableGridMenu: on
    }

    siteStateTemplate = '<div class="btn btn-primary btn-xs glyphicon glyphicon-off" ng-click="getExternalScopes().stopSite(row.entity)" ng-show="getExternalScopes().siteUtils.isRunning(row.entity)"></div>'+
        '<div class="btn btn-primary btn-xs glyphicon glyphicon-play" ng-click="getExternalScopes().startSite(row.entity)" ng-show="!getExternalScopes().siteUtils.isRunning(row.entity)"></div>'+
        '<span class="binding-label">{{row.entity.state}}</span>'

    poolTemplate = '<div ng-class="{\'hide\': row.entity.poolRestartInProgress}"><div class="btn btn-primary btn-xs glyphicon glyphicon-refresh" ng-click="getExternalScopes().restartPool(row.entity)"></div>' +
        '<span>{{getExternalScopes().getSitePool(row.entity)}}</span></div>' +
        '<div class="mProgressContainerRow-msg">{{row.entity.stateName}}</div><div class="mProgressContainerRow progress-row{{row.entity.id}}" ng-class="{\'hide\': !row.entity.poolRestartInProgress}"></div>'
      
    redisTpl = '<div ng-show="getExternalScopes().siteUtils.redisFound(row.entity)" class="btn btn-primary btn-xs glyphicon glyphicon-refresh" ng-click="getExternalScopes().flushRedis(row.entity)" ng-class="{\'hide\': row.redisFlushInProgress}"></div>'+
        '<span tooltip-placement="left" tooltip="{{getExternalScopes().siteUtils.redisName(row.entity)}}">{{getExternalScopes().siteUtils.redisName(row.entity)}}</span>'

    bindingsTpl = '<span ng-repeat="binding in row.entity.bindings" tooltip-placement="left" tooltip="{{binding}}">{{binding}}; </span>'

    vm.gridOptions.columnDefs = [
      {field: "id", displayName: "Id", width: "1", enableFiltering: false},
      {
        field: 'name',
        displayName: 'Name',
        minWidth: 50,
        width: "20%",
        filter: {
          condition: uiGridConstants.filter.CONTAINS,
          placeholder: 'contains'
        }
      },
      {
        field: 'bindings',
        displayName: 'Bindings',
        minWidth: 50,
        width: "5%",
        cellTemplate: bindingsTpl,
        filter: {
          condition: uiGridConstants.filter.CONTAINS,
          placeholder: 'contains'
        }
      },
      {
        field: 'state',
        displayName: 'State',
        width: "30",
        enableFiltering: false,
        cellTemplate: siteStateTemplate
      },
      {
        field: 'pool',
        displayName: 'First app pool',
        width: "10%",
        cellTemplate: poolTemplate,
        enableFiltering: false
      },
      {
        field: 'redis',
        displayName: 'Redis',
        width: "10%",
        cellTemplate: redisTpl,
        enableFiltering: false
      }
    ]

    getSites = ->
      querySucceeded = (data) ->
        vm.gridOptions.data = data
        return

      getSitesImpl = ->
        siteDataService.getSites querySucceeded, ()->
          logger.error 'Web sites loading error'
          return
        .finally ()->
         vm.siteListLoaded = true
         return
        return

      $timeout getSitesImpl, 0
      return

    setPoolRestarting = (row)->
      if (row.poolRestartInProgress)
        return
      row.poolRestartInProgress = on
      row.stateName = "Restarting..."
      createRowProgressBar row
      return

    setSiteState = (siteData)->
      data = getGridData();
      site = _.find(data, (row)->
        row.id == siteData.id
      )
      if (site)
        setSiteNewData site, siteData.newData
        if (siteData.newData.poolRestartState == Enums.PoolState.Restarting.value)
          setPoolRestarting site
        else
          site.poolRestartInProgress = off
          stopRowProgressBar site
      return

    setSiteNewData = (site, newData)->
      $.extend(site, newData)
      return

    createRowProgressBar = (row)->
      hideAllProgressBars true
      mProgress = row.progressBar || new Mprogress(
        template: 3
        parent: '.mProgressContainerRow.progress-row' + row.id)
      do mProgress.start
      row.progressBar = mProgress
      return

    stopRowProgressBar = (row, force)->
      if (row && row.progressBar && row.progressBar.status)
        row.progressBar.end force
      return

    hideAllProgressBars = (force)->
      data = do getGridData
      _.each data, (row)->
        stopRowProgressBar row, force

    getSitePool = (site)->
      firstApp = site.applications[0]
      firstApp && firstApp.pool

    getPoolDisplayName = (pool)->
      return pool && ("#{pool.name} (#{pool.state})")

    getSites()

    vm.clickHandler = {
      flushRedis: (row)->
        hub.FlushRedis row.id
        return false
      getSitePool: (row)->
        pool = getSitePool row
        getPoolDisplayName pool
      startSite: (row)->
        hub.StartSite row.id
        return
      stopSite: (row)->
        hub.StopSite row.id
        return
      restartPool: (row)->
        setPoolRestarting row
        hub.RestartPool row.id
        return false
      siteUtils: siteUtils
    }

    offFunc = $rootScope.$on '$stateChangeStart', (event, toState, toParams, fromState, fromParams) ->
      selfDestruct = offFunc;
      do hub.disconnect
      do selfDestruct
      hideAllProgressBars true
      return


    return
]
