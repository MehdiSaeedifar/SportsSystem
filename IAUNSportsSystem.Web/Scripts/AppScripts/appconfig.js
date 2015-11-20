(function () {
    'use strict';

    var app = angular.module('app');


    app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $locationProvider.html5Mode(false);

        $routeProvider.when("/competition", {
            templateUrl: "/admin/competition/index",
            controller: "CompetitionsListController",
            resolve: {
                data: ['competitionService', '$route', function (competitionService, $route) {

                    return competitionService.getAll().then(function success(data) { return data; },
                    function error(reason) { return false; });
                }]
            }
        }).when("/competition/edit/:competitionId", {
            templateUrl: "/admin/competition/edit",
            controller: "EditCompetitionController",
            resolve: {
                data: ['competitionService', '$route', function (competitionService, $route) {

                    return competitionService.getEditData($route.current.params.competitionId).then(function success(data) { return data; },
                    function error(reason) { return false; });
                }]
            }
        }).when('/competition/add', {
            templateUrl: '/admin/competition/add',
            controller: 'AddCompetitionController',
        })
            .when('/competitionsport/add/:competitionId', {
                templateUrl: '/admin/competitionsport/add',
                controller: 'AddCompetitionSportController',
                resolve: {

                    data: ['competitionSportService', '$route', function (competitionSportService, $route) {

                        return competitionSportService.getAddData($route.current.params.competitionId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/readiness/:competitionId?', {
                templateUrl: '/admin/readiness/index',
                controller: 'ReadinessController',
                resolve: {

                    competitionsList: ['competitionService', function (competitionService) {

                        return competitionService.getCompetitionListForReadiness().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/readiness/users/:competitionSportId', {
                templateUrl: '/admin/readiness/userlist',
                controller: 'RedinessUserController',
                resolve: {

                    data: ['$route', 'readinessService', function ($route, readinessService) {

                        return readinessService.getCompetitionSportUsers($route.current.params.competitionSportId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/readiness/competitors/:participationId', {
                templateUrl: '/admin/readiness/competitorslist',
                controller: 'RedinessCompetitorsListController',
                resolve: {

                    competitorsList: ['$route', 'readinessService', function ($route, readinessService) {
                        return readinessService.getCompetitorsList($route.current.params.participationId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
                reloadOnSearch: false
            }).when('/sport', {
                templateUrl: '/admin/sport/index',
                controller: 'SportsListController',
                resolve: {

                    sports: ['readinessService', function (readinessService) {

                        return readinessService.getSportsList().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/sport/add', {
                templateUrl: '/admin/sport/add',
                controller: 'AddSportController'
            }).when('/sport/edit/:sportId', {
                templateUrl: '/admin/sport/edit',
                controller: 'EditSportController',
                resolve: {
                    sport: ['$route', 'readinessService', function ($route, readinessService) {
                        return readinessService.getSportWithCategoriesAndDetails($route.current.params.sportId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/university', {
                templateUrl: '/admin/university/index',
                controller: 'UniversitiesListController',
                resolve: {
                    universities: ['readinessService', function (readinessService) {
                        return readinessService.getUniversitiesList().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/studyfield', {
                templateUrl: '/admin/studyfield/index',
                controller: 'StudyFieldsListController',
                resolve: {
                    studyFields: ['readinessService', function (readinessService) {
                        return readinessService.getStudyFieldsList().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/studyfielddegree', {
                templateUrl: '/admin/studyfielddegree/index',
                controller: 'StudyFieldDegreesListController',
                resolve: {
                    studyFieldDegrees: ['readinessService', function (readinessService) {
                        return readinessService.getStudyFieldDegreesList().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/user', {
                templateUrl: '/admin/user/index',
                controller: 'UsersListController',
                resolve: {
                    users: ['readinessService', function (readinessService) {
                        return readinessService.getUsersList().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/user/add', {
                templateUrl: '/admin/user/add',
                controller: 'AddUserController',

            }).when('/user/edit/:userId', {
                templateUrl: '/admin/user/edit',
                controller: 'EdituserController',
                resolve: {
                    user: ['readinessService', '$route', function (readinessService, $route) {
                        return readinessService.getUser($route.current.params.userId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/slideshow', {
                templateUrl: '/admin/slideshow/index',
                controller: 'SlideShowsListController',
                resolve: {
                    slides: ['slideShowService', function (slideShowService) {
                        return slideShowService.getSlideShowsList().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/slideshow/add', {
                templateUrl: '/admin/slideshow/add',
                controller: 'AddSlideShowController',
            }).when('/slideshow/edit/:slideId', {
                templateUrl: '/admin/slideshow/edit',
                controller: 'EditSlideShowController',
                resolve: {
                    slide: ['slideShowService', '$route', function (slideShowService, $route) {
                        return slideShowService.getSlideShow($route.current.params.slideId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/dorm', {
                templateUrl: '/admin/dorm/index',
                controller: 'DormsListController',
                resolve: {
                    dorms: ['dormService', function (dormService) {
                        return dormService.getAll().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/technicalstaffrole', {
                templateUrl: '/admin/technicalstaffrole/index',
                controller: 'TechnialStaffRolesListController',
                resolve: {
                    technicalStaffRoles: ['technicalStaffRoleService', function (technicalStaffRoleService) {
                        return technicalStaffRoleService.getAll().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/representativeuser', {
                templateUrl: '/admin/representativeuser/index',
                controller: 'RepresentativeUsersListController',
                resolve: {
                    representativeUsers: ['representativeUserService', function (representativeUserService) {
                        return representativeUserService.getAll().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/competitionrepresentativeuser/:competitionId', {
                templateUrl: '/admin/competitionrepresentativeuser/index',
                controller: 'CompetitionRepresentativeUsersListController',
                resolve: {
                    data: ['competitionRepresentativeUserService', '$route', function (competitionRepresentativeUserService, $route) {
                        return competitionRepresentativeUserService.getAll($route.current.params.competitionId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/register', {
                templateUrl: '/admin/register/index',
                controller: 'RegisterListController',
                resolve: {
                    competitionsList: ['competitionService', function (competitionService) {

                        return competitionService.getCompetitionListForReadiness().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
                reloadOnSearch: false
            }).when('/register/sportslist/:competitionId/:representativeUserId', {
                templateUrl: '/admin/register/sportslist',
                controller: 'RegisterSportsListController',
                resolve: {
                    data: ['registerService', '$route', function (registerService, $route) {

                        return registerService.getSportsList($route.current.params.competitionId, $route.current.params.representativeUserId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
                reloadOnSearch: false
            }).when('/register/competitors/:participationId', {
                templateUrl: '/admin/register/competitorslist',
                controller: 'RegisterCompetitorslistController',
                resolve: {
                    data: ['registerService', '$route', function (registerService, $route) {
                        return registerService.getCompetitorsList($route.current.params.participationId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
                reloadOnSearch: false
            }).when('/register/technicalstaffs/:participationId', {
                templateUrl: '/admin/register/technicalstaffslist',
                controller: 'RegisterTechnicalStaffslistController',
                resolve: {
                    data: ['registerService', '$route', function (registerService, $route) {
                        return registerService.getTehnicalStaffsList($route.current.params.participationId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
                reloadOnSearch: false
            }).when('/register/commontechnicalstaffs/:competitionId/:representativeUserId', {
                templateUrl: '/admin/register/commontechnicalstaffslist',
                controller: 'RegisterCommonTechnicalStaffslistController',
                resolve: {
                    data: ['registerService', '$route', function (registerService, $route) {
                        return registerService.getCommonTechnicalStaffsList($route.current.params.competitionId, $route.current.params.representativeUserId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
                reloadOnSearch: false
            }).when('/announcement/:competitionId', {
                templateUrl: '/admin/announcement/index',
                controller: 'AnnouncementListController',
                resolve: {
                    data: ['announcementService', '$route', function (announcementService, $route) {
                        return announcementService.getCompetitionAnnouncementsList($route.current.params.competitionId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
            }).when('/announcement/add/:competitionId', {
                templateUrl: '/admin/announcement/add',
                controller: 'AddAnnouncementController',
                resolve: {
                    data: ['announcementService', '$route', function (announcementService, $route) {
                        return announcementService.getAddData($route.current.params.competitionId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/announcement/edit/:announcementId', {
                templateUrl: '/admin/announcement/edit',
                controller: 'EditAnnouncementController',
                resolve: {
                    data: ['announcementService', '$route', function (announcementService, $route) {
                        return announcementService.getEditData($route.current.params.announcementId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
            }).when('/news', {
                templateUrl: '/admin/news/index',
                controller: 'NewsListController',
                resolve: {
                    data: ['newsService', '$route', function (newsService, $route) {
                        return newsService.getAll().then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                }
            }).when('/news/add', {
                templateUrl: '/admin/news/add',
                controller: 'AddNewsController',
            }).when('/news/edit/:newsId', {
                templateUrl: '/admin/news/edit',
                controller: 'EditNewsController',
                resolve: {
                    data: ['newsService', '$route', function (newsService, $route) {
                        return newsService.getEditData($route.current.params.newsId).then(function success(data) { return data; },
                        function error(reason) { return false; });
                    }]

                },
            });

    }]);

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

    app.filter('jalaliDate', function () {
        return function (inputDate, format) {
            var date = moment(inputDate);
            return date.format(format);
        }
    });

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

    app.filter('escape', function () {
        return window.encodeURIComponent;
    });


})();