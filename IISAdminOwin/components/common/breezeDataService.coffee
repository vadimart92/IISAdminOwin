define ["breeze", "breeze_ng"], (breeze)->
	BreezeDataService = Class(
		manager: null
		metadataStore: null
		breeze: null
		dataServiceConfig:
			serviceName: "breeze/Name"
			hasServerMetadata: false
		constructor: ->
			this.breeze = breeze
			do breeze.NamingConvention.camelCase.setAsDefault
			do this.configureForBreeze
		##region Private Members

		clearCache: ->
			this.manager.clear()
			return

		configureForBreeze: ->
			breeze.NamingConvention.camelCase.setAsDefault()
			dataService = new (breeze.DataService)(this.dataServiceConfig)
			this.manager = new (breeze.EntityManager)(dataService: dataService)
			this.metadataStore = this.manager.metadataStore
			this.manager.enableSaveQueuing true
			this.configureManagerToSaveModifiedItemImmediately()
			return

		configureManagerToSaveModifiedItemImmediately: ->
			this.manager.entityChanged.subscribe (args) ->
				if args.entityAction == breeze.EntityAction.EntityStateChange
					entity = args.entity
					if entity.entityAspect.entityState.isModified()
						saveEntity entity
				return
			return
		##endregion

		saveFailed: (error) ->
			this.setErrorMessage error
			setTimeout (->
				this.manager.rejectChanges()
				return
			), 1000
			throw error
			return

		saveEntity: (masterEntity) ->
			this.manager.saveChanges().catch this.saveFailed

		setErrorMessage: (error) ->
			stateName = masterEntity.entityAspect.entityState.name.toLowerCase()
			typeName = masterEntity.entityType.shortName
			msg = 'Error saving ' + stateName + ' ' + typeName + ': '
			reason = error.message
			if error.entityErrors
				reason = this.getValidationErrorMessage(error)
			else if isConcurrencyError(error)
				reason = 'can\'t find ' + typeName + '; another user may have deleted it.'
			masterEntity.errorMessage msg + reason
			return

		getValidationErrorMessage: (error) ->
			try
				firstError = error.entityErrors[0]
				return firstError.errorMessage
			catch e
				return "validation error"
			return

		isConcurrencyError: (error) ->
			detail = error.detail
			detail and detail.ExceptionMessage and detail.ExceptionMessage.match(/can't find/i)
	)


	BreezeDataService