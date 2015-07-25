define(["breeze", "breeze_ng"], function(breeze) {
  var BreezeDataService;
  BreezeDataService = Class({
    manager: null,
    metadataStore: null,
    breeze: null,
    dataServiceConfig: {
      serviceName: "breeze/Name",
      hasServerMetadata: false
    },
    constructor: function() {
      this.breeze = breeze;
      breeze.NamingConvention.camelCase.setAsDefault();
      return this.configureForBreeze();
    },
    clearCache: function() {
      this.manager.clear();
    },
    configureForBreeze: function() {
      var dataService;
      breeze.NamingConvention.camelCase.setAsDefault();
      dataService = new breeze.DataService(this.dataServiceConfig);
      this.manager = new breeze.EntityManager({
        dataService: dataService
      });
      this.metadataStore = this.manager.metadataStore;
      this.manager.enableSaveQueuing(true);
      this.configureManagerToSaveModifiedItemImmediately();
    },
    configureManagerToSaveModifiedItemImmediately: function() {
      this.manager.entityChanged.subscribe(function(args) {
        var entity;
        if (args.entityAction === breeze.EntityAction.EntityStateChange) {
          entity = args.entity;
          if (entity.entityAspect.entityState.isModified()) {
            saveEntity(entity);
          }
        }
      });
    },
    saveFailed: function(error) {
      this.setErrorMessage(error);
      setTimeout((function() {
        this.manager.rejectChanges();
      }), 1000);
      throw error;
    },
    saveEntity: function(masterEntity) {
      return this.manager.saveChanges()["catch"](this.saveFailed);
    },
    setErrorMessage: function(error) {
      var msg, reason, stateName, typeName;
      stateName = masterEntity.entityAspect.entityState.name.toLowerCase();
      typeName = masterEntity.entityType.shortName;
      msg = 'Error saving ' + stateName + ' ' + typeName + ': ';
      reason = error.message;
      if (error.entityErrors) {
        reason = this.getValidationErrorMessage(error);
      } else if (isConcurrencyError(error)) {
        reason = 'can\'t find ' + typeName + '; another user may have deleted it.';
      }
      masterEntity.errorMessage(msg + reason);
    },
    getValidationErrorMessage: function(error) {
      var e, firstError;
      try {
        firstError = error.entityErrors[0];
        return firstError.errorMessage;
      } catch (_error) {
        e = _error;
        return "validation error";
      }
    },
    isConcurrencyError: function(error) {
      var detail;
      detail = error.detail;
      return detail && detail.ExceptionMessage && detail.ExceptionMessage.match(/can't find/i);
    }
  });
  return BreezeDataService;
});
