(function () {
	angular.module('meetpub').service('notificationService', notificationService);

	function notificationService() {
		this.init = init;
	};

	function init() {
		var hub = new WindowsAzure.Messaging.NotificationHub(notificationHubPath, connectionString, '304488334096');

		hub.onPushNotificationReceived = function (msg) {
			alert("Push Notification received: " + msg.message);
		};

		hub.registerApplicationAsync().then(function (result) {
			console.log("Registration successful: " + result.registrationId);
		}, function (err) {
			alert(err);
		});
	};
})(angular);

