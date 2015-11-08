define(["common"], function(common) {
  var AppDataService;
  AppDataService = Class(common["class"].BreezeDataService, {
    constructor: function() {
      this.dataServiceConfig = {
        serviceName: "breeze/App",
        hasServerMetadata: false
      };
      return AppDataService.$super.call(this);
    },
    getApplications: function(siteListsObservable, errorObservable) {
      var getFailed, getSucceeded;
      getSucceeded = function(data) {
        siteListsObservable(data.results);
      };
      getFailed = function(error) {
        errorObservable('Error retrieving site lists: ' + error.message);
      };
      return this.breeze.EntityQuery.from("AppList").using(this.manager).execute().then(getSucceeded, getFailed);
    }
  });
  return AppDataService;
});
