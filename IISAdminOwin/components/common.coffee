define ["./common/enums"
				"./common/baseController"
				"./common/stateFullController"
				"./common/utils"
				"./common/logger"],
	(enums, baseController, stateFullController, utils, logger)->
		enums: enums
		class:
			BaseController: baseController
			StateFullController: stateFullController
		utils: utils
		logger: logger
