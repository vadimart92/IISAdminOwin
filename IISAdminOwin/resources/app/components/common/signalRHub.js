define(["jquery", "app", "common", "underscore", "signalR"], function($, app, common) {
  var Hub;
  Hub = Class({
    $static: {
      globalConnections: {}
    },
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
    getConnection: function(options, callback) {
      var connection, connectionName, useSharedConnection;
      connectionName = options.rootPath || options.connectionName || "default";
      useSharedConnection = !(options && options.useSharedConnection === false);
      if (useSharedConnection) {
        if (typeof Hub.globalConnections[connectionName] === "undefined") {
          connection = this.initNewConnection(options);
          Hub.globalConnections[connectionName] = connection;
          return connection;
        } else {
          return Hub.globalConnections[connectionName];
        }
      } else {
        return this.initNewConnection(options);
      }
    },
    on: function(event, fn) {
      this.proxy.on(event, fn);
    },
    invoke: function(method, params) {
      var args, def;
      args = arguments;
      if (this.connection.state === $.signalR.connectionState.connected) {
        return this.proxy.invoke.apply(this.proxy, args);
      } else {
        def = $.Deferred();
        this.connection.start().done((function(_this) {
          return function() {
            var innerDef;
            innerDef = _this.proxy.invoke.apply(_this.proxy, args);
            return innerDef.then(def.resolve).fail(def.reject);
          };
        })(this));
        return def.promise();
      }
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
    constructor: function(hubName, options, callback) {
      callback = callback || function() {};
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
        this.connection.error(common.logger.error);
      }
      if (this.connection.state !== 1) {
        this.connect(callback);
      } else {
        callback();
      }
    }
  });
  return Hub;
});
