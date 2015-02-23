app.constant('$', $)
.factory 'Hub', [
  '$'
  'logger'
  ($, logger) ->
    #This will allow same connection to be used for all Hubs
    #It also keeps connection as singleton.
    globalConnections = []
    initNewConnection = (options) ->
      connection = null
      if options and options.rootPath
        connection = $.hubConnection(options.rootPath, useDefaultPath: false)
      else
        connection = $.hubConnection()
      connection.logging = if options and options.logging then true else false
      connection

    getConnection = (options) ->
      useSharedConnection = !(options and options.useSharedConnection == false)
      if useSharedConnection
        if typeof globalConnections[options.rootPath] == 'undefined' then (globalConnections[options.rootPath] = initNewConnection(options)) else globalConnections[options.rootPath]
      else
        initNewConnection options

    (hubName, options) ->
      Hub = this
      Hub.connection = getConnection(options)
      Hub.proxy = Hub.connection.createHubProxy(hubName)

      Hub.on = (event, fn) ->
        Hub.proxy.on event, fn
        return

      Hub.invoke = (method, args) ->
        Hub.proxy.invoke.apply Hub.proxy, arguments

      Hub.disconnect = ->
        Hub.connection.stop()
        return

      Hub.connect = ->
        transport = options.transport || if Boolean(window.chrome) then [
          $.signalR.transports.serverSentEvents.name
          $.signalR.transports.longPolling.name
        ] else null
        Hub.connection.start if transport then transport:  transport else null

      if options and options.listeners
        angular.forEach options.listeners, (fn, event) ->
          Hub.on event, fn
          return
      if options and options.methods
        angular.forEach options.methods, (method) ->

          Hub[method] = ->
            args = $.makeArray(arguments)
            args.unshift method
            Hub.invoke.apply Hub, args

          return
      if options and options.queryParams
        Hub.connection.qs = options.queryParams
      if options and options.errorHandler
        Hub.connection.error logger.error
      #Adding additional property of promise allows to access it in rest of the application.
      Hub.promise = Hub.connect()
      Hub
]
