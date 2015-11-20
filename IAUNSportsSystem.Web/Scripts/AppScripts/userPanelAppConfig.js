(function () {
    'use strict';

    var app = angular.module('userPanelApp');


    app.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

        $locationProvider.html5Mode(false);

        $httpProvider.interceptors.push("error500HttpInterceptor");

        $routeProvider.when("/readiness/competitionsportslist/:competitionId", {
            templateUrl: "/userpanel/readiness/competitionsportslist",
            controller: "ParticipationController",
            resolve: {
                competition: [
                    '$route', 'participationService', function ($route, participationService) {

                        return participationService.getCompetitionSportsList($route.current.params.competitionId).then(function success(data) {
                            return data;
                        },
                            function error(reason) { return false; });
                    }
                ]
            }
        }).when("/readiness/competitionsportslist/confirm/:competitionId", {
            templateUrl: "/userpanel/readiness/confirmcompetitionsportslist",
            controller: "ConfirmReadinessController",
            resolve: {
                data: [
                    '$route', 'redinessService', function ($route, redinessService) {

                        return redinessService.getConfirmCompetitionSportsList($route.current.params.competitionId).then(function success(data) {
                            return data;
                        },
                            function error(reason) { return false; });
                    }
                ]
            }
        }).when("/participation/competitor/:participationId", {
            templateUrl: "/participation/competitor/index",
            controller: "AddCompetitorController",
            resolve: {
                participationData: [
                    '$route', 'participationService', function ($route, participationService) {
                        return participationService.getAddCompetitorData($route.current.params.participationId).then(function success(data) {
                            return data;
                        },
                            function error(reason) { return false; });
                    }
                ]
            }
        }).when("/readiness", {
            templateUrl: "/userpanel/readiness/index",
            controller: "UserPanelReadinessListController",
            resolve: {
                competitionsList: [
                     'participationService', function (participationService) {
                         return participationService.getReadyCompetitionsList().then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            },
            activeRoute: 'readiness'
        }).when("/register", {
            templateUrl: "/userpanel/register/index",
            controller: "UserReadyCompetitionsListController",
            resolve: {
                competitionsList: [
                     'participationService', function (participationService) {
                         return participationService.getUserReadyCompetitionsList().then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            },
            activeRoute: 'register'
        }).when("/register/:competitionId", {
            templateUrl: "/userpanel/register/approvedparticipations",
            controller: "ApprovedCompetitionParticipationsListController",
            resolve: {
                competition: [
                     'participationService', '$route', function (participationService, $route) {
                         return participationService.getApprovedCompetitionParticipationsList($route.current.params.competitionId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/technicalstaff/:participationId", {
            templateUrl: "/userpanel/technicalstaff/index",
            controller: "TechnicalStaffsListController",
            resolve: {
                data: [
                     'technicalStaffService', '$route', function (technicalStaffService, $route) {
                         return technicalStaffService.getList($route.current.params.participationId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/technicalstaff/add/:participationId", {
            templateUrl: "/userpanel/technicalstaff/add",
            controller: "AddTechnicalStaffController",
            resolve: {
                data: [
                     'technicalStaffService', '$route', function (technicalStaffService, $route) {
                         return technicalStaffService.getAddData($route.current.params.participationId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/technicalstaff/edit/:technicalStaffId/:participationId", {
            templateUrl: "/userpanel/technicalstaff/edit",
            controller: "EditTechnicalStaffController",
            resolve: {
                data: [
                     'technicalStaffService', '$route', function (technicalStaffService, $route) {
                         return technicalStaffService.getEditData($route.current.params.technicalStaffId, $route.current.params.participationId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/competitor/:participationId", {
            templateUrl: "/userpanel/competitor/index",
            controller: "CompetitorsListController",
            resolve: {
                data: [
                     'competitorService', '$route', function (competitorService, $route) {
                         return competitorService.getList($route.current.params.participationId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/competitor/edit/:competitorId/:participationId", {
            templateUrl: "/userpanel/competitor/edit",
            controller: "EditCompetitorController",
            resolve: {
                data: [
                     'competitorService', '$route', function (competitorService, $route) {
                         return competitorService.getCompetitor($route.current.params.competitorId, $route.current.params.participationId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/commontechnicalstaff/:competitionId", {
            templateUrl: "/userpanel/commontechnicalstaff/index",
            controller: "CommonTechnicalStaffsListController",
            resolve: {
                data: [
                     'commonTechnicalStaffService', '$route', function (commonTechnicalStaffService, $route) {
                         return commonTechnicalStaffService.getList($route.current.params.competitionId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/commontechnicalstaff/add/:competitionId", {
            templateUrl: "/userpanel/commontechnicalstaff/add",
            controller: "AddCommonTechnicalStaffController",
            resolve: {
                data: [
                     'commonTechnicalStaffService', '$route', function (commonTechnicalStaffService, $route) {
                         return commonTechnicalStaffService.getAddData($route.current.params.competitionId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/commontechnicalstaff/edit/:competitionId/:technicalStaffId", {
            templateUrl: "/userpanel/commontechnicalstaff/edit",
            controller: "EditCommonTechnicalStaffController",
            resolve: {
                data: [
                     'commonTechnicalStaffService', '$route', function (commonTechnicalStaffService, $route) {
                         return commonTechnicalStaffService.getEditData($route.current.params.technicalStaffId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/teamcolor/:participationId", {
            templateUrl: "/userpanel/teamcolor/index",
            controller: "TeamColorController",
            resolve: {
                data: [
                     'participationService', '$route', function (participationService, $route) {
                         return participationService.getTeamColors($route.current.params.participationId).then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/card", {
            templateUrl: "/userpanel/card/index",
            controller: "CardsListController",
            resolve: {
                data: [
                     'cardService', '$route', function (cardService, $route) {
                         return cardService.getCardsList().then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            },
            activeRoute: 'card'
        }).when("/representativeuser/edit", {
            templateUrl: "/userpanel/representativeuser/edit",
            controller: "EditRepresentativeUserController",
            resolve: {
                data: [
                     'representativeUserService', '$route', function (representativeUserService, $route) {
                         return representativeUserService.getRepresentativeUser().then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            }
        }).when("/representativeuser/changepassword", {
            templateUrl: "/userpanel/representativeuser/changepassword",
            controller: "ChangePasswordController"
        }).when("/dashboard", {
            templateUrl: "/userpanel/home/dashboard",
            controller: "DashboardController",
            resolve: {
                data: [
                     'dashboardUserService', '$route', function (dashboardUserService, $route) {
                         return dashboardUserService.getRejectedPersons().then(function success(data) {
                             return data;
                         },
                             function error(reason) { return false; });
                     }
                ]
            },
            activeRoute: 'dashboard'
        });


    }]);

    app.factory("error500HttpInterceptor", ['$q', 'toaster', function ($q, toaster) {
        return {
            'responseError': function (response) {

                toaster.pop('error', '', 'خطایی رخ داده است، لطفا دوباره سعی نمایید');

                if (response.status == 401) {
                    // Handle 401 error code
                }
                if (response.status == 500) {
                    // Handle 500 error code
                    //response.status=400;
                }
                console.log(response);
                // Always reject (or resolve) the deferred you're given
                return $q.reject(response);
            }
        };
    }]);

    app.run([
       '$route', '$rootScope', '$location', function ($route, $rootScope, $location) {

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

           var original = $location.path;
           $location.path = function (path, reload) {
               if (reload === false) {
                   var lastRoute = $route.current;
                   var un = $rootScope.$on('$locationChangeSuccess', function () {
                       $route.current = lastRoute;
                       un();
                   });
               }
               return original.apply($location, [path]);
           };
       }
    ]);


    app.filter('jalaliDate', function () {
        return function (inputDate, format) {
            var date = moment(inputDate);
            return date.format(format);
        }
    });

    app.filter('gender', function () {
        return function (gender, option) {

            var persianGender;
            if (gender == "Male") {

                if (option !== 'plural') {
                    persianGender = "مرد";
                } else {
                    persianGender = "مردان";
                }

            } else {
                if (option !== 'plural') {
                    persianGender = "زن";
                } else {
                    persianGender = "زنان";
                }

            }



            return persianGender;
        }
    });


    app.filter('status', function () {
        return function (status) {

            var persianStatus;
            if (status == true) {

                persianStatus = "تاییدشده";

            } else if (status == false) {
                persianStatus = "تایید نشده";

            } else {
                persianStatus = "بررسی نشده";
            }



            return persianStatus;
        }
    });

    app.directive('stRatio', function () {
        return {
            link: function (scope, element, attr) {
                var ratio = +(attr.stRatio);

                element.css('width', ratio + '%');

            }
        };
    });




    app.directive('stResize', [function () {
        return {
            link: function (scope, element, attr) {

                var isMouseButtonPressed;
                var $resizingElement = undefined;
                var resizingElementStartWidth;
                var mouseCursorStartX;
                var isCursorInResizingPosition;

                var addResizingCursorStyle = function ($element) {
                    $element.css({
                        'cursor': 'col-resize',
                        'user-select': 'none',
                        '-o-user-select': 'none',
                        '-ms-user-select': 'none',
                        '-moz-user-select': 'none',
                        '-khtml-user-select': 'none',
                        '-webkit-user-select': 'none',
                    });
                };

                var removeResizingCursorStyle = function ($element) {
                    $element.css({
                        'cursor': 'default',
                        'user-select': 'text',
                        '-o-user-select': 'text',
                        '-ms-user-select': 'text',
                        '-moz-user-select': 'text',
                        '-khtml-user-select': 'text',
                        '-webkit-user-select': 'text',
                    });
                };

                var canResize = function (e) {
                    return (e.offsetX || e.clientX - $(e.target).offset().left) < 10;
                };




                var tableColumns = element.find('th');


                tableColumns.filter(':not(:last-child)').mousedown(function (e) {

                    $resizingElement = $(this);
                    isMouseButtonPressed = true;
                    mouseCursorStartX = e.pageX;
                    resizingElementStartWidth = $resizingElement.width();

                    //e.stopPropagation();
                });



                tableColumns.mousemove(function (e) {

                    if (canResize(e)) {
                        addResizingCursorStyle($(e.target));
                        isCursorInResizingPosition = true;

                    } else if (!isMouseButtonPressed) {
                        removeResizingCursorStyle($(e.target));
                        isCursorInResizingPosition = false;
                    }

                    if (isCursorInResizingPosition && isMouseButtonPressed) {

                        $resizingElement.width(resizingElementStartWidth + (mouseCursorStartX - e.pageX));
                    }

                });

                $(document).mouseup(function (e) {
                    if (isMouseButtonPressed) {
                        removeResizingCursorStyle($resizingElement);
                        isMouseButtonPressed = false;

                    }

                });



            }
        };
    }]);






})();