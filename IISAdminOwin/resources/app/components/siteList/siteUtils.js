app.factory('siteUtils', [
  'Enums', function(Enums) {
    var getIsRunning, getRedisString, utils;
    getIsRunning = function(site) {
      return site && site.state === Enums.SiteState.Started.displayValue;
    };
    getRedisString = function(site) {
      return site.redis.db + " (" + site.redis.host + ":" + (site.redis.port || "") + ")";
    };
    utils = {
      isRunning: getIsRunning,
      redisName: getRedisString
    };
    return utils;
  }
]);
