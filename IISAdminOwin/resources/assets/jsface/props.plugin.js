(function(){
	'use strict';
	if (Class === undefined || Class.plugins === undefined){
		return;
	}
	Class.plugins.$props = function (clazz, parent, api) {
		var key, props = api.$props;
		for (key in props) {
			if (props.hasOwnProperty(key)){
				var config = props[key];
				Object.defineProperty(clazz.prototype, key, config);
			}
		}
	};
})(window);
