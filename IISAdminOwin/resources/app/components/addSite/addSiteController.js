app.controller("addSiteController", [
  '$rootScope', '$scope', '$timeout', 'Hub', '$', function($rootScope, $scope, $timeout, Hub, $) {
    var Site, getSiteCreateInfo, offFunc, vm;
    vm = $scope;
    Site = Class.create({
      name: null,
      redis: 0,
      workUri: null,
      db: null,
      msSqlInstances: [],
      releaseInfo: {},
      isFormDataValid: function() {
        return this.name && this.redis && this.workUri && this.db;
      }
    });
    vm.site = new Site();
    vm.hub = new Hub('SiteCreateHub', {
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
          label: 'Build uri',
          placeholder: 'Paste product build uri here',
          required: true
        },
        validators: {
          uri: {
            expression: function(viewValue, modelValue) {
              var value;
              value = modelValue || viewValue;
              return /\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b/.test(value);
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
        key: 'db',
        type: 'uiSelect',
        templateOptions: {
          redisKey: 'redis',
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
          vm.site.redis = siteInfo.freeRedisDbNum;
          vm.setSqlInstances(siteInfo.sqlServerInstances);
        });
      });
    };
    vm.updateReleaseInfo = function(uri) {
      vm.hub.GetReleaseInfo(uri).then(function(data) {
        return vm.$apply(function() {
          $.extend(vm.site.releaseInfo, data);
          vm.site.name = vm.site.releaseInfo.name;
        });
      });
    };
    vm.addSite = function() {
      vm.hub.AddSite(vm.site);
    };
    $timeout(function() {
      return getSiteCreateInfo();
    }, 2000);
    offFunc = $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
      var selfDestruct;
      selfDestruct = offFunc;
      vm.hub.disconnect();
      selfDestruct();
      hideAllProgressBars(true);
    });
  }
]);
