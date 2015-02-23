app.constant('$', $).factory('Hub', [
  '$', 'logger', function($, logger) {
    var getConnection, globalConnections, initNewConnection;
    globalConnections = [];
    initNewConnection = function(options) {
      var connection;
      connection = null;
      if (options && options.rootPath) {
        connection = $.hubConnection(options.rootPath, {
          useDefaultPath: false
        });
      } else {
        connection = $.hubConnection();
      }
      connection.logging = options && options.logging ? true : false;
      return connection;
    };
    getConnection = function(options) {
      var useSharedConnection;
      useSharedConnection = !(options && options.useSharedConnection === false);
      if (useSharedConnection) {
        if (typeof globalConnections[options.rootPath] === 'undefined') {
          return globalConnections[options.rootPath] = initNewConnection(options);
        } else {
          return globalConnections[options.rootPath];
        }
      } else {
        return initNewConnection(options);
      }
    };
    return function(hubName, options) {
      var Hub;
      Hub = this;
      Hub.connection = getConnection(options);
      Hub.proxy = Hub.connection.createHubProxy(hubName);
      Hub.on = function(event, fn) {
        Hub.proxy.on(event, fn);
      };
      Hub.invoke = function(method, args) {
        return Hub.proxy.invoke.apply(Hub.proxy, arguments);
      };
      Hub.disconnect = function() {
        Hub.connection.stop();
      };
      Hub.connect = function() {
        var transport;
        transport = options.transport || (Boolean(window.chrome) ? [$.signalR.transports.serverSentEvents.name, $.signalR.transports.longPolling.name] : null);
        return Hub.connection.start(transport ? {
          transport: transport
        } : null);
      };
      if (options && options.listeners) {
        angular.forEach(options.listeners, function(fn, event) {
          Hub.on(event, fn);
        });
      }
      if (options && options.methods) {
        angular.forEach(options.methods, function(method) {
          Hub[method] = function() {
            var args;
            args = $.makeArray(arguments);
            args.unshift(method);
            return Hub.invoke.apply(Hub, args);
          };
        });
      }
      if (options && options.queryParams) {
        Hub.connection.qs = options.queryParams;
      }
      if (options && options.errorHandler) {
        Hub.connection.error(logger.error);
      }
      Hub.promise = Hub.connect();
      return Hub;
    };
  }
]);
