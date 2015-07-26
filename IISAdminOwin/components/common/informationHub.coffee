define ["common", "hub"], (common, Hub)->
	InformationHub = Class(common.class.BaseClass,
		$static:
			hub: do ()->
				hub = new Hub("Information",
					connectionName: "information"
					logging: on
					listeners:
						"info": (data) -> common.logger.info data.msg, data.title
						"success": (data) -> common.logger.success data.msg, data.title
						"warning": (data) -> common.logger.warning data.msg, data.title
						"error": (data) -> common.logger.error data.msg, data.title
					methods: []
				)
				hub
	)
	InformationHub