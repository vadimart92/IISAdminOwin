# CoffeeScript
app.controller "addSiteController",['$rootScope', '$scope', '$timeout', 'Hub', '$', ($rootScope, $scope, $timeout, Hub, $) ->
  vm = $scope

  Site = Class.create(
    name: null,
    redis: 0,
    workUri: null,
    db: null,
    msSqlInstances: [],
    releaseInfo: {},
    isFormDataValid: () ->
      this.name && this.redis && this.workUri && this.db
  )

  vm.site = new Site();

  vm.hub = new Hub('SiteCreate',
    listeners: []
    methods: [
      'AddSite'
      'GetReleaseInfo'
      'GetStartupInfo'
    ]
  )

  vm.releaseInfoFields = [
    { key: 'createdOn', type: 'input', templateOptions: {label: 'Created on', disabled: on }}
    { key: 'name', type: 'input', templateOptions: {label: 'Build name', disabled: on }}
    { key: 'buildFolderLink', type: 'input', templateOptions: {label: 'Build file link', disabled: on }}
    { key: 'version', type: 'input', templateOptions: {label: 'Version', disabled: on }}
    { key: 'release', type: 'checkbox', templateOptions: {label: 'Release', disabled: on }}
  ]

  vm.siteFields = [
    {
      key: 'workUri',
      type: 'input',
      templateOptions: {
        label: 'Build uri',
        placeholder: 'Paste product build uri here'
        required: true
      }
      validators: uri:
        expression: (viewValue, modelValue) ->
          value = modelValue or viewValue
          /{\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b}/.test value
        message: '$viewValue + " is not a valid build URI"'
      watcher: listener: (field, newValue, oldValue, scope, stopWatching) ->
        vm.site.releaseInfo = {}
        if newValue
          vm.updateReleaseInfo newValue
        return
    },
    { key: 'name', type: 'input', templateOptions: {label: 'Name' }}
    {
      key: 'db', type: 'uiSelect',
      templateOptions: {
        redisKey: 'redis',
        label: 'MSSQL Instance'
      }
    }
  ]

  vm.setSqlInstances = (list)->
    vm.site.msSqlInstances = list
    return

  getSiteCreateInfo = ()->
    vm.hub.GetStartupInfo()
    .then (siteInfo)->
      $scope.$apply ()->
        vm.site.redis = siteInfo.freeRedisDbNum
        vm.setSqlInstances siteInfo.sqlServerInstances

  vm.updateReleaseInfo = (uri)->
    vm.hub.GetReleaseInfo uri
    .then (data)-> $scope.$apply ()-> $.extend vm.site.releaseInfo, data
    return

  vm.addSite = ()->
    vm.hub.AddSite vm.site
    return

  $timeout ()->
    do getSiteCreateInfo
    vm.site.workUri = "{7D53CBC8-E052-4A7A-9419-E7FF5D6AFE7E}"
  ,1000

  offFunc = $rootScope.$on '$stateChangeStart', (event, toState, toParams, fromState, fromParams) ->
    selfDestruct = offFunc;
    do vm.hub.disconnect
    do selfDestruct
    hideAllProgressBars true
    return

  return
]