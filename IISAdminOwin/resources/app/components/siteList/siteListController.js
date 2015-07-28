define(["app", "../dataService/siteDataService", "common", "hub", "siteList/site", "toaster", "jquery", "text!templates/siteList/siteList.columns.siteState.html", "text!templates/siteList/siteList.columns.pool.html", "text!templates/siteList/siteList.columns.redis.html", "text!templates/siteList/siteList.columns.bindings.html"], function(app, SiteDataService, common, Hub, Site, toaster, $, siteStateTpl, poolTpl, redisTpl, bindingsTpl) {
  var SiteList;
  SiteList = Class(common["class"].StateFullController, {
    uiGridConstants: null,
    $timeout: null,
    siteListData: [],
    hub: null,
    initHub: function() {
      return this.hub = new Hub("SiteManagementHub", {
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
    setGridData: function(data) {
      this.siteListData = data;
      return this.$scope.gridOptions.data = this.siteListData;
    },
    getSites: function() {
      var querySucceeded;
      querySucceeded = (function(_this) {
        return function(data) {
          _this.setGridData(_.map(data, function(row) {
            return new Site(row, _this.hub, _this);
          }));
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
    setSiteState: function(siteData) {
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
            field: "key",
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
      return this.setGridData([]);
    }
  });
  return [
    "$rootScope", "$scope", "$timeout", "uiGridConstants", function($rootScope, $scope, $timeout, uiGridConstants) {
      return new SiteList($rootScope, $scope, $timeout, uiGridConstants);
    }
  ];
});
