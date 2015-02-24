app.controller("siteListController", [
  '$rootScope', '$scope', '$timeout', 'uiGridConstants', 'toasterService', 'siteDataService', 'logger', 'Hub', 'Enums', 'siteUtils', function($rootScope, $scope, $timeout, uiGridConstants, toasterService, siteDataService, logger, Hub, Enums, siteUtils) {
    var bindingsTpl, createRowProgressBar, getGridData, getPoolDisplayName, getSitePool, getSites, hideAllProgressBars, hub, offFunc, poolTemplate, redisTpl, setPoolRestarting, setSiteNewData, setSiteState, siteStateTemplate, stopRowProgressBar, vm;
    vm = $scope;
    hub = new Hub('SiteManagement', {
      listeners: {
        'updateSiteState': function(siteData) {
          setSiteState(siteData);
          $scope.$apply();
        }
      },
      methods: ['RestartPool', 'StopSite', 'StartSite', 'FlushRedis']
    });
    vm.siteListLoaded = false;
    getGridData = function() {
      return vm.gridOptions.data;
    };
    vm.gridOptions = {
      enableScrollbars: 0,
      enableColumnResizing: true,
      enableRowSelection: false,
      enableRowHeaderSelection: false,
      multiSelect: false,
      enableFiltering: true,
      enableGridMenu: true
    };
    siteStateTemplate = '<div class="btn btn-primary btn-xs glyphicon glyphicon-off" ng-click="getExternalScopes().stopSite(row.entity)" ng-show="getExternalScopes().siteUtils.isRunning(row.entity)"></div>' + '<div class="btn btn-primary btn-xs glyphicon glyphicon-play" ng-click="getExternalScopes().startSite(row.entity)" ng-show="!getExternalScopes().siteUtils.isRunning(row.entity)"></div>' + '<span class="binding-label">{{row.entity.state}}</span>';
    poolTemplate = '<div ng-class="{\'hide\': row.entity.poolRestartInProgress}"><div class="btn btn-primary btn-xs glyphicon glyphicon-refresh" ng-click="getExternalScopes().restartPool(row.entity)"></div>' + '<span>{{getExternalScopes().getSitePool(row.entity)}}</span></div>' + '<div class="mProgressContainerRow-msg">{{row.entity.stateName}}</div><div class="mProgressContainerRow progress-row{{row.entity.id}}" ng-class="{\'hide\': !row.entity.poolRestartInProgress}"></div>';
    redisTpl = '<div ng-show="getExternalScopes().siteUtils.redisFound(row.entity)" class="btn btn-primary btn-xs glyphicon glyphicon-refresh" ng-click="getExternalScopes().flushRedis(row.entity)" ng-class="{\'hide\': row.redisFlushInProgress}"></div>' + '<span tooltip-placement="left" tooltip="{{getExternalScopes().siteUtils.redisName(row.entity)}}">{{getExternalScopes().siteUtils.redisName(row.entity)}}</span>';
    bindingsTpl = '<span ng-repeat="binding in row.entity.bindings" tooltip-placement="left" tooltip="{{binding}}">{{binding}}; </span>';
    vm.gridOptions.columnDefs = [
      {
        field: "id",
        displayName: "Id",
        width: "1",
        enableFiltering: false
      }, {
        field: 'name',
        displayName: 'Name',
        minWidth: 50,
        width: "20%",
        filter: {
          condition: uiGridConstants.filter.CONTAINS,
          placeholder: 'contains'
        }
      }, {
        field: 'bindings',
        displayName: 'Bindings',
        minWidth: 50,
        width: "5%",
        cellTemplate: bindingsTpl,
        filter: {
          condition: uiGridConstants.filter.CONTAINS,
          placeholder: 'contains'
        }
      }, {
        field: 'state',
        displayName: 'State',
        width: "30",
        enableFiltering: false,
        cellTemplate: siteStateTemplate
      }, {
        field: 'pool',
        displayName: 'First app pool',
        width: "10%",
        cellTemplate: poolTemplate,
        enableFiltering: false
      }, {
        field: 'redis',
        displayName: 'Redis',
        width: "10%",
        cellTemplate: redisTpl,
        enableFiltering: false
      }
    ];
    getSites = function() {
      var getSitesImpl, querySucceeded;
      querySucceeded = function(data) {
        vm.gridOptions.data = data;
      };
      getSitesImpl = function() {
        siteDataService.getSites(querySucceeded, function() {
          logger.error('Web sites loading error');
        })["finally"](function() {
          vm.siteListLoaded = true;
        });
      };
      $timeout(getSitesImpl, 0);
    };
    setPoolRestarting = function(row) {
      if (row.poolRestartInProgress) {
        return;
      }
      row.poolRestartInProgress = true;
      row.stateName = "Restarting...";
      createRowProgressBar(row);
    };
    setSiteState = function(siteData) {
      var data, site;
      data = getGridData();
      site = _.find(data, function(row) {
        return row.id === siteData.id;
      });
      if (site) {
        setSiteNewData(site, siteData.newData);
        if (siteData.newData.poolRestartState === Enums.PoolState.Restarting.value) {
          setPoolRestarting(site);
        } else {
          site.poolRestartInProgress = false;
          stopRowProgressBar(site);
        }
      }
    };
    setSiteNewData = function(site, newData) {
      $.extend(site, newData);
    };
    createRowProgressBar = function(row) {
      var mProgress;
      hideAllProgressBars(true);
      mProgress = row.progressBar || new Mprogress({
        template: 3,
        parent: '.mProgressContainerRow.progress-row' + row.id
      });
      mProgress.start();
      row.progressBar = mProgress;
    };
    stopRowProgressBar = function(row, force) {
      if (row && row.progressBar && row.progressBar.status) {
        row.progressBar.end(force);
      }
    };
    hideAllProgressBars = function(force) {
      var data;
      data = getGridData();
      return _.each(data, function(row) {
        return stopRowProgressBar(row, force);
      });
    };
    getSitePool = function(site) {
      var firstApp;
      firstApp = site.applications[0];
      return firstApp && firstApp.pool;
    };
    getPoolDisplayName = function(pool) {
      return pool && (pool.name + " (" + pool.state + ")");
    };
    getSites();
    vm.clickHandler = {
      flushRedis: function(row) {
        hub.FlushRedis(row.id);
        return false;
      },
      getSitePool: function(row) {
        var pool;
        pool = getSitePool(row);
        return getPoolDisplayName(pool);
      },
      startSite: function(row) {
        hub.StartSite(row.id);
      },
      stopSite: function(row) {
        hub.StopSite(row.id);
      },
      restartPool: function(row) {
        setPoolRestarting(row);
        hub.RestartPool(row.id);
        return false;
      },
      siteUtils: siteUtils
    };
    offFunc = $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
      var selfDestruct;
      selfDestruct = offFunc;
      hub.disconnect();
      selfDestruct();
      hideAllProgressBars(true);
    });
  }
]);
