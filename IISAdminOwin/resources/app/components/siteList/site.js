define(["common", "mProgress"], function(common, Mprogress) {
  var SiteViewModel;
  SiteViewModel = Class(common["class"].BaseClass, {
    id: null,
    model: null,
    hub: null,
    controller: null,
    progressBar: null,
    stateName: null,
    poolRestartInProgress: null,
    $props: {
      name: {
        get: function() {
          return this.model.name;
        }
      },
      poolName: {
        get: function() {
          var pool;
          pool = this.getSitePool();
          return pool && (pool.name + " (" + pool.state + ")");
        }
      },
      bindings: {
        get: function() {
          return this.model.bindings.join("; ");
        }
      },
      state: {
        get: function() {
          return this.model.state;
        }
      },
      isSiteRunning: {
        get: function() {
          return this.model.state === common.enums.SiteState.Started.value;
        }
      },
      redisName: {
        get: function() {
          if (this.redisFound) {
            return this.model.redis.db + " (" + this.model.redis.host + ":" + (this.model.redis.port || "") + ")";
          } else {
            return "can't find";
          }
        }
      },
      redisFound: {
        get: function() {
          return Boolean(this.model.redis);
        }
      }
    },
    constructor: function(model, siteHub, siteController) {
      SiteViewModel.$super.call(this);
      this.model = model;
      this.hub = siteHub;
      this.id = model.id;
      this.controller = siteController;
    },
    getSitePool: function() {
      var firstApp;
      firstApp = this.model.applications[0];
      return firstApp && firstApp.pool;
    },
    createRowProgressBar: function() {
      var mProgress;
      mProgress = this.progressBar || new Mprogress({
        start: true,
        template: 3,
        parent: ".mProgressContainerRow.progress-row" + this.id
      });
      this.progressBar = mProgress;
    },
    destroyProgressBar: function(force) {
      if (this.progressBar && this.progressBar.status) {
        this.progressBar.end(force);
        delete this.progressBar;
      }
    },
    restartPool: function() {
      if (this.poolRestartInProgress) {
        return;
      }
      this.poolRestartInProgress = true;
      this.stateName = "Restarting...";
      this.createRowProgressBar();
      this.hub.RestartPool(this.id);
    },
    setState: function(newData) {
      $.extend(this.model, newData);
      if (newData.poolRestartState === common.enums.PoolState.Restarting.value) {
        this.createRowProgressBar();
      } else {
        this.poolRestartInProgress = false;
        this.destroyProgressBar();
      }
      this.clearCache();
    },
    flushRedis: function() {
      this.hub.FlushRedis(this.id);
    },
    startSite: function() {
      this.hub.StartSite(this.id);
    },
    stopSite: function() {
      this.hub.StopSite(this.id);
    }
  });
  return SiteViewModel;
});
