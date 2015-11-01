define(["app", "../dataService/appDataService", "appList/application", "common", "hub", "jquery", "text!templates/appList/appList.columns.pool.html", "text!templates/appList/appList.columns.redis.html"], function(app, AppDataService, ApplicationViewModel, common, Hub, $, poolTpl, redisTpl) {
  var AppList;
  AppList = Class(common["class"].StateFullController, {
    uiGridConstants: null,
    $timeout: null,
    appListData: [],
    hub: null,
    initHub: function() {
      return this.hub = new Hub("SiteManagementHub", {
        listeners: {
          "updateAppState": (function(_this) {
            return function(siteData) {
              _this.setAppState(siteData);
              return _this.$scope.$apply();
            };
          })(this)
        },
        methods: ["RestartPool", "StopPool", "StartPool", "FlushRedis"]
      });
    },
    AppDataService: null,
    initAppDataService: function() {
      this.AppDataService = new AppDataService();
      return this.getList();
    },
    constructor: function($rootScope, $scope, $timeout, uiGridConstants) {
      this.initHub();
      this.uiGridConstants = uiGridConstants;
      this.$timeout = $timeout;
      AppList.$super.call(this, $scope, $rootScope);
      this.initAppDataService();
    },
    defineScope: function() {
      AppList.$superp.defineScope.call(this);
      this.initGridOptions();
      this.initScope();
    },
    initScope: function() {
      this.listLoaded(false);
    },
    apply: function() {
      return this.$scope.$apply();
    },
    onStateChangeStart: function() {
      AppList.$superp.onStateChangeStart.call(this);
      this.hub.disconnect();
      this.$scope.hideAllProgressBars = true;
    },
    listLoaded: function(value) {
      if (this.$scope.listLoaded !== value) {
        return this.$scope.listLoaded = value;
      }
    },
    getGridData: function() {
      return this.appListData;
    },
    setGridData: function(data) {
      this.appListData = data;
      return this.$scope.gridOptions.data = this.appListData;
    },
    getList: function() {
      var querySucceeded;
      querySucceeded = (function(_this) {
        return function(data) {
          _this.setGridData(_.map(data, function(row) {
            return new ApplicationViewModel(row, _this.hub, _this);
          }));
        };
      })(this);
      this.$timeout((function(_this) {
        return function() {
          _this.AppDataService.getApplications(_this.bind(querySucceeded, function() {
            common.logger.error("Web sites loading error");
          }))["finally"](function() {
            _this.listLoaded(true);
          });
        };
      })(this), 0);
    },
    setAppState: function(siteData) {
      var data, site;
      data = this.getGridData();
      site = _.find(data, function(row) {
        return row.id === siteData.id;
      });
      if (site) {
        return site.setState(siteData.newData);
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
    initGridOptions: function() {
      this.$scope.gridOptions = {
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
            width: "30%",
            filter: {
              condition: this.uiGridConstants.filter.CONTAINS,
              placeholder: "contains"
            }
          }, {
            field: "pool",
            displayName: "App pool",
            width: "30%",
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
      return this.setGridData([]);
    }
  });
  return [
    "$rootScope", "$scope", "$timeout", "uiGridConstants", function($rootScope, $scope, $timeout, uiGridConstants) {
      return new AppList($rootScope, $scope, $timeout, uiGridConstants);
    }
  ];
});
