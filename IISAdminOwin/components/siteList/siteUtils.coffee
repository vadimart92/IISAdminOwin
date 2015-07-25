define ["app", "../common/enums"], (app, Enums)->
	SiteUtils = Class(
		$static: {
			getIsRunning: (site)->
				return site && site.state == Enums.SiteState.Started.value

			getRedisString: (site)->
				if Boolean(site.redis)
				then "#{site.redis.db} (#{site.redis.host}:#{site.redis.port || ""})"
				else "can't find"

			getIsRedisFound: (site)->
				return Boolean(site.redis)
		}
	)
	SiteUtils