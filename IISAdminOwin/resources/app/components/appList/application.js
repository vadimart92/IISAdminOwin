define(["common", "mProgress"], function(common, Mprogress) {
  var ApplicationViewModel;
  ApplicationViewModel = Class(common["class"].BaseClass, {
    id: null,
    model: null,
    hub: null,
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
      isRunning: {
        get: function() {
          return "yes";
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
    constructor: function(model, siteHub) {
      ApplicationViewModel.$super.call(this);
      this.model = model;
      this.hub = siteHub;
      this.id = model.id;
    },
    getSitePool: function() {
      return this.model.pool;
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
      this.hub.StartPool(this.id);
    },
    stopSite: function() {
      this.hub.StopPool(this.id);
    }
  });
  return ApplicationViewModel;
});
