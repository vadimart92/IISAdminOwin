app.factory 'logger', [
	'$log'
	'toaster'
	($log, toaster) ->
		error = (message, title) ->
			toaster.error message, title
			$log.error 'Error: ' + message
			return

		info = (message, title) ->
			toaster.info message, title
			$log.info 'Info: ' + message
			return

		log = (message) ->
			$log.log message
			return

		success = (message, title) ->
			toaster.success message, title
			$log.info 'Success: ' + message
			return

		warning = (message, title) ->
			toaster.warning message, title
			$log.warn 'Warning: ' + message
			return

		toaster.options.timeOut = 2000

		toaster.options.positionClass = 'toast-bottom-right'
		logger =
			error: error
			info: info
			log: log
			success: success
			warning: warning
		logger
]
