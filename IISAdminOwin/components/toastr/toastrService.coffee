define ["app", "toaster" ], (app, toaster) ->
	app.service "toasterService", ['toaster', 'Hub', '$', (toaster, Hub, $)->
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

		hubOptions = {
			"positionClass": "toast-bottom-right",
			"timeOut": 5000,
			"preventDuplicates": off
		}

		hub = new Hub('Information',
			useSharedConnection: false
			listeners:
				'info': (msgData) ->
					toaster.info(msgData.msg, msgData.title, hubOptions)
					return
				'success': (msgData) ->
					toaster.success(msgData.msg, msgData.title, hubOptions)
					return
				'warning': (msgData) ->
					toaster.warning msgData.msg, msgData.title, $.extend hubOptions, {timeOut: 10000}
					return
				'error': (msgData) ->
					toaster.error msgData.msg, msgData.title, $.extend hubOptions, {timeOut: 15000}
					return
		)
		do hub.connect

		this.getInfoHub = ->
			hub

		this.showInfo = (text, title)->
			toaster.info(text, title)

		return
	]