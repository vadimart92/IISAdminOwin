app.controller("addSiteController", [
  '$rootScope', '$scope', '$timeout', 'Hub', function($rootScope, $scope, $timeout, Hub) {
    var getSiteCreateInfo, offFunc, vm;
    vm = $scope;
    vm.site = {};
    vm.releaseInfo = {};
    vm.hub = new Hub('SiteCreate', {
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
        key: 'buildFolderLink',
        type: 'input',
        templateOptions: {
          label: 'Build file link',
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
              return /{\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b}/.test(value);
            },
            message: '$viewValue + " is not a valid build URI"'
          }
        },
        watcher: {
          listener: function(field, newValue, oldValue, scope, stopWatching) {
            vm.releaseInfo = {};
            if (newValue) {
              vm.updateReleaseInfo(newValue);
            }
          }
        }
      }, {
        key: 'redis',
        type: 'input',
        templateOptions: {
          label: 'Redis',
          disabled: true
        }
      }, {
        key: 'db',
        type: 'uiSelect',
        templateOptions: {
          label: 'MSSQL Instance'
        }
      }
    ];
    vm.setSqlInstances = function(list) {
      vm.site.msSqlInstances = list;
    };
    getSiteCreateInfo = function() {
      return vm.hub.GetStartupInfo().then(function(siteInfo) {
        return $scope.$apply(function() {
          vm.site.redis = siteInfo.freeRedisDbNum;
          return vm.setSqlInstances(siteInfo.sqlServerInstances);
        });
      });
    };
    vm.updateReleaseInfo = function(uri) {
      vm.hub.GetReleaseInfo(uri).then(function(data) {
        return $scope.$apply(function() {
          return $.extend(vm.releaseInfo, data);
        });
      });
    };
    vm.addSite = function() {
      vm.hub.AddSite(vm.site);
    };
    $timeout(function() {
      return getSiteCreateInfo();
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
