(function () {
    angular.module('meetpub').controller('LoadingCtrl', loadingController);

    function loadingController($cordova, $location, notificationService) {
        $cordova.ready.then(function resolved(resp) {
            notificationService.init();
            $location.path('/');
        }, function rejected(resp) {
            $location.path('/error');
        });
    };
})(angular);