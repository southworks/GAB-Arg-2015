(function (angular, undefined) {
    angular.module('meetpub').service('mobileService', mobileService);

    var msclient = new WindowsAzure.MobileServiceClient("https://meetpub-boot.azure-mobile.net/", "UXFHfEkSKgADsSrnEUpSeedshLXBFE52");
  
    function mobileService($rootScope, $q) {
        this.getUser = getUser;
        this.login = login;
        this.pubs = pubs;
        this.going = going;
        this.addGoing = addGoing;
     
        function list(table, id) {
            if (id == undefined) {
                return msclient.getTable(table).read();
            } else {
                return msclient.getTable(table).lookup(id);
            }
        };

        function getUser() {
            return msclient.currentUser;
        };

        function login(provider) {
            return msclient
                .login(provider);
        };

        function pubs(id) {
            return list('pub', id);
        };

        function going(id) {
            return list('assistance', id);
        }

        function addGoing(id) {
            return msclient.getTable('assistance').insert(
            {
                pubID: id,
                user: msclient.currentUser.id
            });
        }

    };
})(angular);