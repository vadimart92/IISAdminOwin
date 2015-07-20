define(["app", "hub", "jquery", "common"], function(app, Hub, $, common) {
  var AddSite, Site;
  Site = Class({
    name: null,
    workUri: null,
    db: null,
    msSqlInstances: [],
    releaseInfo: {},
    isFormDataValid: function() {
      return this.name && this.workUri && this.db;
    }
  });
  AddSite = Class(common["class"].StateFullController, {
    $rootScope: null,
    $timeout: null,
    site: new Site(),
    hub: null,
    constructor: function($rootScope, $scope, $timeout) {
      AddSite.$super.call(this, $scope, $rootScope);
      return this.initHub();
    },
    initHub: function() {
      this.hub = new Hub("SiteCreateHub", {
        logging: true,
        listeners: [],
        methods: ["AddSite", "GetReleaseInfo", "GetStartupInfo"]
      });
      this.hub.connect((function() {
        this.getSiteCreateInfo();
        return this.site.workUri = "f63e0379-c338-4fe0-846e-ca088acdbb5d";
      }).bind(this));
    },
    onWorkUriChange: function(field, newValue) {
      this.site.releaseInfo = {};
      if (newValue) {
        this.updateReleaseInfo(newValue);
      }
    },
    apply: function() {
      return this.$scope.$apply();
    },
    defineScope: function() {
      AddSite.$superp.defineScope.call(this);
      this.initSiteFields();
      this.initReleaseInfoFields();
      return this.$scope.site = this.site;
    },
    onStateChangeStart: function() {
      AddSite.$superp.onStateChangeStart.call(this);
      this.hub.disconnect();
      return this.$scope.hideAllProgressBars = true;
    },
    setSqlInstances: function(sqlInstances) {
      this.site.msSqlInstances = sqlInstances;
    },
    getSiteCreateInfo: function() {
      var me;
      me = this;
      return this.hub.GetStartupInfo().then(function(siteInfo) {
        return me.$scope.$apply(function() {
          me.setSqlInstances(siteInfo.sqlServerInstances);
        });
      });
    },
    updateReleaseInfo: function(uri) {
      var me;
      me = this;
      this.hub.GetReleaseInfo(uri).then(function(data) {
        $.extend(me.site.releaseInfo, data.release);
        me.site.name = data.webAppName;
        me.site.webAppDir = data.webAppDir;
        return me.apply();
      });
    },
    addSite: function() {
      this.hub.AddSite(this.site);
    },
    initSiteFields: function() {
      this.$scope.siteFields = [
        {
          key: "workUri",
          type: "input",
          templateOptions: {
            label: "Build uri/id",
            placeholder: "Paste product build uri here",
            required: true
          },
          validators: {
            uri: {
              expression: function(viewValue, modelValue) {
                return common.utils.containsGUID(modelValue || viewValue);
              },
              message: '$viewValue + " is not a valid build URI"'
            }
          },
          watcher: {
            listener: this.onWorkUriChange.bind(this)
          }
        }, {
          key: "name",
          type: "input",
          templateOptions: {
            label: "Name"
          }
        }, {
          key: "webAppDir",
          type: "input",
          templateOptions: {
            label: "Web app directory"
          }
        }, {
          key: "db",
          type: "uiSelect",
          templateOptions: {
            label: "MSSQL Instance"
          }
        }
      ];
    },
    initReleaseInfoFields: function() {
      return this.$scope.releaseInfoFields = [
        {
          key: "createdOn",
          type: "input",
          templateOptions: {
            label: "Created on",
            disabled: true
          }
        }, {
          key: "name",
          type: "input",
          templateOptions: {
            label: "Build name",
            disabled: true
          }
        }, {
          key: "zipFilePath",
          type: "input",
          templateOptions: {
            label: "Zip file path",
            disabled: true
          }
        }, {
          key: "version",
          type: "input",
          templateOptions: {
            label: "Version",
            disabled: true
          }
        }, {
          key: "release",
          type: "checkbox",
          templateOptions: {
            label: "Release",
            disabled: true
          }
        }
      ];
    }
  });
  return [
    "$rootScope", "$scope", "$timeout", function($rootScope, $scope, $timeout) {
      return new AddSite($rootScope, $scope, $timeout);
    }
  ];
});
