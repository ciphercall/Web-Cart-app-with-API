// Creating a module
var userApp = angular.module("userApp", []);

// Creating a factory from the module
userApp.factory('userFactory', function($http) {
    return {
        getFormData: function(callback) {
            $http.get('api/ApiLevelEntry').success(callback);
        }
    };
});

// Creating a controller from the module
userApp.controller("ApiLevelEntry", function ($scope, userFactory) {
    getFormData();
    function getFormData() {
        userFactory.getFormData(function(results) {
            $scope.new = results;

        });
    }

    $scope.add = function() {
        $scope.message = "User Data Submited";
    };
})