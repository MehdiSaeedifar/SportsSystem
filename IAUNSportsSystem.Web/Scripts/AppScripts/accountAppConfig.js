(function () {
    'use strict';

    var app = angular.module('accountApp');

    app.config([
        '$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

            $locationProvider.html5Mode(false);

            $routeProvider.when("/login", {
                templateUrl: "/account/login",
                controller: "LoginUserController"
            }).when("/readiness/:competitionId?", {
                templateUrl: "/account/logincompetition",
                controller: "LoginCompetitionController",
                resolve: {
                    data: [
                        'accountService', '$route', function (accountService, $route) {
                            return accountService.getCompetitionData($route.current.params.competitionId).then(function success(data) {
                                return data;
                            },function error(reason) { return false; });
                        }
                    ]
                }
            }).when("/loginadmin", {
                templateUrl: "/account/loginadmin",
                controller: "LoginAdminController"
            });


        }
    ]);

    app.run(['$rootScope', function ($rootScope) {

        $rootScope.getCssClasses = function (ngModelContoller) {
            return {
                error: ngModelContoller.$invalid && ngModelContoller.$dirty,
                success: ngModelContoller.$valid && ngModelContoller.$dirty
            };
        };

        $rootScope.isDirty = function (ngModelContoller) {
            return ngModelContoller.$dirty;
        }

        $rootScope.hasError = function (ngModelContoller) {

            return ngModelContoller.$invalid && ngModelContoller.$dirty;
        }

        $rootScope.showError = function (ngModelController, error) {
            return ngModelController.$error[error];
        };

        $rootScope.isInvalid = function (ngModelController) {
            return ngModelController.$invalid;
        }

    }]);


})();