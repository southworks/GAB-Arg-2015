(function () {
    angular.module('meetpub').service('$cordova', ['$document', '$timeout', '$window', '$q', cordovaService]);

    function cordovaService($document, $timeout, $window, $q) {
        var defer = $q.defer();
        this.ready = defer.promise;

        var timoutPromise = $timeout(function () {
            if ($window.cordova) {
                defer.resolve($window.cordova);
            } else {
                defer.reject("Cordova failed to load");
            }
        }, 1200);

        angular.element($document)[0].addEventListener('deviceready', function () {
            $timeout.cancel(timoutPromise);
            defer.resolve($window.cordova);
        });
    };
})(angular);