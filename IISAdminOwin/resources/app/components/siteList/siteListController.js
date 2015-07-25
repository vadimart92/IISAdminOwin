define(["app", "../dataService/siteDataService", "common", "hub", "siteList/siteUtils", "toaster", "jquery", "mProgress", "text!templates/siteList/siteList.columns.siteState.html", "text!templates/siteList/siteList.columns.pool.html", "text!templates/siteList/siteList.columns.redis.html", "text!templates/siteList/siteList.columns.bindings.html"], function(app, SiteDataService, common, Hub, siteUtils, toaster, $, Mprogress, siteStateTpl, poolTpl, redisTpl, bindingsTpl) {
  var SiteList;
  SiteList = Class(common["class"].StateFullController, {
    uiGridConstants: null,
    $timeout: null,
    siteListData: [],
    hub: null,
    initHub: function() {
      this.hub = new Hub("SiteManagementHub", {
        listeners: {
          "updateSiteState": (function(_this) {
            return function(siteData) {
              _this.setSiteState(siteData);
              return _this.$scope.$apply();
            };
          })(this)
        },
        methods: ["RestartPool", "StopSite", "StartSite", "FlushRedis"]
      });
      return this.hub.connect();
    },
    siteDataService: null,
    initSiteDataService: function() {
      this.siteDataService = new SiteDataService();
      return this.getSites();
    },
    constructor: function($rootScope, $scope, $timeout, uiGridConstants) {
      this.initHub();
      this.uiGridConstants = uiGridConstants;
      this.$timeout = $timeout;
      SiteList.$super.call(this, $scope, $rootScope);
      this.initClickHandler();
      this.initSiteDataService();
    },
    defineScope: function() {
      SiteList.$superp.defineScope.call(this);
      this.initGridOptions();
      this.initScope();
    },
    initScope: function() {
      this.siteListLoaded(false);
    },
    apply: function() {
      return this.$scope.$apply();
    },
    onStateChangeStart: function() {
      SiteList.$superp.onStateChangeStart.call(this);
      this.hub.disconnect();
      this.$scope.hideAllProgressBars = true;
    },
    siteListLoaded: function(value) {
      if (this.$scope.siteListLoaded !== value) {
        return this.$scope.siteListLoaded = value;
      }
    },
    getGridData: function() {
      return this.siteListData;
    },
    getSites: function() {
      var querySucceeded;
      querySucceeded = (function(_this) {
        return function(data) {
          var gridData;
          gridData = _this.getGridData();
          _.each(data, function(row) {
            return gridData.push(row);
          });
        };
      })(this);
      this.$timeout((function(_this) {
        return function() {
          _this.siteDataService.getSites(_this.bind(querySucceeded, function() {
            common.logger.error("Web sites loading error");
          }))["finally"](function() {
            _this.siteListLoaded(true);
          });
        };
      })(this), 0);
    },
    setPoolRestarting: function(row) {
      if (row.poolRestartInProgress) {
        return;
      }
      row.poolRestartInProgress = true;
      row.stateName = "Restarting...";
      this.createRowProgressBar(row);
    },
    setSiteState: function(siteData) {
      var data, site;
      data = this.getGridData();
      site = _.find(data, function(row) {
        return row.id === siteData.id;
      });
      if (site) {
        this.setSiteNewData(site, siteData.newData);
        if (siteData.newData.poolRestartState === common.enums.PoolState.Restarting.value) {
          this.setPoolRestarting(site);
        } else {
          site.poolRestartInProgress = false;
          this.stopRowProgressBar(site);
        }
      }
    },
    setSiteNewData: function(site, newData) {
      $.extend(site, newData);
    },
    createRowProgressBar: function(row) {
      var mProgress;
      this.hideAllProgressBars(true);
      mProgress = row.progressBar || new Mprogress({
        template: 3,
        parent: ".mProgressContainerRow.progress-row" + row.id
      });
      mProgress.start();
      row.progressBar = mProgress;
    },
    stopRowProgressBar: function(row, force) {
      if (row && row.progressBar && row.progressBar.status) {
        row.progressBar.end(force);
      }
    },
    hideAllProgressBars: function(force) {
      var data;
      data = this.getGridData();
      _.each(data, (function(_this) {
        return function(row) {
          return _this.stopRowProgressBar(row, force);
        };
      })(this));
    },
    getSitePool: function(site) {
      var firstApp;
      firstApp = site.applications[0];
      firstApp && firstApp.pool;
    },
    getPoolDisplayName: function(pool) {
      return pool && (pool.name + " (" + pool.state + ")");
    },
    initClickHandler: function() {
      return this.$scope.clickHandler = {
        flushRedis: (function(_this) {
          return function(row) {
            _this.hub.FlushRedis(row.id);
            return false;
          };
        })(this),
        getSitePool: (function(_this) {
          return function(row) {
            var pool;
            pool = _this.getSitePool(row);
            _this.getPoolDisplayName(pool);
          };
        })(this),
        startSite: (function(_this) {
          return function(row) {
            _this.hub.StartSite(row.id);
          };
        })(this),
        stopSite: (function(_this) {
          return function(row) {
            _this.hub.StopSite(row.id);
          };
        })(this),
        restartPool: (function(_this) {
          return function(row) {
            _this.setPoolRestarting(row);
            _this.hub.RestartPool(row.id);
            return false;
          };
        })(this),
        siteUtils: siteUtils
      };
    },
    initGridOptions: function() {
      return this.$scope.gridOptions = {
        data: this.siteListData,
        enableScrollbars: 0,
        enableColumnResizing: true,
        enableRowSelection: false,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableFiltering: true,
        enableGridMenu: true,
        columnDefs: [
          {
            field: "id",
            displayName: "Id",
            width: "1",
            enableFiltering: false
          }, {
            field: "name",
            displayName: "Name",
            minWidth: 50,
            width: "20%",
            filter: {
              condition: this.uiGridConstants.filter.CONTAINS,
              placeholder: "contains"
            }
          }, {
            field: "bindings",
            displayName: "Bindings",
            minWidth: 50,
            width: "5%",
            cellTemplate: bindingsTpl,
            filter: {
              condition: this.uiGridConstants.filter.CONTAINS,
              placeholder: "contains"
            }
          }, {
            field: "state",
            displayName: "State",
            width: "30",
            enableFiltering: false,
            cellTemplate: siteStateTpl
          }, {
            field: "pool",
            displayName: "First app pool",
            width: "10%",
            cellTemplate: poolTpl,
            enableFiltering: false
          }, {
            field: "redis",
            displayName: "Redis",
            width: "10%",
            cellTemplate: redisTpl,
            enableFiltering: false
          }
        ]
      };
    }
  });
  return [
    "$rootScope", "$scope", "$timeout", "uiGridConstants", function($rootScope, $scope, $timeout, uiGridConstants) {
      return new SiteList($rootScope, $scope, $timeout, uiGridConstants);
    }
  ];
});
