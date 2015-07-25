define([], function() {
  var utils;
  utils = Class({
    containsGUID: function(value) {
      return /\b[a-fA-F0-9]{8}(?:-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\b/.test(value);
    }
  });
  return new utils();
});
