define([], function() {
  var BaseClass;
  BaseClass = Class({
    cache: {},
    constructor: function() {
      var propName, value;
      for (propName in this) {
        value = this[propName];
        if (typeof value === 'function') {
          this[propName] = value.bind(this);
        }
      }
    },
    bind: function(func) {
      return func.bind(this);
    },
    clearCache: function() {
      return this.cache = {};
    }
  });
  return BaseClass;
});
