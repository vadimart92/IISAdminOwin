define ["jquery", "app", "common", "underscore", "signalR"], ($, app, common)->
	Hub = Class({
		globalConnections: []
		initNewConnection:(options) ->
			connection = null
			if options and options.rootPath
				connection = $.hubConnection(options.rootPath, useDefaultPath: false)
			else
				connection = $.hubConnection()
			connection.logging = if options and options.logging then true else false
			connection
		getConnection: (options) ->
			useSharedConnection = !(options and options.useSharedConnection == false)
			if useSharedConnection
				if typeof this.globalConnections[options.rootPath] == "undefined" then (this.globalConnections[options.rootPath] = this.initNewConnection(options)) else this.globalConnections[options.rootPath]
			else
				initNewConnection options
		on: (event, fn) ->
			this.proxy.on event, fn
			return
		invoke: (method, args) ->
			this.proxy.invoke.apply this.proxy, arguments
		disconnect: ->
			this.connection.stop()
			return
		connect: (callback) ->
			transport = this.options.transport || if Boolean(window.chrome) then transport: [
				$.signalR.transports.serverSentEvents.name
				$.signalR.transports.longPolling.name
			] else null
			this.connection.start(transport, callback)
		promise: -> this.connect()
		constructor: (hubName, options)->
			this.options = options
			this.connection = this.getConnection(options)
			this.proxy = this.connection.createHubProxy(hubName)
			if options and options.listeners
				_.each options.listeners, ((fn, event) -> this.on(event, fn)), this
			if options and options.methods
				_.each options.methods, ((method) ->
					this[method] = ->
						args = $.makeArray(arguments)
						args.unshift method
						this.invoke.apply this, args
					return), this
			if options and options.queryParams
				this.connection.qs = options.queryParams
			if options and options.errorHandler
				this.connection.error common.logger.error
	})
	Hub
