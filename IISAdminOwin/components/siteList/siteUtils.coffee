app.factory 'siteUtils', [
  'Enums'
  (Enums) ->

    getIsRunning = (site)->
      return site && site.state == Enums.SiteState.Started.displayValue

    getRedisString = (site)->
      return "#{site.redis.db} (#{site.redis.host}:#{site.redis.port || ""})"
    utils =
      isRunning: getIsRunning
      redisName: getRedisString

    utils
]
