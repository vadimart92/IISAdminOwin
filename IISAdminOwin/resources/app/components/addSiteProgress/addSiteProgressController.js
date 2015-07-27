define(["app", "hub", "jquery", "common"], function(app, Hub, $, common) {
  var AddSiteProgress, QueueSiteInfo, SiteInfo;
  SiteInfo = Class(common["class"].BaseClass, {
    id: null
  });
  QueueSiteInfo = Class(common["class"].BaseClass, {
    id: null
  });
  AddSiteProgress = Class(common["class"].StateFullController, {
    queue: [],
    hub: null,
    constructor: function($scope, $rootScope) {
      AddSiteProgress.$super.apply(this, arguments);
      return this.initHub();
    },
    initHub: function() {
      this.hub = new Hub("SiteCreateHub", {
        useSharedConnection: false,
        logging: true,
        listeners: {
          "refreshDeployStatus": this.bind(this.refreshDeployStatus)
        },
        methods: ["CancelDeploy"]
      }, this.bind(function() {
        return console.log;
      }));
    },
    apply: function() {
      return this.$scope.$apply();
    },
    defineScope: function() {
      return AddSiteProgress.$superp.defineScope.call(this);
    },
    onStateChangeStart: function() {
      AddSiteProgress.$superp.onStateChangeStart.call(this);
      return this.hub.disconnect();
    },
    refreshDeployStatus: function(data) {
      return alert(data);
    }
  });
  return [
    "$rootScope", "$scope", function($rootScope, $scope) {
      return new AddSiteProgress($rootScope, $scope);
    }
  ];
});
