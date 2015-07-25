app.controller("addSite.progress.Controller", [
  '$rootScope', '$scope', '$timeout', 'Hub', '$', 'utils', function($rootScope, $scope, $timeout, Hub, $, utils) {
    var Site, getSiteCreateInfo, offFunc, vm;
    vm = $scope;
    Site = Class.create({
      name: null,
      workUri: null,
      db: null,
      msSqlInstances: [],
      releaseInfo: {},
      isFormDataValid: function() {
        return this.name && this.workUri && this.db;
      }
    });
    vm.site = new Site();
    vm.hub = new Hub('SiteCreateHub', {
      logging: true,
      listeners: [],
      methods: ['AddSite', 'GetReleaseInfo', 'GetStartupInfo']
    });
    vm.releaseInfoFields = [
      {
        key: 'createdOn',
        type: 'input',
        templateOptions: {
          label: 'Created on',
          disabled: true
        }
      }, {
        key: 'name',
        type: 'input',
        templateOptions: {
          label: 'Build name',
          disabled: true
        }
      }, {
        key: 'zipFilePath',
        type: 'input',
        templateOptions: {
          label: 'Zip file path',
          disabled: true
        }
      }, {
        key: 'version',
        type: 'input',
        templateOptions: {
          label: 'Version',
          disabled: true
        }
      }, {
        key: 'release',
        type: 'checkbox',
        templateOptions: {
          label: 'Release',
          disabled: true
        }
      }
    ];
    vm.siteFields = [
      {
        key: 'workUri',
        type: 'input',
        templateOptions: {
          label: 'Build uri/id',
          placeholder: 'Paste product build uri here',
          required: true
        },
        validators: {
          uri: {
            expression: function(viewValue, modelValue) {
              return utils.containsGUID(modelValue || viewValue);
            },
            message: '$viewValue + " is not a valid build URI"'
          }
        },
        watcher: {
          listener: function(field, newValue, oldValue, scope, stopWatching) {
            vm.site.releaseInfo = {};
            if (newValue) {
              vm.updateReleaseInfo(newValue);
            }
          }
        }
      }, {
        key: 'name',
        type: 'input',
        templateOptions: {
          label: 'Name'
        }
      }, {
        key: 'webAppDir',
        type: 'input',
        templateOptions: {
          label: 'Web app directory'
        }
      }, {
        key: 'db',
        type: 'uiSelect',
        templateOptions: {
          label: 'MSSQL Instance'
        }
      }
    ];
    vm.setSqlInstances = function(sqlInstances) {
      vm.site.msSqlInstances = sqlInstances;
    };
    getSiteCreateInfo = function() {
      return vm.hub.GetStartupInfo().then(function(siteInfo) {
        return vm.$apply(function() {
          vm.setSqlInstances(siteInfo.sqlServerInstances);
        });
      });
    };
    vm.updateReleaseInfo = function(uri) {
      vm.hub.GetReleaseInfo(uri).then(function(data) {
        return vm.$apply(function() {
          $.extend(vm.site.releaseInfo, data.release);
          vm.site.name = data.webAppName;
          vm.site.webAppDir = data.webAppDir;
        });
      });
    };
    vm.addSite = function() {
      vm.hub.AddSite(vm.site);
    };
    $timeout(function() {
      getSiteCreateInfo();
      return vm.site.workUri = "f63e0379-c338-4fe0-846e-ca088acdbb5d";
    }, 1000);
    offFunc = $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
      var selfDestruct;
      selfDestruct = offFunc;
      vm.hub.disconnect();
      selfDestruct();
      hideAllProgressBars(true);
    });
  }
]);