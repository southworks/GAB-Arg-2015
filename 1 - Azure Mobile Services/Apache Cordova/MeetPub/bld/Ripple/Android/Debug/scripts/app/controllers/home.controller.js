(function () {
    angular.module('meetpub').controller('HomeCtrl', mainController);

    function mainController($scope, sharedService, mobileService, $rootScope) {
        var vm = this;
        angular.extend(vm, sharedService);
        vm.today = "Hoy 25/04/2015";
        vm.user = mobileService.getUser();
        mobileService.pubs().then(function (items) {
            $rootScope.$apply(function () {
                vm.pubs = [];
                angular.forEach(items, function (value) {
                    vm.pubs.push(new Pub(value));
                });
            });
        });
    };
})(angular);