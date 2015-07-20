define ["toaster", "app", "angular"], (toaster, app, angular)->
	Logger = Class(
		toaster: null
		$log: null
		constructor: ($log, toaster)->
			this.toaster = toaster
			do this.initToaster
			return
		initToaster: ()->
			toaster.options.timeOut = 2000
			toaster.options.positionClass = 'toast-bottom-right'
		error: (message, title) ->
			this.toaster.error message, title
			this.$log.error 'Error: ' + message
			return
		info: (message, title) ->
			this.toaster.info message, title
			this.$log.info 'Info: ' + message
			return
		log: (message) ->
			this.$log.log message
			return
		success: (message, title) ->
			this.toaster.success message, title
			this.$log.info 'Success: ' + message
			return
		warning: (message, title) ->
			this.toaster.warning message, title
			this.$log.warn 'Warning: ' + message
			return
	)
	$injector = angular.injector(["ng"])
	ngLog = $injector.get("$log")
	new Logger(ngLog, toaster)
