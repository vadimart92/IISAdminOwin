define ["common"], (common)->
	SiteDataService = Class(common.class.BreezeDataService,
		constructor: ->
			this.dataServiceConfig =
				serviceName: "breeze/Site"
				hasServerMetadata: false
			SiteDataService.$super.call(this)
		getSites: (siteListsObservable, errorObservable) ->
			getSucceeded = (data) ->
				siteListsObservable data.results
				return
			getFailed = (error) ->
				errorObservable 'Error retrieving site lists: ' + error.message
				return
			this.breeze.EntityQuery.from("SiteList").using(this.manager).execute().then getSucceeded, getFailed
	)
	SiteDataService