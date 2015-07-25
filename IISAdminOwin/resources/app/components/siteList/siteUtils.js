define(["app", "../common/enums"], function(app, Enums) {
  var SiteUtils;
  SiteUtils = Class({
    $static: {
      getIsRunning: function(site) {
        return site && site.state === Enums.SiteState.Started.value;
      },
      getRedisString: function(site) {
        if (Boolean(site.redis)) {
          return site.redis.db + " (" + site.redis.host + ":" + (site.redis.port || "") + ")";
        } else {
          return "can't find";
        }
      },
      getIsRedisFound: function(site) {
        return Boolean(site.redis);
      }
    }
  });
  return SiteUtils;
});
