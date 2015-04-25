(function (angular) {
    angular.module('meetpub').controller('LoginCtrl', loginController);

    function loginController(sharedService, mobileService, $rootScope) {
        var vm = this;

        angular.extend(vm, sharedService);
        vm.user = mobileService.getUser();

        if (mobileService.getUser() !== null) {
           vm.GoTo('/');
        }

        this.login = function () {
            mobileService.login("facebook").then(function () {
                $rootScope.$apply(function () {
                    vm.GoTo("/");
                });
            });
        };

    };
})(angular);