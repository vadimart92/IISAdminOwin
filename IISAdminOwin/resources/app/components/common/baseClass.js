define([], function() {
  var BaseClass;
  BaseClass = Class({
    bind: function(func) {
      return func.bind(this);
    }
  });
  return BaseClass;
});
