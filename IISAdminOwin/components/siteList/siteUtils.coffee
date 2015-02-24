app.factory 'siteUtils', [
  'Enums'
  (Enums) ->

    getIsRunning = (site)->
      return site && site.state == Enums.SiteState.Started.value

    getRedisString = (site)->
      return if Boolean(site.redis)
      then "#{site.redis.db} (#{site.redis.host}:#{site.redis.port || ""})"
      else "can't find"

    getIsRedisFound = (site)->
      return Boolean(site.redis)

    utils =
      isRunning: getIsRunning
      redisName: getRedisString
      redisFound: getIsRedisFound
    utils
]
