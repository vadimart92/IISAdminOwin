define ["common"], (common)->
	AppDataService = Class(common.class.BreezeDataService,
		constructor: ->
			this.dataServiceConfig =
				serviceName: "breeze/App"
				hasServerMetadata: false
			AppDataService.$super.call(this)
		getApplications: (siteListsObservable, errorObservable) ->
			getSucceeded = (data) ->
				siteListsObservable data.results
				return
			getFailed = (error) ->
				errorObservable 'Error retrieving site lists: ' + error.message
				return
			this.breeze.EntityQuery.from("AppList").using(this.manager).execute().then getSucceeded, getFailed
	)
	AppDataService