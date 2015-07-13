app.factory 'utils', [() ->
		utils = Class.create(
			containsGUID: (value)-> return /\b[a-fA-F0-9]{8}(?:-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\b/.test(value)
		)
		new utils()
]
