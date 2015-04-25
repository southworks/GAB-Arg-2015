angular
    .module('meetpub', ['ngRoute', 'leaflet-directive'])
    .config(['$routeProvider',
          function ($routeProvider) {
              $routeProvider.
                when('/', {
                    templateUrl: 'scripts/app/partials/home.html',
                    controller: 'HomeCtrl as home'
                }).
                when('/pub/:pubId', {
                    templateUrl: 'scripts/app/partials/details.html',
                    controller: 'DetailsCtrl as details'
                }).
                when('/init', {
                    templateUrl: 'scripts/app/partials/loading.html',
                    controller: 'LoadingCtrl as loading'
                }).
                  when('/error', {
                      templateUrl: 'scripts/app/partials/error.html',
                      controller: 'ErrorCtrl as error'
                  }).
                  when('/login', {
                      templateUrl: 'scripts/app/partials/login.html',
                      controller: 'LoginCtrl as login'
                  }).
                otherwise({
                    redirectTo: '/init'
                });
          }
    ])
    .run(function ($rootScope, $location, mobileService) {
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (mobileService.getUser() == null) {
                $location.path("/login");
            }
        });
    });

