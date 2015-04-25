(function () {
	angular.module('meetpub').controller('ErrorCtrl', errorController);

	function errorController(sharedService) {
		angular.extend(this, sharedService);
	};
})(angular);