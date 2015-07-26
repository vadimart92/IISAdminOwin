define ["./common/enums"
				"./common/baseController"
				"./common/stateFullController"
				"./common/utils"
				"./common/logger"
				"./common/breezeDataService"
				"./common/baseClass"
	],
	(enums, baseController, stateFullController, utils, logger, BreezeDataService, baseClass)->
		enums: enums
		class:
			BaseController: baseController
			StateFullController: stateFullController
			BreezeDataService: BreezeDataService
			BaseClass: baseClass
		utils: utils
		logger: logger
