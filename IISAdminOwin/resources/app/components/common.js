define(["./common/enums", "./common/baseController", "./common/stateFullController", "./common/utils", "./common/logger", "./common/breezeDataService"], function(enums, baseController, stateFullController, utils, logger, BreezeDataService) {
  return {
    enums: enums,
    "class": {
      BaseController: baseController,
      StateFullController: stateFullController,
      BreezeDataService: BreezeDataService
    },
    utils: utils,
    logger: logger
  };
});
