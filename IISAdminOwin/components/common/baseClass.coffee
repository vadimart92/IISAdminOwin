define [], ()->
	BaseClass = Class(
		cache: {}
		constructor: ->
			for propName of this
				value = this[propName]
				if typeof value == 'function'
					this[propName] = value.bind this
			return

		bind: (func)->
			func.bind this

		clearCache: ->
			this.cache = {}
	)
	BaseClass