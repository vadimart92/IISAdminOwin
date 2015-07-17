define(["breeze"], function(breeze) {
  var siteDataService;
  return siteDataService = function($http, $q, $timeout) {
    var clearCache, configureForBreeze, configureManagerToSaveModifiedItemImmediately, getSites, manager, metadataStore, saveEntity, service;
    breeze.NamingConvention.camelCase.setAsDefault();
    manager = void 0;
    metadataStore = void 0;
    clearCache = function() {
      manager.clear();
    };
    configureForBreeze = function() {
      var dataService;
      breeze.NamingConvention.camelCase.setAsDefault();
      dataService = new breeze.DataService({
        serviceName: 'breeze/Site',
        hasServerMetadata: false
      });
      manager = new breeze.EntityManager({
        dataService: dataService
      });
      metadataStore = manager.metadataStore;
      manager.enableSaveQueuing(true);
      configureManagerToSaveModifiedItemImmediately();
    };
    configureManagerToSaveModifiedItemImmediately = function() {
      manager.entityChanged.subscribe(function(args) {
        var entity;
        if (args.entityAction === breeze.EntityAction.EntityStateChange) {
          entity = args.entity;
          if (entity.entityAspect.entityState.isModified()) {
            saveEntity(entity);
          }
        }
      });
    };
    getSites = function(siteListsObservable, errorObservable) {
      var getFailed, getSucceeded;
      getSucceeded = function(data) {
        siteListsObservable(data.results);
      };
      getFailed = function(error) {
        errorObservable('Error retrieving todo lists: ' + error.message);
      };
      return breeze.EntityQuery.from('SiteList').using(manager).execute().then(getSucceeded)["catch"](getFailed);
    };
    saveEntity = function(masterEntity) {
      var getValidationErrorMessage, isConcurrencyError, saveFailed, setErrorMessage;
      saveFailed = function(error) {
        setErrorMessage(error);
        setTimeout((function() {
          manager.rejectChanges();
        }), 1000);
        throw error;
      };
      setErrorMessage = function(error) {
        var msg, reason, stateName, typeName;
        stateName = masterEntity.entityAspect.entityState.name.toLowerCase();
        typeName = masterEntity.entityType.shortName;
        msg = 'Error saving ' + stateName + ' ' + typeName + ': ';
        reason = error.message;
        if (error.entityErrors) {
          reason = getValidationErrorMessage(error);
        } else if (isConcurrencyError(error)) {
          reason = 'can\'t find ' + typeName + '; another user may have deleted it.';
        }
        masterEntity.errorMessage(msg + reason);
      };
      getValidationErrorMessage = function(error) {
        var e, firstError;
        try {
          firstError = error.entityErrors[0];
          return firstError.errorMessage;
        } catch (_error) {
          e = _error;
          return 'validation error';
        }
      };
      isConcurrencyError = function(error) {
        var detail;
        detail = error.detail;
        return detail && detail.ExceptionMessage && detail.ExceptionMessage.match(/can't find/i);
      };
      return manager.saveChanges()["catch"](saveFailed);
    };
    configureForBreeze();
    service = {
      clearCache: clearCache,
      metadataStore: metadataStore,
      getSites: getSites
    };
    return service;
  };
});

siteDataService.$inject = ["$http", "$q", "$timeout"];

app.factory("siteDataService", siteDataService);
