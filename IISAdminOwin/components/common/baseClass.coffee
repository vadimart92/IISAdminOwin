define [], ->
	BaseClass = Class(
		bind: (func)->
			func.bind(this)
	)
	BaseClass