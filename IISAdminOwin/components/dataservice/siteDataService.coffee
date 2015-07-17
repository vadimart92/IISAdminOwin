define ["breeze"], (breeze)->
	siteDataService = ($http, $q, $timeout) ->
		do breeze.NamingConvention.camelCase.setAsDefault
		manager = undefined
		metadataStore = undefined
		##region Private Members

		clearCache = ->
			manager.clear()
			return

		configureForBreeze = ->
			breeze.NamingConvention.camelCase.setAsDefault()
			dataService = new (breeze.DataService)(
				serviceName: 'breeze/Site'
				hasServerMetadata: false)
			manager = new (breeze.EntityManager)(dataService: dataService)
			metadataStore = manager.metadataStore
			manager.enableSaveQueuing true
			configureManagerToSaveModifiedItemImmediately()
			return

		configureManagerToSaveModifiedItemImmediately = ->
			manager.entityChanged.subscribe (args) ->
				if args.entityAction == breeze.EntityAction.EntityStateChange
					entity = args.entity
					if entity.entityAspect.entityState.isModified()
						saveEntity entity
				return
			return

		getSites = (siteListsObservable, errorObservable) ->
			getSucceeded = (data) ->
				siteListsObservable data.results
				return

			getFailed = (error) ->
				errorObservable 'Error retrieving todo lists: ' + error.message
				return

			breeze.EntityQuery.from('SiteList').using(manager).execute().then(getSucceeded).catch(getFailed)

		saveEntity = (masterEntity) ->
			saveFailed = (error) ->
				setErrorMessage error
				# Let them see it "wrong" briefly before reverting"
				setTimeout (->
					manager.rejectChanges()
					return
				), 1000
				throw error
				# so caller can see failure
				return

			setErrorMessage = (error) ->
				stateName = masterEntity.entityAspect.entityState.name.toLowerCase()
				typeName = masterEntity.entityType.shortName
				msg = 'Error saving ' + stateName + ' ' + typeName + ': '
				reason = error.message
				if error.entityErrors
					reason = getValidationErrorMessage(error)
				else if isConcurrencyError(error)
					reason = 'can\'t find ' + typeName + '; another user may have deleted it.'
				masterEntity.errorMessage msg + reason
				return

			getValidationErrorMessage = (error) ->
				try
	# return the first error message
					firstError = error.entityErrors[0]
					return firstError.errorMessage
				catch e
	# ignore problem extracting error message
					return 'validation error'
				return

			isConcurrencyError = (error) ->
				detail = error.detail
				detail and detail.ExceptionMessage and detail.ExceptionMessage.match(/can't find/i)

			manager.saveChanges().catch saveFailed

		configureForBreeze()


		service =
			clearCache: clearCache
			metadataStore: metadataStore
			getSites: getSites
		service
	##endregion

siteDataService.$inject = ["$http"
													 "$q"
													 "$timeout"]
app.factory "siteDataService", siteDataService