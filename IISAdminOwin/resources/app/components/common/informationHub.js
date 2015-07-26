define(["common", "hub"], function(common, Hub) {
  var InformationHub;
  InformationHub = Class(common["class"].BaseClass, {
    $static: {
      hub: (function() {
        var hub;
        hub = new Hub("Information", {
          connectionName: "information",
          logging: true,
          listeners: {
            "info": function(data) {
              return common.logger.info(data.msg, data.title);
            },
            "success": function(data) {
              return common.logger.success(data.msg, data.title);
            },
            "warning": function(data) {
              return common.logger.warning(data.msg, data.title);
            },
            "error": function(data) {
              return common.logger.error(data.msg, data.title);
            }
          },
          methods: []
        });
        return hub;
      })()
    }
  });
  return InformationHub;
});
