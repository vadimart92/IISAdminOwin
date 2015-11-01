define ["common",
				"mProgress"], (common, Mprogress)->
	ApplicationViewModel = Class(common.class.BaseClass,
		id: null,
		model: null,
		hub: null,
		progressBar: null,
		stateName: null,
		poolRestartInProgress: null,
		$props:
			name:
				get: -> this.model.name
			poolName:
				get: ->
					pool = do this.getSitePool
					pool && ("#{pool.name} (#{pool.state})")
			isRunning:
				get: ->
					"yes"
			redisName:
				get: ->
					if this.redisFound
					then "#{this.model.redis.db} (#{this.model.redis.host}:#{this.model.redis.port || ""})"
					else "can't find"
			redisFound:
				get: -> Boolean(this.model.redis)

		constructor: (model, siteHub) ->
			ApplicationViewModel.$super.call this
			this.model = model
			this.hub = siteHub
			this.id = model.id
			return

		getSitePool: ()->
			this.model.pool

		createRowProgressBar: ->
			mProgress = this.progressBar || new Mprogress(
				start: on
				template: 3
				parent: ".mProgressContainerRow.progress-row" + this.id)
			this.progressBar = mProgress
			return

		destroyProgressBar: (force)->
			if (this.progressBar && this.progressBar.status)
				this.progressBar.end force
				delete this.progressBar
			return

# Methods: Public
		restartPool: ->
			if (this.poolRestartInProgress)
				return
			this.poolRestartInProgress = on
			this.stateName = "Restarting..."
			do this.createRowProgressBar
			this.hub.RestartPool this.id
			return

		setState:(newData)->
			$.extend(this.model, newData)
			if (newData.poolRestartState == common.enums.PoolState.Restarting.value)
				do this.createRowProgressBar
			else
				this.poolRestartInProgress = off
				do this.destroyProgressBar
			do this.clearCache
			return

		flushRedis:->
			this.hub.FlushRedis this.id
			return

		startSite:->
			this.hub.StartPool this.id
			return

		stopSite:->
			this.hub.StopPool this.id
			return

# Methods: Public
	)
	ApplicationViewModel