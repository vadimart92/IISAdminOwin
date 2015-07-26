define ["common",
				"mProgress"], (common, Mprogress)->
	SiteViewModel = Class(common.class.BaseClass,
		id: null,
		model: null,
		hub: null,
		controller: null,
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
			bindings:
				get: ->
					this.model.bindings.join "; "
			state:
				get: ->
					this.model.state
			isSiteRunning:
				get: ->
					this.model.state == common.enums.SiteState.Started.value
			redisName:
				get: ->
					if this.redisFound
					then "#{this.model.redis.db} (#{this.model.redis.host}:#{this.model.redis.port || ""})"
					else "can't find"
			redisFound:
				get: -> Boolean(this.model.redis)

		constructor: (model, siteHub, siteController) ->
			SiteViewModel.$super.call this
			this.model = model
			this.hub = siteHub
			this.id = model.id
			this.controller = siteController
			return

		getSitePool: ()->
			firstApp = this.model.applications[0]
			firstApp && firstApp.pool

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
			this.hub.StartSite this.id
			return

		stopSite:->
			this.hub.StopSite this.id
			return

		# Methods: Public
	)
	SiteViewModel