app.factory('siteUtils', [
  'Enums', function(Enums) {
    var getIsRedisFound, getIsRunning, getRedisString, utils;
    getIsRunning = function(site) {
      return site && site.state === Enums.SiteState.Started.value;
    };
    getRedisString = function(site) {
      if (Boolean(site.redis)) {
        return site.redis.db + " (" + site.redis.host + ":" + (site.redis.port || "") + ")";
      } else {
        return "can't find";
      }
    };
    getIsRedisFound = function(site) {
      return Boolean(site.redis);
    };
    utils = {
      isRunning: getIsRunning,
      redisName: getRedisString,
      redisFound: getIsRedisFound
    };
    return utils;
  }
]);
