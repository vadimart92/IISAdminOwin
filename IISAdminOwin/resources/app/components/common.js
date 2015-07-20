define(["./common/enums", "./common/baseController", "./common/stateFullController", "./common/utils", "./common/logger"], function(enums, baseController, stateFullController, utils, logger) {
  return {
    enums: enums,
    "class": {
      BaseController: baseController,
      StateFullController: stateFullController
    },
    utils: utils,
    logger: logger
  };
});
