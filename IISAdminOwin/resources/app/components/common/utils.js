app.factory('utils', [
  function() {
    var utils;
    utils = Class.create({
      containsGUID: function(value) {
        return /\b[a-fA-F0-9]{8}(?:-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\b/.test(value);
      }
    });
    return new utils();
  }
]);
