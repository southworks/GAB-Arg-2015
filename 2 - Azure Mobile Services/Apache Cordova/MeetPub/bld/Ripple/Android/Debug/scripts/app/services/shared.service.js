(function () {
    angular.module('meetpub').service('sharedService', sharedService);

    function sharedService($location) {
        this.GoTo = function (value) {
            $location.path(value);
        };
    };

})(angular);