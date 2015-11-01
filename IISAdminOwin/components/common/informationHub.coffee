define ["common", "hub", "toaster", "jquery"], (common, Hub, toaster, $)->
	InformationHub = Class(common.class.BaseClass,
		constructor: ->
			InformationHub.$super.constructor.call(this);
			do this.init
		init: ->
			do this.initToasterOptions
			do this.hub.connect
		hub: new Hub("Information",
			connectionName: "information"
			logging: on
			listeners:
				"info": (data) ->
					common.logger.info data.msg, data.title
					toaster.info(data.msg, data.title, this.hubOptions)
				"success": (data) ->
					common.logger.success data.msg, data.title
					toaster.success(data.msg, data.title, this.hubOptions)
				"warning": (data) ->
					common.logger.warning data.msg, data.title
					toaster.warning data.msg, data.title, $.extend this.hubOptions, {timeOut: 10000}
				"error": (data) ->
					common.logger.error data.msg, data.title
					toaster.error data.msg, data.title, $.extend this.hubOptions, {timeOut: 15000}
			methods: []
		)
		hubOptions: {
			"positionClass": "toast-bottom-right",
			"timeOut": 5000,
			"preventDuplicates": off
		},
		initToasterOptions: ->
			toaster.options = {
				"closeButton": on,
				"debug": off,
				"newestOnTop": on,
				"progressBar": on,
				"positionClass": "toast-top-right",
				"preventDuplicates": on,
				"onclick": null,
				"showDuration": "200",
				"hideDuration": "1000",
				"timeOut": "5000",
				"extendedTimeOut": "1000",
				"showEasing": "swing",
				"hideEasing": "linear",
				"showMethod": "fadeIn",
				"hideMethod": "fadeOut"
			}
	)
	new InformationHub()