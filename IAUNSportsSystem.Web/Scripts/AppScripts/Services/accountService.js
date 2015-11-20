(function () {
    'use strict';

    angular
        .module('accountApp')
        .factory('accountService', accountService);

    accountService.$inject = ['$http'];

    function accountService($http) {


        var registerUser = function (user) {

            return $http.post("/account/register", user)
              .then(function (response) {
                  return response.data;
              });
        }

        var login = function (user) {

            return $http.post("/account/login", user)
              .then(function (response) {
                  return response.data;
              });
        }

        var getCompetitionData = function (competitionId) {

            return $http.get("/competition/home/getcompetitiondata", {
                params: {
                    competitionId: competitionId
                }
            })
              .then(function (response) {
                  return response.data;
              });
        }

        var loginAdmin = function (user) {

            return $http.post("/account/loginadmin", user)
              .then(function (response) {
                  return response.data;
              });
        }

        var service = {
            registerUser: registerUser,
            login: login,
            getCompetitionData: getCompetitionData,
            loginAdmin: loginAdmin
        };

        return service;
    }
})();