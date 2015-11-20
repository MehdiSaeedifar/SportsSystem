(function () {
    'use strict';

    var accountApp = angular
        .module('accountApp');

    accountApp.controller('LoginUserController', LoginUserController);

    LoginUserController.$inject = ['$scope', 'accountService', 'toaster', '$timeout', '$window'];

    function LoginUserController($scope, accountService, toaster, $timeout, $window) {
        $scope.captchaImage = {
            url: "/captcha/index/?rndDate =" + new Date().getTime()
        }
        $scope.refreshCaptcha = function () {

            $scope.captchaImage.url = "/captcha/index/?rndDate =" + new Date().getTime();
        }

        $scope.user = {};
        $scope.errors = [];

        $scope.redirectUrls = {};

        $scope.$watch("redirectUrls.userPanelUrl", function () {

            if ($scope.redirectUrls.userPanelUrl != null && $scope.redirectUrls.userPanelUrl != "") {
                $window.location = $scope.redirectUrls.userPanelUrl;
            }

        });

        $scope.login = function () {

            var toasterMessage = toaster.pop('info', '', 'لطفا منتظر بمانید...');

            accountService.login($scope.user).then(function onSuccess(data) {

                toaster.clear(toasterMessage);

                toaster.pop("success", "", 'در حال ورود به سیستم، لطفا منتظر بمانید...');


                $timeout(function () {

                    $window.location.href = "/userpanel/#/dashboard";

                }, 1000);

            }, function onError(errorData) {

                toaster.clear(toasterMessage);

                $scope.errors = errorData.data;
                $scope.user.captchaInputText = null;
                $scope.user.password = null;
                $scope.refreshCaptcha();

            });
        }
    }

    accountApp.controller('LoginCompetitionController', ['$scope', 'data', '$sce', '$modal', function ($scope, data, $sce, $modal) {

        $scope.competition = data;

        $scope.competition.rule = $sce.trustAsHtml($scope.competition.rule);

        $scope.showAnnouncement = function (announcement) {

            var modalInstance = $modal.open({
                templateUrl: 'showAnnouncement.html',
                controller: 'ShowAnnouncementController',
                resolve: {
                    announcement: function () {
                        //technicalStaff.competz = $scope.participationId;
                        return announcement;
                    }
                }
            });

            modalInstance.result.then(function () {

            }, function () {

            });

        }

    }

    ]);

    accountApp.controller('ShowAnnouncementController', [
        '$scope', 'announcement', '$modalInstance', '$sce', function ($scope, announcement, $modalInstance, $sce) {

            $scope.announcement = angular.copy(announcement);

            $scope.announcement.websiteText = $sce.trustAsHtml($scope.announcement.websiteText);

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };

        }
    ]);


    accountApp.controller('LoginAdminController', [
        '$scope', 'accountService', 'toaster', '$timeout', '$window', function ($scope, accountService, toaster, $timeout, $window) {

            $scope.captchaImage = {
                url: "/captcha/index/?rndDate =" + new Date().getTime()
            }
            $scope.refreshCaptcha = function () {

                $scope.captchaImage.url = "/captcha/index/?rndDate =" + new Date().getTime();
            }

            $scope.user = {};
            $scope.errors = [];

            $scope.redirectUrls = {};

            $scope.$watch("redirectUrls.userPanelUrl", function () {

                if ($scope.redirectUrls.userPanelUrl != null && $scope.redirectUrls.userPanelUrl != "") {
                    $window.location = $scope.redirectUrls.userPanelUrl;
                }

            });

            $scope.login = function () {

                var toasterMessage = toaster.pop('info', '', 'لطفا منتظر بمانید...');

                accountService.loginAdmin($scope.user).then(function onSuccess(data) {

                    toaster.clear(toasterMessage);

                    toaster.pop("success", "", 'در حال ورود به سیستم، لطفا منتظر بمانید...');


                    $timeout(function () {

                        $window.location.href = "/admin/#/";

                    }, 1000);

                }, function onError(errorData) {

                    toaster.clear(toasterMessage);

                    $scope.errors = errorData.data;
                    $scope.user.captchaInputText = null;
                    $scope.user.password = null;
                    $scope.refreshCaptcha();

                });
            }

        }
    ]);




})();
