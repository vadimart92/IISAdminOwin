define(["jquery", "app", "common", "underscore", "signalR"], function($, app, common) {
  var Hub;
  Hub = Class({
    globalConnections: [],
    initNewConnection: function(options) {
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
    },
    getConnection: function(options) {
      var useSharedConnection;
      useSharedConnection = !(options && options.useSharedConnection === false);
      if (useSharedConnection) {
        if (typeof this.globalConnections[options.rootPath] === "undefined") {
          return this.globalConnections[options.rootPath] = this.initNewConnection(options);
        } else {
          return this.globalConnections[options.rootPath];
        }
      } else {
        return initNewConnection(options);
      }
    },
    on: function(event, fn) {
      this.proxy.on(event, fn);
    },
    invoke: function(method, args) {
      return this.proxy.invoke.apply(this.proxy, arguments);
    },
    disconnect: function() {
      this.connection.stop();
    },
    connect: function(callback) {
      var transport;
      transport = this.options.transport || (Boolean(window.chrome) ? {
        transport: [$.signalR.transports.serverSentEvents.name, $.signalR.transports.longPolling.name]
      } : null);
      return this.connection.start(transport, callback);
    },
    promise: function() {
      return this.connect();
    },
    constructor: function(hubName, options) {
      this.options = options;
      this.connection = this.getConnection(options);
      this.proxy = this.connection.createHubProxy(hubName);
      if (options && options.listeners) {
        _.each(options.listeners, (function(fn, event) {
          return this.on(event, fn);
        }), this);
      }
      if (options && options.methods) {
        _.each(options.methods, (function(method) {
          this[method] = function() {
            var args;
            args = $.makeArray(arguments);
            args.unshift(method);
            return this.invoke.apply(this, args);
          };
        }), this);
      }
      if (options && options.queryParams) {
        this.connection.qs = options.queryParams;
      }
      if (options && options.errorHandler) {
        return this.connection.error(common.logger.error);
      }
    }
  });
  return Hub;
});
