define(["common"], function(common) {
  var SiteDataService;
  SiteDataService = Class(common["class"].BreezeDataService, {
    constructor: function() {
      this.dataServiceConfig = {
        serviceName: "breeze/Site",
        hasServerMetadata: false
      };
      return SiteDataService.$super.call(this);
    },
    getSites: function(siteListsObservable, errorObservable) {
      var getFailed, getSucceeded;
      getSucceeded = function(data) {
        siteListsObservable(data.results);
      };
      getFailed = function(error) {
        errorObservable('Error retrieving todo lists: ' + error.message);
      };
      return this.breeze.EntityQuery.from("SiteList").using(this.manager).execute().then(getSucceeded, getFailed);
    }
  });
  return SiteDataService;
});
