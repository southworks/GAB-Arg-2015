(function () {
    angular.module('meetpub').controller('DetailsCtrl', detailsController);

    function detailsController($scope, $routeParams, sharedService, mobileService, $rootScope) {
        var vm = this;
        angular.extend(vm, sharedService);
        vm.PubId = $routeParams.pubId;
        mobileService.pubs(vm.PubId).then(function (item) {
            $rootScope.$apply(function() {
                vm.Pub = new Pub(item);
            });
        });

        vm.Going = function() {
            mobileService.addGoing(vm.PubId).then(function () {
                vm.GoTo("/");
			}, function (err) {
			    alert("Error:" + err);
			});
        };
    };

})(angular);