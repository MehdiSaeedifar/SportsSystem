(function () {
    'use strict';

    var app = angular.module('app');

    app.controller('ReadinessController', ReadinessController);

    ReadinessController.$inject = ['$scope', 'competitionsList', 'competitionSportService', '$modal', '$location', '$routeParams'];

    function ReadinessController($scope, competitionsList, competitionSportService, $modal, $location, $routeParams) {

        $scope.viewModel = {};

        $scope.competitionsList = competitionsList;

        $scope.viewModel.selectedCompetition = null;

        $scope.competitionSportsList = [];

        $scope.displayedCompetitionSportsList = [].concat($scope.competitionSportsList);

        $scope.onCompetitionSelect = function ($item, $model) {

            if ($item == null) {
                return;
            }

            if ($routeParams.competitionId != $item.id) {
                $location.path('readiness/' + $item.id, false);
                $routeParams.competitionId = $item.id;
            }

            competitionSportService.getCompetitionSportListForReadiness($item.id).then(function success(data) {
                $scope.competitionSportsList = data;
            });

        };


        if ($routeParams.competitionId != null) {

            angular.forEach($scope.competitionsList, function (item, index) {

                if (item.id == $routeParams.competitionId) {

                    $scope.viewModel.selectedCompetition = $scope.competitionsList[index];

                    $scope.onCompetitionSelect({ id: item.id });
                }

            });
        }

    }


    app.controller('RedinessUserController', ['$scope', 'data', '$modal', '$routeParams', 'readinessService', function ($scope, data, $modal, $routeParams, readinessService) {

        $scope.competitionId = data.competitionId;
        $scope.competitionName = data.competitionName;
        $scope.sportName = data.sportName;
        $scope.gender = data.gender;

        $scope.usersList = data.representativeUsers;

        $scope.displayedUsersList = [].concat($scope.usersList);

        $scope.openUserInfo = function (user) {

            var modalInstance = $modal.open({
                templateUrl: 'showUserInfoModal.html',
                controller: 'ShowUserInfoModalController',
                resolve: {
                    competitionSportId: function () {
                        return user;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                $scope.selected = selectedItem;
            }, function () {
            });
        };


        $scope.approveParticipation = function (user) {

            readinessService.approve($routeParams.competitionSportId, user.id).then(function success(data) {
                user.isApproved = true;
            });

        };

        $scope.rejectParticipation = function (user) {

            readinessService.reject($routeParams.competitionSportId, user.id).then(function success(data) {

                user.isApproved = false;
            });

        };

    }]);


    app.controller('ShowUserInfoModalController', ['$scope', '$modalInstance', 'competitionSportId', function ($scope, $modalInstance, user) {

        $scope.user = user;

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('RedinessCompetitorsListController', ['$scope', 'competitorsList', '$modal', '$routeParams', 'readinessService', '$location', function ($scope, competitorsList, $modal, $routeParams, readinessService, $location) {

        $scope.participationId = $routeParams.participationId;

        $scope.competitorsList = competitorsList.competitors;

        $scope.displayedCompetitorsList = [].concat($scope.competitorsList);




    }]);





    app.controller('SportsListController', ['$scope', 'sports', '$modal', function ($scope, sports, $modal) {

        $scope.sportsList = sports;

        $scope.displayedSportsList = [].concat($scope.sportsList);

        $scope.removeSport = function (sport, index) {

            var modalInstance = $modal.open({
                templateUrl: 'deleteSport.html',
                controller: 'DeleteSportController',
                resolve: {
                    sport: function () {
                        return sport;
                    }
                }
            });

            modalInstance.result.then(function () {

                $scope.displayedSportsList.splice(index, 1);

            }, function () {

            });



        }

    }]);


    app.controller('AddSportController', ['$scope', 'readinessService', function ($scope, readinessService) {

        var initialize = function () {
            $scope.sport = {};
            $scope.sportCategories = [];
            $scope.sportDetails = [];

            $scope.sport = {}
            $scope.sportCategory = {};
            $scope.sportDetail = {};
        }

        initialize();


        $scope.displayedSportCategories = [].concat($scope.sportCategories);

        $scope.displayedSportDetails = [].concat($scope.sportDetails);


        $scope.addSportCategory = function () {
            $scope.sportCategories.push($scope.sportCategory);

            $scope.sportCategory = {};
        }


        $scope.removeSportCategory = function (idx) {

            $scope.sportCategories.splice(idx, 1);
        }

        $scope.addSportDetail = function () {

            $scope.sportDetails.push($scope.sportDetail);

            $scope.sportDetail = {};
        }

        $scope.removeSportDetail = function (idx) {

            $scope.sportDetails.splice(idx, 1);
        }


        $scope.addSport = function () {

            readinessService.addSport({
                sportName: $scope.sport.name,
                sportCategories: $scope.sportCategories,
                sportDetails: $scope.sportDetails
            }).then(function onSuccess(data) {

                initialize();

            });

        };


    }]);

    app.controller('DeleteSportController', ['$scope', 'sport', '$modalInstance', 'readinessService', function ($scope, sport, $modalInstance, readinessService) {

        $scope.sport = sport;

        $scope.ok = function () {
            readinessService.deleteSport(sport.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');

        };

    }]);


    app.controller('EditSportController', ['$scope', 'sport', 'readinessService', function ($scope, sport, readinessService) {

        var initialize = function () {
            $scope.sport = { id: sport.id, name: sport.name };
            $scope.sportCategories = sport.sportCategories;
            $scope.sportDetails = sport.sportDetails;

            //$scope.sport = {}
            $scope.sportCategory = {};
            $scope.sportDetail = {};
        }

        initialize();


        $scope.displayedSportCategories = [].concat($scope.sportCategories);

        $scope.displayedSportDetails = [].concat($scope.sportDetails);


        $scope.addSportCategory = function () {

            readinessService.addSportCategory({
                sportId: $scope.sport.id,
                sportCategoryName: $scope.sportCategory.name
            }).then(function onSuccess(data) {

                $scope.sportCategory.id = data;

                $scope.sportCategories.push($scope.sportCategory);

                $scope.sportCategory = {};

            });
        }


        $scope.removeSportCategory = function (idx, sportCategoryId) {

            readinessService.deleteSportCategory(sportCategoryId).then(function onSuccess(data) {

                $scope.displayedSportCategories.splice(idx, 1);

            });

        }

        $scope.addSportDetail = function () {

            readinessService.addSportDetail({
                sportId: $scope.sport.id,
                sportDetailName: $scope.sportDetail.name
            }).then(function onSuccess(data) {

                $scope.sportDetail.id = data;

                $scope.sportDetails.push($scope.sportDetail);

                $scope.sportDetail = {};

            });


        }

        $scope.removeSportDetail = function (idx, sportDetailId) {

            console.log(sportDetailId);
            readinessService.deleteSportDetail(sportDetailId).then(function onSuccess(data) {

                $scope.displayedSportDetails.splice(idx, 1);

            });


        }


        $scope.editSport = function () {

            readinessService.editSport($scope.sport).then(function onSuccess(data) {

                alert("ok");

            });

        };


    }]);


    app.controller('UniversitiesListController', [
        '$scope', 'universities', '$modal', 'readinessService', function ($scope, universities, $modal, readinessService) {


            $scope.universities = universities;

            $scope.displayedUniversities = [].concat($scope.universities);

            $scope.university = {}

            $scope.areas = ["واحد", "منطقه", "استان"];

            $scope.selectedArea = $scope.areas[0];

            $scope.addUniversity = function () {

                var university = angular.copy($scope.university);

                university.name = $scope.selectedArea + " " + university.name;

                readinessService.addUniversity(university).then(function onSuccess(data) {

                    university.id = data;
                    $scope.universities.push(university);

                    $scope.university = null;

                });

            };


            $scope.deleteUniversity = function (index, university) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteUniversity.html',
                    controller: 'DeleteUniversityController',
                    resolve: {
                        university: function () {
                            return university;
                        }
                    }
                });

                modalInstance.result.then(function () {



                    for (var i = 0; i < $scope.universities.length; i++) {
                        if ($scope.universities[i].id == university.id) {
                            $scope.universities.splice(i, 1);
                            return;
                        }
                    }

                    //$scope.displayedUniversities.splice(index, 1);


                }, function () {

                });

            }

            $scope.editUniversity = function (university) {

                var modalInstance = $modal.open({
                    templateUrl: 'editUniversity.html',
                    controller: 'EditUniversityController',
                    resolve: {
                        university: function () {
                            return university;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('DeleteUniversityController', ['$scope', 'university', '$modalInstance', 'readinessService', function ($scope, university, $modalInstance, readinessService) {

        $scope.university = university;


        $scope.ok = function () {
            readinessService.deleteUniversity(university.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('EditUniversityController', ['$scope', 'university', '$modalInstance', 'readinessService', function ($scope, university, $modalInstance, readinessService) {
        $scope.university = angular.copy(university);

        $scope.ok = function () {
            readinessService.editUniversity($scope.university).then(function onSuccess(data) {

                university.name = $scope.university.name;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('StudyFieldsListController', [
        '$scope', 'studyFields', '$modal', 'readinessService', function ($scope, studyFields, $modal, readinessService) {


            $scope.studyFields = studyFields;

            $scope.displayedStudyFields = [].concat($scope.studyFields);

            $scope.studyField = {}

            $scope.addStudyField = function () {

                readinessService.addStudyField($scope.studyField).then(function onSuccess(data) {

                    $scope.studyField.id = data;
                    $scope.studyFields.push($scope.studyField);

                    $scope.studyField = null;

                });

            };


            $scope.deleteStudyField = function (index, studyField) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteStudyFiled.html',
                    controller: 'DeleteStudyFiledController',
                    resolve: {
                        studyField: function () {
                            return studyField;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    $scope.displayedStudyFields.splice(index, 1);

                }, function () {

                });

            }

            $scope.editStudyField = function (studyField) {

                var modalInstance = $modal.open({
                    templateUrl: 'editStudyFiled.html',
                    controller: 'EditStudyFiledController',
                    resolve: {
                        studyField: function () {
                            return studyField;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('DeleteStudyFiledController', ['$scope', 'studyField', '$modalInstance', 'readinessService', function ($scope, studyField, $modalInstance, readinessService) {

        $scope.studyField = studyField;


        $scope.ok = function () {
            readinessService.deleteStudyField(studyField.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('EditStudyFiledController', ['$scope', 'studyField', '$modalInstance', 'readinessService', function ($scope, studyField, $modalInstance, readinessService) {
        $scope.studyField = angular.copy(studyField);

        $scope.ok = function () {
            readinessService.editStudyField($scope.studyField).then(function onSuccess(data) {

                studyField.name = $scope.studyField.name;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);

    app.controller('StudyFieldDegreesListController', [
        '$scope', 'studyFieldDegrees', '$modal', 'readinessService', function ($scope, studyFieldDegrees, $modal, readinessService) {


            $scope.studyFieldDegrees = studyFieldDegrees;

            $scope.displayedStudyFieldDegrees = [].concat($scope.studyFieldDegrees);

            $scope.studyFieldDegree = {}

            $scope.addStudyFieldDegree = function () {

                readinessService.addStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                    $scope.studyFieldDegree.id = data;
                    $scope.studyFieldDegrees.push($scope.studyFieldDegree);

                    $scope.studyFieldDegree = null;

                });

            };


            $scope.deleteStudyFieldDegree = function (index, studyFieldDegree) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteStudyFieldDegree.html',
                    controller: 'DeleteStudyFieldDegreeController',
                    resolve: {
                        studyFieldDegree: function () {
                            return studyFieldDegree;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    $scope.displayedStudyFieldDegrees.splice(index, 1);

                }, function () {

                });

            }

            $scope.editStudyFieldDegree = function (studyFieldDegree) {

                var modalInstance = $modal.open({
                    templateUrl: 'editStudyFieldDegree.html',
                    controller: 'EditStudyFieldDegreeController',
                    resolve: {
                        studyFieldDegree: function () {
                            return studyFieldDegree;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('EditStudyFieldDegreeController', ['$scope', 'studyFieldDegree', '$modalInstance', 'readinessService', function ($scope, studyFieldDegree, $modalInstance, readinessService) {
        $scope.studyFieldDegree = angular.copy(studyFieldDegree);

        $scope.ok = function () {
            readinessService.editStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                studyFieldDegree.name = $scope.studyFieldDegree.name;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);

    app.controller('DeleteStudyFieldDegreeController', ['$scope', 'studyFieldDegree', '$modalInstance', 'readinessService', function ($scope, studyFieldDegree, $modalInstance, readinessService) {

        $scope.studyFieldDegree = studyFieldDegree;


        $scope.ok = function () {
            readinessService.deleteStudyFieldDegree(studyFieldDegree.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('UsersListController', [
        '$scope', 'users', '$modal', 'readinessService', function ($scope, users, $modal, readinessService) {


            $scope.users = users;

            $scope.displayedUsers = [].concat($scope.users);

            $scope.studyFieldDegree = {}

            $scope.addStudyFieldDegree = function () {

                readinessService.addStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                    $scope.studyFieldDegree.id = data;
                    $scope.studyFieldDegrees.push($scope.studyFieldDegree);

                    $scope.studyFieldDegree = null;

                });

            };


            $scope.deleteUser = function (index, user) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteUser.html',
                    controller: 'DeleteUserController',
                    resolve: {
                        user: function () {
                            return user;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    $scope.displayedUsers.splice(index, 1);

                }, function () {

                });

            }

            $scope.editStudyFieldDegree = function (studyFieldDegree) {

                var modalInstance = $modal.open({
                    templateUrl: 'editStudyFieldDegree.html',
                    controller: 'EditStudyFieldDegreeController',
                    resolve: {
                        studyFieldDegree: function () {
                            return studyFieldDegree;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('AddUserController', ['$scope', 'readinessService', 'toaster', 'regExes', function ($scope, readinessService, toaster, regExes) {

        $scope.user = {
            role: 'admin'
        };

        $scope.errors = [];

        $scope.addUser = function () {
            readinessService.addUser($scope.user).then(function onSuccess(data) {

                toaster.pop('success', '', 'اطلاعات مدیر جدید با موفقیت در سیستم ثبت شد');

            }, function onError(errorData) {

                $scope.errors = errorData.data;

            });
        }

        $scope.passwordRegEx = regExes.password;

    }]);

    app.controller('EdituserController', ['$scope', 'readinessService', 'user', 'regExes', 'toaster', function ($scope, readinessService, user, regExes, toaster) {

        $scope.user = user;

        $scope.addUser = function () {

            readinessService.editUser($scope.user).then(function onSuccess(data) {

                toaster.pop('success', '', 'اطلاعات مدیر با موفقیت در سیستم ویرایش شد');

                $scope.errors = [];

            }, function onError(errorData) {

                $scope.errors = errorData.data;

            });
        }

        $scope.errors = [];
        $scope.passwordRegEx = regExes.password;

    }]);


    app.controller('DeleteUserController', ['$scope', 'user', '$modalInstance', 'readinessService', 'toaster', function ($scope, user, $modalInstance, readinessService, toaster) {

        $scope.user = user;

        $scope.ok = function () {
            readinessService.deleteUser(user.id).then(function onSuccess(data) {
                $modalInstance.close();
            }, function onError(errorData) {



            });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    //app.controller('EditUserController', ['$scope', 'studyFieldDegree', '$modalInstance', 'readinessService', function ($scope, studyFieldDegree, $modalInstance, readinessService) {
    //    $scope.studyFieldDegree = angular.copy(studyFieldDegree);

    //    $scope.ok = function () {
    //        readinessService.editStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

    //            studyFieldDegree.name = $scope.studyFieldDegree.name;

    //            $modalInstance.close();
    //        });


    //    };

    //    $scope.cancel = function () {
    //        $modalInstance.dismiss('cancel');
    //    };

    //}]);





    app.controller('SlideShowsListController', [
        '$scope', 'slides', '$modal', 'readinessService', function ($scope, slides, $modal, readinessService) {


            $scope.slides = slides;

            $scope.displayedSlides = [].concat($scope.slides);


            $scope.addStudyFieldDegree = function () {

                readinessService.addStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                    $scope.studyFieldDegree.id = data;
                    $scope.studyFieldDegrees.push($scope.studyFieldDegree);

                    $scope.studyFieldDegree = null;

                });

            };


            $scope.deleteSlideShow = function (index, slideShow) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteSlideShow.html',
                    controller: 'DeleteSlideShowController',
                    resolve: {
                        slideShow: function () {
                            return slideShow;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    $scope.displayedSlides.splice(index, 1);

                }, function () {

                });

            }

            $scope.editStudyFieldDegree = function (studyFieldDegree) {

                var modalInstance = $modal.open({
                    templateUrl: 'editStudyFieldDegree.html',
                    controller: 'EditStudyFieldDegreeController',
                    resolve: {
                        studyFieldDegree: function () {
                            return studyFieldDegree;
                        }
                    }
                });

                modalInstance.result.then(function () {

                }, function () {

                });

            }
        }

    ]);


    app.controller('AddSlideShowController', ['$scope', 'slideShowService', '$modal', 'toaster', function ($scope, slideShowService, $modal, toaster) {

        $scope.slide = {};

        $scope.showSlideShowImages = function (index, user) {

            var modalInstance = $modal.open({
                templateUrl: 'slideShowImages.html',
                controller: 'SlideShowImagesController'
            });

            modalInstance.result.then(function (fileName) {
                $scope.slide.image = fileName;

            }, function () {

            });

        }

        $scope.addSlideImage = function () {

            slideShowService.addSlideShow($scope.slide).then(function onSuccess(data) {

                toaster.pop('success', '', 'اسلاید جدید با موفقیت در سیستم ثبت شد');

            });

        }

    }]);


    app.controller('SlideShowImagesController', ['$scope', 'Upload', '$modalInstance', 'slideShowImageService', function ($scope, Upload, $modalInstance, slideShowImageService) {


        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.slideImages = [];

        $scope.displayedSlideImages = [].concat($scope.slideImages);

        $scope.showSlideImagesList = function () {
            slideShowImageService.getAllImages().then(function onSucceess(data) {
                $scope.slideImages = data;
            });
        }

        $scope.selectSlideImage = function (slideImage) {

            $modalInstance.close(slideImage);
        }

        $scope.deleteSlideImage = function (slideImage) {

            slideShowImageService.deleteSlideImage(slideImage).then(function onSuccess(data) {

                for (var i = 0; i < $scope.slideImages.length; i++) {
                    if ($scope.slideImages[i] == slideImage) {
                        $scope.slideImages.splice(i, 1);
                        return;
                    }
                }

            });

        }

        $scope.upload = function (files) {
            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/slideshow/upload',
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    }).success(function (data, status, headers, config) {
                        $modalInstance.close(data);
                    });
                }
            }
        };


    }]);

    app.controller('EditSlideShowController', ['$scope', 'slideShowService', 'slide', '$modal', 'toaster', function ($scope, slideShowService, slide, $modal, toaster) {

        $scope.slide = slide;

        $scope.showSlideShowImages = function (index, user) {

            var modalInstance = $modal.open({
                templateUrl: 'slideShowImages.html',
                controller: 'SlideShowImagesController'
            });

            modalInstance.result.then(function (fileName) {
                $scope.slide.image = fileName;

            }, function () {

            });

        }

        $scope.addSlideImage = function () {

            slideShowService.editSlideShow($scope.slide).then(function onSuccess(data) {

                toaster.pop('success', '', 'اسلاید جدید با موفقیت در سیستم ویرایش شد');

            });

        }


    }]);


    app.controller('DeleteSlideShowController', ['$scope', 'slideShow', '$modalInstance', 'slideShowService', function ($scope, slideShow, $modalInstance, slideShowService) {


        $scope.ok = function () {
            slideShowService.deleteSlideShow(slideShow.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('EditUserController', ['$scope', 'studyFieldDegree', '$modalInstance', 'readinessService', function ($scope, studyFieldDegree, $modalInstance, readinessService) {
        $scope.studyFieldDegree = angular.copy(studyFieldDegree);

        $scope.ok = function () {
            readinessService.editStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                studyFieldDegree.name = $scope.studyFieldDegree.name;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);




    app.controller('StudyFieldDegreesListController', [
        '$scope', 'studyFieldDegrees', '$modal', 'readinessService', function ($scope, studyFieldDegrees, $modal, readinessService) {


            $scope.studyFieldDegrees = studyFieldDegrees;

            $scope.displayedStudyFieldDegrees = [].concat($scope.studyFieldDegrees);

            $scope.studyFieldDegree = {}

            $scope.addStudyFieldDegree = function () {

                readinessService.addStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                    $scope.studyFieldDegree.id = data;
                    $scope.studyFieldDegrees.push($scope.studyFieldDegree);

                    $scope.studyFieldDegree = null;

                });

            };


            $scope.deleteStudyFieldDegree = function (index, studyFieldDegree) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteStudyFieldDegree.html',
                    controller: 'DeleteStudyFieldDegreeController',
                    resolve: {
                        studyFieldDegree: function () {
                            return studyFieldDegree;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    $scope.displayedStudyFieldDegrees.splice(index, 1);

                }, function () {

                });

            }

            $scope.editStudyFieldDegree = function (studyFieldDegree) {

                var modalInstance = $modal.open({
                    templateUrl: 'editStudyFieldDegree.html',
                    controller: 'EditStudyFieldDegreeController',
                    resolve: {
                        studyFieldDegree: function () {
                            return studyFieldDegree;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('EditStudyFieldDegreeController', ['$scope', 'studyFieldDegree', '$modalInstance', 'readinessService', function ($scope, studyFieldDegree, $modalInstance, readinessService) {
        $scope.studyFieldDegree = angular.copy(studyFieldDegree);

        $scope.ok = function () {
            readinessService.editStudyFieldDegree($scope.studyFieldDegree).then(function onSuccess(data) {

                studyFieldDegree.name = $scope.studyFieldDegree.name;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);

    app.controller('DeleteStudyFieldDegreeController', ['$scope', 'studyFieldDegree', '$modalInstance', 'readinessService', function ($scope, studyFieldDegree, $modalInstance, readinessService) {

        $scope.studyFieldDegree = studyFieldDegree;


        $scope.ok = function () {
            readinessService.deleteStudyFieldDegree(studyFieldDegree.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('DormsListController', [
        '$scope', 'dorms', '$modal', 'dormService', function ($scope, dorms, $modal, dormService) {


            $scope.dorms = dorms;

            $scope.displayedDorms = [].concat($scope.dorms);

            $scope.dorm = {}

            $scope.addDorm = function () {

                dormService.addDorm($scope.dorm).then(function onSuccess(data) {

                    $scope.dorm.id = data;
                    $scope.dorms.push($scope.dorm);

                    $scope.dorm = null;

                });

            };


            $scope.deleteDorm = function (index, dorm) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteDorm.html',
                    controller: 'DeleteDormController',
                    resolve: {
                        dorm: function () {
                            return dorm;
                        }
                    }
                });

                modalInstance.result.then(function () {



                    for (var i = 0; i < $scope.dorms.length; i++) {
                        if ($scope.dorms[i].id == dorm.id) {
                            $scope.dorms.splice(i, 1);
                            return;
                        }
                    }

                    //$scope.displayedUniversities.splice(index, 1);


                }, function () {

                });

            }

            $scope.editDorm = function (dorm) {

                var modalInstance = $modal.open({
                    templateUrl: 'editDorm.html',
                    controller: 'EditDormController',
                    resolve: {
                        dorm: function () {
                            return dorm;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('DeleteDormController', ['$scope', 'dorm', '$modalInstance', 'dormService', function ($scope, dorm, $modalInstance, dormService) {

        $scope.dorm = dorm;


        $scope.ok = function () {
            dormService.deleteDorm(dorm.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('EditDormController', ['$scope', 'dorm', '$modalInstance', 'dormService', function ($scope, dorm, $modalInstance, dormService) {
        $scope.dorm = angular.copy(dorm);

        $scope.ok = function () {
            dormService.editDorm($scope.dorm).then(function onSuccess(data) {

                dorm.name = $scope.dorm.name;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);







    app.controller('TechnialStaffRolesListController', [
        '$scope', 'technicalStaffRoles', '$modal', 'technicalStaffRoleService', function ($scope, technicalStaffRoles, $modal, technicalStaffRoleService) {


            $scope.technicalStaffRoles = technicalStaffRoles;

            $scope.displayedTechnicalStaffRoles = [].concat($scope.technicalStaffRoles);

            $scope.technicalStaffRole = {}

            $scope.addTechnicalStaffRole = function () {

                technicalStaffRoleService.addTechnicalStaffRole($scope.technicalStaffRole).then(function onSuccess(data) {

                    $scope.technicalStaffRole.id = data;
                    $scope.technicalStaffRoles.push($scope.technicalStaffRole);

                    $scope.technicalStaffRole = null;

                });

            };


            $scope.deleteTechnicalStaffRole = function (index, technicalStaffRole) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteTechnicalStaffRole.html',
                    controller: 'DeleteTechnicalStaffController',
                    resolve: {
                        technicalStaffRole: function () {
                            return technicalStaffRole;
                        }
                    }
                });

                modalInstance.result.then(function () {



                    for (var i = 0; i < $scope.technicalStaffRoles.length; i++) {
                        if ($scope.technicalStaffRoles[i].id == technicalStaffRole.id) {
                            $scope.technicalStaffRoles.splice(i, 1);
                            return;
                        }
                    }

                    //$scope.displayedUniversities.splice(index, 1);


                }, function () {

                });

            }

            $scope.editTechnicalStaffRole = function (technicalStaffRole) {

                var modalInstance = $modal.open({
                    templateUrl: 'editTechnicalStaffRole.html',
                    controller: 'EditTechnicalStaffRoleController',
                    resolve: {
                        technicalStaffRole: function () {
                            return technicalStaffRole;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    //$scope.displayedUniversities.splice(index, 1);

                }, function () {

                });

            }






        }
    ]);


    app.controller('DeleteTechnicalStaffController', ['$scope', 'technicalStaffRole', '$modalInstance', 'technicalStaffRoleService', function ($scope, technicalStaffRole, $modalInstance, technicalStaffRoleService) {

        $scope.technicalStaffRole = technicalStaffRole;


        $scope.ok = function () {
            technicalStaffRoleService.deleteTechnicalStaffRole(technicalStaffRole.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('EditTechnicalStaffRoleController', ['$scope', 'technicalStaffRole', '$modalInstance', 'technicalStaffRoleService', function ($scope, technicalStaffRole, $modalInstance, technicalStaffRoleService) {
        $scope.technicalStaffRole = angular.copy(technicalStaffRole);

        $scope.ok = function () {
            technicalStaffRoleService.editTechnicalStaffRole($scope.technicalStaffRole).then(function onSuccess(data) {

                technicalStaffRole.name = $scope.technicalStaffRole.name;
                technicalStaffRole.isCommon = $scope.technicalStaffRole.isCommon;

                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);



    app.controller('RepresentativeUsersListController', [
        '$scope', 'representativeUsers', '$modal', function ($scope, representativeUsers, $modal) {

            $scope.representativeUsers = representativeUsers;

            $scope.displayedRepresentativeUsers = [].concat($scope.representativeUsers);

            $scope.RepresentativeUser = {}

            $scope.addRepresentativeUser = function () {

                var modalInstance = $modal.open({
                    templateUrl: 'addRepresentativeUser.html',
                    controller: 'AddRepresentativeUserController',
                    resolve: {
                        universities: ['readinessService', function (readinessService) {
                            return readinessService.getUniversitiesList().then(function success(data) { return data; },
                            function error(reason) { return false; });
                        }],
                        representativeUsers: function () {
                            return $scope.representativeUsers;
                        }
                    }
                });

                modalInstance.result.then(function (newUser) {

                    $scope.representativeUsers.push(newUser);

                }, function () {

                });

            }

            $scope.deleteRepresentativeUser = function (index, representativeUser) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteRepresentativeUser.html',
                    controller: 'DeleteRepresentativeUserController',
                    resolve: {
                        representativeUser: function () {
                            return representativeUser;
                        }

                    }
                });

                modalInstance.result.then(function () {

                    for (var i = 0; i < $scope.representativeUsers.length; i++) {
                        if ($scope.representativeUsers[i].id == representativeUser.id) {
                            $scope.representativeUsers.splice(i, 1);
                            return;
                        }
                    }

                }, function () {

                });

            }

            $scope.editRepresentativeUser = function (representativeUser) {

                var modalInstance = $modal.open({
                    templateUrl: 'editRepresentativeUser.html',
                    controller: 'EditRepresentativeUserController',
                    resolve: {
                        representativeUser: function () {
                            return representativeUser;
                        },
                        universities: ['readinessService', function (readinessService) {
                            return readinessService.getUniversitiesList().then(function success(data) { return data; },
                            function error(reason) { return false; });
                        }]
                    }
                });

                modalInstance.result.then(function () {

                }, function () {

                });
            }

        }

    ]);


    app.controller('AddRepresentativeUserController', ['$scope', '$modalInstance', 'representativeUserService', 'regExes', 'toaster', 'universities', 'representativeUsers', function ($scope, $modalInstance, representativeUserService, regExes, toaster, universities, representativeUsers) {

        $scope.universityList = universities;

        $scope.passwordRegEx = regExes.password;
        $scope.numberRegEx = regExes.number;
        $scope.persianRegEx = regExes.persianChars;
        $scope.mobileNumberRegEx = regExes.mobileNumber;

        $scope.errors = [];

        $scope.user = {};

        $scope.registerUser = function (frm) {

            representativeUserService.addRepresentativeUser($scope.user).then(function success(data) {

                toaster.pop("success", "", 'اطلاعات شما با موفقیت در سیستم ثبت شد');

                $scope.user.id = data;

                angular.forEach($scope.universityList, function (item) {

                    if (item.id === $scope.user.universityId) {

                        $scope.user.universityName = item.name;
                        return;
                    }

                });

                representativeUsers.push($scope.user);
                frm.$setPristine();
                $scope.errors = [];
                $scope.user = {};

                //$modalInstance.close($scope.user);

            }, function error(errorData) {

                $scope.errors = errorData.data;

                angular.forEach($scope.errors, function (item) {
                    if (item == "پست الکترونیکی وارد شده قبلا در سیستم ثبت شده است.") {
                        $scope.user.email = null;
                        return;
                    }
                });

            });
        }

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('DeleteRepresentativeUserController', ['$scope', 'representativeUser', '$modalInstance', 'representativeUserService', function ($scope, representativeUser, $modalInstance, representativeUserService) {

        $scope.representativeUser = representativeUser;

        $scope.ok = function () {

            representativeUserService.deleteRepresentativeUser(representativeUser.id).then(function onSuccess(data) {
                $modalInstance.close();
            });

        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('EditRepresentativeUserController', ['$scope', 'representativeUser', '$modalInstance', 'representativeUserService', 'regExes', 'toaster', 'universities', function ($scope, representativeUser, $modalInstance, representativeUserService, regExes, toaster, universities) {

        $scope.universityList = universities;

        $scope.passwordRegEx = regExes.password;
        $scope.numberRegEx = regExes.number;
        $scope.persianRegEx = regExes.persianChars;
        $scope.mobileNumberRegEx = regExes.mobileNumber;

        $scope.errors = [];

        $scope.user = angular.copy(representativeUser);

        angular.forEach(universities, function (item) {
            if (item.name == $scope.user.universityName) {
                $scope.user.universityId = item.id;
            }
        });

        $scope.registerUser = function () {

            representativeUserService.editRepresentativeUser($scope.user).then(function success(data) {

                toaster.pop("success", "", 'اطلاعات نماینده با موفقیت در سیستم ویرایش شد');

                angular.forEach($scope.universityList, function (item) {

                    if (item.id === $scope.user.universityId) {

                        representativeUser.universityName = item.name;
                        return;
                    }

                });

                representativeUser.email = $scope.user.email;
                representativeUser.firstName = $scope.user.firstName;
                representativeUser.lastName = $scope.user.lastName;
                representativeUser.mobileNumber = $scope.user.mobileNumber;
                representativeUser.nationalCode = $scope.user.nationalCode;

                $modalInstance.close($scope.user);

            }, function error(errorData) {

                $scope.errors = errorData.data;

                angular.forEach($scope.errors, function (item) {
                    if (item == "پست الکترونیکی وارد شده قبلا در سیستم ثبت شده است.") {
                        $scope.user.email = null;
                        return;
                    }
                });

            });
        }

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('CompetitionsListController', [
        '$scope', 'data', '$modal', 'readinessService', function ($scope, data, $modal, readinessService) {


            $scope.competitions = data;

            $scope.displayedCompetitions = [].concat($scope.competitions);


            $scope.deleteCompetition = function (competition) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteCompetition.html',
                    controller: 'DeleteCompetitionController',
                    resolve: {
                        competition: function () {
                            return competition;
                        }

                    }
                });

                modalInstance.result.then(function () {

                    for (var i = 0; i < $scope.competitions.length; i++) {
                        if ($scope.competitions[i].id == competition.id) {
                            $scope.competitions.splice(i, 1);
                            return;
                        }
                    }

                }, function () {

                });

            }


        }
    ]);

    app.controller('DeleteCompetitionController', ['$scope', 'competition', '$modalInstance', 'competitionService', function ($scope, competition, $modalInstance, competitionService) {

        $scope.competition = competition;

        $scope.ok = function () {
            competitionService.deleteCompetition(competition.id).then(function onSuccess(data) {
                $modalInstance.close();
            });


        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('AddCompetitionController', ['$scope', 'toaster', 'competitionService', 'Upload', function ($scope, toaster, competitionService, Upload) {

        $scope.competitionModel = {
            isReadyActive: true,
            isRegisterActive: true,
            isPrintCardActive: true
        };


        $scope.readyStartDate = {};
        $scope.readyEndDate = {};

        $scope.registerStartDate = {};
        $scope.registerEndDate = {};

        $scope.printCardStartDate = {};
        $scope.printCardEndDate = {};

        $scope.addCompetition = function () {

            if ($scope.competitionModel.isReadyActive) {

                var tmpDate = moment($scope.readyStartDate.year + '/' +
                    $scope.readyStartDate.month + '/' +
                    $scope.readyStartDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ شروع اعلام آمادگی اشتباه است");
                    return;
                }

                $scope.competitionModel.readyStartDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.readyEndDate.year + '/' +
                   $scope.readyEndDate.month + '/' +
                   $scope.readyEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ پایان اعلام آمادگی اشتباه است");
                    return;
                }

                $scope.competitionModel.readyEndDate = tmpDate.format('YYYY/M/D');
            }

            if ($scope.competitionModel.isRegisterActive) {

                var tmpDate = moment($scope.registerStartDate.year + '/' +
                    $scope.registerStartDate.month + '/' +
                    $scope.registerStartDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ شروع ثبت نام اشتباه است");
                    return;
                }

                $scope.competitionModel.registerStartDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.registerEndDate.year + '/' +
                   $scope.registerEndDate.month + '/' +
                   $scope.registerEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ پایان ثبت نام اشتباه است");
                    return;
                }

                $scope.competitionModel.registerEndDate = tmpDate.format('YYYY/M/D');
            }


            if ($scope.competitionModel.isPrintCardActive) {

                var tmpDate = moment($scope.printCardStartDate.year + '/' +
                    $scope.printCardStartDate.month + '/' +
                    $scope.printCardStartDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ شروع چاپ کارت اشتباه است");
                    return;
                }

                $scope.competitionModel.printCardStartDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.printCardEndDate.year + '/' +
                   $scope.printCardEndDate.month + '/' +
                   $scope.printCardEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ پایان چاپ کارت اشتباه است");
                    return;
                }
                $scope.competitionModel.printCardEndDate = tmpDate.format('YYYY/M/D');
            }


            console.log($scope.competitionModel);

            var sendToast = toaster.pop('info', "", "در حال ثبت اطلاعات در سرور، لطفا منتظر بمانید ...");
            competitionService.addCompetition(angular.copy($scope.competitionModel)).then(function (data) {

                toaster.clear(sendToast);

                $scope.competitionModel.id = data.id;

                toaster.pop('success', "", "مسابقه جدید با موفقیت ایجاد شد.");

            }, function () {
                toaster.clear(sendToast);
                toaster.pop('error', "", "متاسفانه خطایی رخ داده است، لطفا مجددا سعی نمایید.");
            });

        };

        $scope.uploadLogoImage = function (files) {

            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/home/uploadlogoimage',
                        //fields: { 'username': $scope.username },
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                        //console.log('progress: ' + progressPercentage + '% ' + evt.config.file.name);
                    }).success(function (data, status, headers, config) {
                        //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                        $scope.logoImageUrl = data.url;
                        $scope.competitionModel.logoImage = data.name;
                    });
                }
            }
        };

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);


    app.controller('EditCompetitionController', ['$scope', 'toaster', 'competitionService', 'Upload', 'data', function ($scope, toaster, competitionService, Upload, data) {

        $scope.competitionModel = data;


        $scope.readyStartDate = {};
        $scope.readyEndDate = {};

        $scope.registerStartDate = {};
        $scope.registerEndDate = {};

        $scope.printCardStartDate = {};
        $scope.printCardEndDate = {};

        var tmpDate;

        if ($scope.competitionModel.readyStartDate != null) {

            tmpDate = moment($scope.competitionModel.readyStartDate);

            $scope.readyStartDate = {
                day: tmpDate.jDate(),
                month: tmpDate.jMonth() + 1,
                year: tmpDate.jYear()

            };
        }

        if ($scope.competitionModel.readyEndDate != null) {
            tmpDate = moment($scope.competitionModel.readyEndDate);

            $scope.readyEndDate = {
                day: tmpDate.jDate(),
                month: tmpDate.jMonth() + 1,
                year: tmpDate.jYear()

            };
        }

        if ($scope.competitionModel.registerStartDate != null) {
            tmpDate = moment($scope.competitionModel.registerStartDate);

            $scope.registerStartDate = {
                day: tmpDate.jDate(),
                month: tmpDate.jMonth() + 1,
                year: tmpDate.jYear()

            };
        }

        if ($scope.competitionModel.registerEndDate != null) {
            tmpDate = moment($scope.competitionModel.registerEndDate);

            $scope.registerEndDate = {
                day: tmpDate.jDate(),
                month: tmpDate.jMonth() + 1,
                year: tmpDate.jYear()

            };
        }

        if ($scope.competitionModel.printCardStartDate != null) {
            tmpDate = moment($scope.competitionModel.printCardStartDate);

            $scope.printCardStartDate = {
                day: tmpDate.jDate(),
                month: tmpDate.jMonth() + 1,
                year: tmpDate.jYear()

            };
        }

        if ($scope.competitionModel.printCardEndDate != null) {
            tmpDate = moment($scope.competitionModel.printCardEndDate);

            $scope.printCardEndDate = {
                day: tmpDate.jDate(),
                month: tmpDate.jMonth() + 1,
                year: tmpDate.jYear()

            };
        }

        $scope.logoImageUrl = '/file/home/getlogoimage?fileName=' + $scope.competitionModel.logoImage;


        $scope.addCompetition = function () {

            if ($scope.competitionModel.isReadyActive) {

                var tmpDate = moment($scope.readyStartDate.year + '/' +
                    $scope.readyStartDate.month + '/' +
                    $scope.readyStartDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ شروع اعلام آمادگی اشتباه است");
                    return;
                }

                $scope.competitionModel.readyStartDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.readyEndDate.year + '/' +
                   $scope.readyEndDate.month + '/' +
                   $scope.readyEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ پایان اعلام آمادگی اشتباه است");
                    return;
                }
                $scope.competitionModel.readyEndDate = tmpDate.format('YYYY/M/D');
            }

            if ($scope.competitionModel.isRegisterActive) {

                var tmpDate = moment($scope.registerStartDate.year + '/' +
                    $scope.registerStartDate.month + '/' +
                    $scope.registerStartDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ شروع ثبت نام اشتباه است");
                    return;
                }

                $scope.competitionModel.registerStartDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.registerEndDate.year + '/' +
                   $scope.registerEndDate.month + '/' +
                   $scope.registerEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "تاریخ اشتباه", "تاریخ پایان ثبت نام اشتباه است");
                    return;
                }
                $scope.competitionModel.registerEndDate = tmpDate.format('YYYY/M/D');
            }


            if ($scope.competitionModel.isPrintCardActive) {

                var tmpDate = moment($scope.printCardStartDate.year + '/' +
                    $scope.printCardStartDate.month + '/' +
                    $scope.printCardStartDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ شروع چاپ کارت اشتباه است");
                    return;
                }

                $scope.competitionModel.printCardStartDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.printCardEndDate.year + '/' +
                   $scope.printCardEndDate.month + '/' +
                   $scope.printCardEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ پایان چاپ کارت اشتباه است");
                    return;
                }
                $scope.competitionModel.printCardEndDate = tmpDate.format('YYYY/M/D');
            }


            console.log($scope.competitionModel);

            var sendToast = toaster.pop('info', "", "در حال ثبت اطلاعات در سرور، لطفا منتظر بمانید ...");
            competitionService.editCompetition(angular.copy($scope.competitionModel)).then(function (data) {

                toaster.clear(sendToast);

                //$scope.competitionModel.id = data.id;

                toaster.pop('success', "", "مسابقه جدید با موفقیت ایجاد شد.");

            }, function () {
                toaster.clear(sendToast);
                toaster.pop('error', "", "متاسفانه خطایی رخ داده است، لطفا مجددا سعی نمایید.");
            });

        };

        $scope.uploadLogoImage = function (files) {

            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/home/uploadlogoimage',
                        //fields: { 'username': $scope.username },
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                        console.log('progress: ' + progressPercentage + '% ' + evt.config.file.name);
                    }).success(function (data, status, headers, config) {
                        //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                        $scope.logoImageUrl = data.url;
                        $scope.competitionModel.logoImage = data.name;
                    });
                }
            }
        };

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);



    app.controller('CompetitionRepresentativeUsersListController', ['$scope', 'competitionRepresentativeUserService', 'regExes', 'toaster', 'data', '$routeParams', function ($scope, competitionRepresentativeUserService, regExes, toaster, data, $routeParams) {


        $scope.competitionName = data.competitionName;
        $scope.competitionId = $routeParams.competitionId;

        $scope.representativeUsers = data.representativeUsers;

        $scope.displayedRepresentativeUsers = [].concat($scope.representativeUsers);

        $scope.selectedRepresentativeUsers = [];

        angular.forEach($scope.representativeUsers, function (item) {

            if (item.isSelected === true) {
                $scope.selectedRepresentativeUsers.push(item.id);
            }

        });

        $scope.toggleSelection = function toggleSelection(csId) {

            var idx = $scope.selectedRepresentativeUsers.indexOf(csId);

            // is currently selected
            if (idx > -1) {
                $scope.selectedRepresentativeUsers.splice(idx, 1);
            }

                // is newly selected
            else {
                $scope.selectedRepresentativeUsers.push(csId);
            }
        };

        $scope.sumbitData = function () {

            competitionRepresentativeUserService.add($routeParams.competitionId, $scope.selectedRepresentativeUsers)
                .then(function onSuccess(data) {
                    toaster.pop("success", "", "اطلاعات با موفقیت در سیستم ثبت شد");
                });
        }

    }]);



    app.controller('RegisterListController', ['$scope', 'competitionsList', '$modal', '$routeParams', 'registerService', '$location', function ($scope, competitionsList, $modal, $routeParams, registerService, $location) {

        $scope.competitionId = $routeParams.competitionId;

        $scope.viewModel = {};

        $scope.competitionsList = competitionsList;

        $scope.viewModel.selectedCompetition = null;

        $scope.onCompetitionSelect = function ($item, $model) {

            if ($item == null) {
                return;
            }

            var competitionId = $item.id;

            if (competitionId != ($location.search()).competitionId) {

                $location.search({ competitionId: competitionId });
                $scope.competitionId = competitionId;
            }

            registerService.getRepresentativeUsers(competitionId).then(function onSuccess(data) {
                console.log(data);
                $scope.representativeUsersList = data;
            });


        };



        if (($location.search()).competitionId != null) {

            var competitionId = ($location.search()).competitionId;

            angular.forEach($scope.competitionsList, function (item, index) {

                if (item.id == competitionId) {

                    $scope.viewModel.selectedCompetition = $scope.competitionsList[index];

                    $scope.onCompetitionSelect({ id: item.id });
                }

            });

        }




        $scope.representativeUsersList = [];

        $scope.displayedRepresentativeUsersList = [].concat($scope.representativeUsersList);



    }]);


    app.controller('RegisterSportsListController', ['$scope', 'data', '$modal', '$routeParams', 'registerService', '$location', function ($scope, data, $modal, $routeParams, registerService, $location) {

        $scope.competitionId = $routeParams.competitionId;
        $scope.representativeUserId = $routeParams.representativeUserId;

        $scope.university = data.university;

        $scope.competitionName = data.competitionName;

        $scope.unverifiedCommonTechnicalStaffsCount = data.unverifiedCommonTechnicalStaffsCount;

        $scope.sportsList = data.sportsList;

        $scope.displayedSportsList = [].concat($scope.sportsList);



    }]);

    app.controller('RegisterCompetitorslistController', ['$scope', 'data', '$modal', '$routeParams', 'registerService', '$location', 'readinessService', function ($scope, data, $modal, $routeParams, registerService, $location, readinessService) {

        $scope.participationId = $routeParams.participationId;

        $scope.competition = {};
        $scope.competition.name = data.competitionName;
        $scope.competition.id = data.competitionId;
        $scope.sport = {};
        $scope.sport.name = data.sport;
        $scope.sport.gender = data.gender;
        $scope.university = data.university;
        $scope.representativeUserId = data.representativeUserId;
        $scope.isIndividual = data.isIndividual;

        $scope.competitorsList = data.competitors;

        $scope.displayedCompetitorsList = [].concat($scope.competitorsList);


        var modalInstance = {};
        modalInstance.isOpen = false;


        $scope.openCompetitorModal = function (competitor) {

            $location.search('modal', competitor.id);

            modalInstance = $modal.open({
                templateUrl: 'showCompetitiorModal.html',
                controller: 'ShowCompetitorModalController',
                size: 'lg',
                resolve: {
                    data: function () {
                        return registerService.getCompetitorEditData(competitor.id).then(function success(data) { return data });
                    },
                    selectedCompetitor: function () { return competitor }
                }
            });

            modalInstance.isOpen = true;

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });

        };



        if (($location.search()).modal != null) {

            var competitorId = ($location.search()).modal;

            angular.forEach($scope.competitorsList, function (item) {
                if (item.id == competitorId) {

                    $scope.openCompetitorModal(item);
                    return;
                }
            });

        }

        if (($location.search()).modal != null && modalInstance.isOpen == false) {
            var competitorId = ($location.search()).modal;

            angular.forEach($scope.competitorsList, function (item) {
                if (item.id == competitorId) {

                    $scope.openCompetitorModal(item);
                    return;
                }
            });
        }

        $scope.openTeamColorModal = function () {

            modalInstance = $modal.open({
                templateUrl: 'teamColorModal.html',
                controller: 'TeamColorController',
                resolve: {
                    colors: function () {
                        return registerService.getTeamColors($scope.participationId).then(function success(data) { return data });
                    }
                }
            });
        };


    }]);

    app.controller('ShowCompetitorModalController', ['$scope', '$modalInstance', 'data', '$location', 'selectedCompetitor', 'readinessService', function ($scope, $modalInstance, data, $location, selectedCompetitor, readinessService) {


        $scope.isApprovedOptions = [{ id: null, value: 'بررسی نشده' }, { id: true, value: 'تایید شده' }, { id: false, value: 'تایید نشده' }];

        $scope.$on('$locationChangeSuccess', function () {
            if (($location.search()).modal == null) {

                $scope.cancel();
            }
        });

        $scope.competitor = data.competitor;

        $scope.dormsList = data.dormsList;

        $scope.viewModel = {};

        $scope.viewModel.competitor = {
            competitorId: data.competitor.id,
            isApproved: data.competitor.isApproved,
            error: data.competitor.error,
            dormId: data.competitor.dormId,
            dormNumber: data.competitor.dormNumber,
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');

        };

        $modalInstance.result.then(function () {
            // not called... at least for me
        }, function () {
            $location.search({});
        }).then(function () { $modalInstance.isOpen = false });


        $scope.saveChanges = function () {
            readinessService.editApproval($scope.viewModel.competitor).then(function success(data) {
                selectedCompetitor.isApproved = $scope.viewModel.competitor.isApproved;
                $modalInstance.close();
            });
        };


        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);



    app.controller('RegisterTechnicalStaffslistController', ['$scope', 'data', '$modal', '$routeParams', 'registerService', '$location', 'readinessService', function ($scope, data, $modal, $routeParams, registerService, $location, readinessService) {

        $scope.participationId = $routeParams.participationId;
        $scope.competition = {};
        $scope.competition.name = data.competitionName;
        $scope.sport = {};
        $scope.sport.name = data.competitionSport.sportName;
        $scope.sport.gender = data.competitionSport.gender;
        $scope.university = data.university;

        $scope.representativeUserId = data.representativeUserId;
        $scope.competition.id = data.competitionId;

        $scope.competitorsList = data.technicalStaffs;

        $scope.displayedCompetitorsList = [].concat($scope.competitorsList);


        var modalInstance = {};

        modalInstance.isOpen = false;

        $scope.openCompetitorModal = function (competitor) {
            $location.search('modal', competitor.id);

            modalInstance = $modal.open({
                templateUrl: 'technicalStaffApproval.html',
                controller: 'TechnicalStaffApprovalController',
                resolve: {
                    data: function () {
                        return registerService.getTehnicalStaffEditData(competitor.id, $routeParams.participationId).then(function success(data) { return data });
                    },
                    selectedCompetitor: function () { return competitor }
                }
            });

            modalInstance.isOpen = true;

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });

        };



        if (($location.search()).modal != null) {

            var competitorId = ($location.search()).modal;

            angular.forEach($scope.competitorsList, function (item) {
                if (item.id == competitorId) {

                    $scope.openCompetitorModal(item);
                    return;
                }
            });

        }

        if (($location.search()).modal != null && modalInstance.isOpen == false) {
            var competitorId = ($location.search()).modal;

            angular.forEach($scope.competitorsList, function (item) {
                if (item.id == competitorId) {

                    $scope.openCompetitorModal(item);
                    return;
                }
            });
        }

    }]);

    app.controller('TechnicalStaffApprovalController', ['$scope', '$modalInstance', 'data', '$location', 'selectedCompetitor', 'registerService', function ($scope, $modalInstance, data, $location, selectedCompetitor, registerService) {
        console.log(data);

        $scope.isApprovedOptions = [{ id: null, value: 'بررسی نشده' }, { id: true, value: 'تایید شده' }, { id: false, value: 'تایید نشده' }];


        $scope.$on('$locationChangeSuccess', function () {
            if (($location.search()).modal == null) {

                $scope.cancel();
            }
        });

        $scope.competitor = data.technicalStaff;

        $scope.dormsList = data.dormsList;


        $scope.viewModel = {};

        $scope.viewModel.competitor = {
            id: data.technicalStaff.id,
            isApproved: data.technicalStaff.isApproved,
            error: data.technicalStaff.error,
            dormId: data.technicalStaff.dormId,
            dormNumber: data.technicalStaff.dormNumber,
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');

        };

        $modalInstance.result.then(function () {
            // not called... at least for me
        }, function () {
            $location.search({});
        }).then(function () { $modalInstance.isOpen = false });


        $scope.saveChanges = function () {
            registerService.editTechnicalStaffApproval($scope.viewModel.competitor).then(function success(data) {
                selectedCompetitor.isApproved = $scope.viewModel.competitor.isApproved;
                $modalInstance.close();
            });
        };


        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);



    app.controller('RegisterCommonTechnicalStaffslistController', ['$scope', 'data', '$modal', '$routeParams', 'registerService', '$location', 'readinessService', function ($scope, data, $modal, $routeParams, registerService, $location, readinessService) {

        $scope.competitionId = $routeParams.competitionId;
        $scope.representativeUserId = $routeParams.representativeUserId;

        $scope.competition = {};
        $scope.competition.name = data.competition.competitionName;
        $scope.university = data.competition.university;

        $scope.competitorsList = data.technicalStaffs;

        $scope.displayedCompetitorsList = [].concat($scope.competitorsList);


        var modalInstance = {};
        modalInstance.isOpen = false;

        $scope.openCompetitorModal = function (competitor) {
            $location.search('modal', competitor.id);

            modalInstance = $modal.open({
                templateUrl: 'technicalStaffApproval.html',
                controller: 'TechnicalStaffApprovalController',
                resolve: {
                    data: function () {
                        return registerService.getCommonTehnicalStaffEditData(competitor.id).then(function success(data) { return data });
                    },
                    selectedCompetitor: function () { return competitor }
                }
            });

            modalInstance.isOpen = true;

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });

        };

        if (($location.search()).modal != null) {

            var competitorId = ($location.search()).modal;

            angular.forEach($scope.competitorsList, function (item) {
                if (item.id == competitorId) {

                    $scope.openCompetitorModal(item);
                    return;
                }
            });

        }

        if (($location.search()).modal != null && modalInstance.isOpen == false) {
            var competitorId = ($location.search()).modal;

            angular.forEach($scope.competitorsList, function (item) {
                if (item.id == competitorId) {

                    $scope.openCompetitorModal(item);
                    return;
                }
            });
        }



    }]);

    app.controller('CommonTechnicalStaffApprovalController', ['$scope', '$modalInstance', 'data', '$location', 'selectedCompetitor', 'registerService', function ($scope, $modalInstance, data, $location, selectedCompetitor, registerService) {
        console.log(data);

        $scope.isApprovedOptions = [{ id: null, value: 'بررسی نشده' }, { id: true, value: 'تایید شده' }, { id: false, value: 'تایید نشده' }];


        $scope.$on('$locationChangeSuccess', function () {
            if (($location.search()).modal == null) {

                $scope.cancel();
            }
        });

        $scope.competitor = data.technicalStaff;

        $scope.dormsList = data.dormsList;


        $scope.viewModel = {};

        $scope.viewModel.competitor = {
            id: data.technicalStaff.id,
            isApproved: data.technicalStaff.isApproved,
            error: data.technicalStaff.error,
            dormId: data.technicalStaff.dormId,
            dormNumber: data.technicalStaff.dormNumber,
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');

        };

        $modalInstance.result.then(function () {
            // not called... at least for me
        }, function () {
            $location.search({});
        }).then(function () { $modalInstance.isOpen = false });


        $scope.saveChanges = function () {
            registerService.editTechnicalStaffApproval($scope.viewModel.competitor).then(function success(data) {
                selectedCompetitor.isApproved = $scope.viewModel.competitor.isApproved;
                $modalInstance.close();
            });
        };


        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);


    app.controller('AnnouncementListController', ['$scope', 'data', '$modal', '$routeParams', function ($scope, data, $modal, $routeParams) {

        $scope.competitionId = $routeParams.competitionId;

        $scope.competitionName = data.competitionName;

        $scope.announcementsList = data.announcements;

        $scope.displayedAnnouncementsList = [].concat($scope.announcementsList);

        $scope.deleteAnnouncement = function (announcement, index) {

            var modalInstance = $modal.open({
                templateUrl: 'deleteAnnouncement.html',
                controller: 'DeleteAnnouncementController',
                resolve: {
                    announcement: function () {
                        return announcement;
                    }
                }
            });

            modalInstance.result.then(function () {

                $scope.displayedAnnouncementsList.splice(index, 1);

            }, function () {

            });
        }

    }]);


    app.controller('AddAnnouncementController', ['$scope', 'announcementService', '$routeParams', 'data', 'toaster', function ($scope, announcementService, $routeParams, data, toaster) {

        $scope.announcement = {
            competitionId: $routeParams.competitionId
        };

        $scope.competitionName = data;
        $scope.competitionId = $routeParams.competitionId;

        $scope.submitAnnouncement = function () {

            announcementService.addAnnouncement($scope.announcement).then(function onSuccess(data) {
                toaster.pop("success", "", "اطلاعیه جدید با موفقیت در سیستم ثبت شد");
            });

        }

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };

    }]);

    app.controller('DeleteAnnouncementController', ['$scope', 'announcement', '$modalInstance', 'announcementService', function ($scope, announcement, $modalInstance, announcementService) {

        $scope.announcement = announcement;

        $scope.ok = function () {
            announcementService.deleteAnnouncement(announcement.id).then(function onSuccess(data) {
                $modalInstance.close();
            });

        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('EditAnnouncementController', ['$scope', 'data', 'announcementService', 'toaster', function ($scope, data, announcementService, toaster) {

        $scope.announcement = data;

        $scope.submitAnnouncement = function () {

            announcementService.editAnnouncement($scope.announcement).then(function onSuccess(data) {
                toaster.pop("success", "", "اطلاعیه با موفقیت ویرایش شد");
            });

        }

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);



    app.controller('NewsListController', ['$scope', 'data', '$modal', '$routeParams', function ($scope, data, $modal, $routeParams) {

        $scope.newsList = data;

        $scope.displayedNewsList = [].concat($scope.newsList);

        $scope.deleteNews = function (news, index) {

            var modalInstance = $modal.open({
                templateUrl: 'deleteNews.html',
                controller: 'DeleteNewsController',
                resolve: {
                    news: function () {
                        return news;
                    }
                }
            });

            modalInstance.result.then(function () {

                $scope.displayedNewsList.splice(index, 1);

            }, function () {

            });



        }

    }]);


    app.controller('AddNewsController', ['$scope', 'newsService', '$routeParams', 'toaster', function ($scope, newsService, $routeParams, toaster) {

        $scope.news = {};

        $scope.submitNews = function () {

            newsService.addNews($scope.news).then(function onSuccess(data) {
                toaster.pop("success", "", "خبر جدید با موفقیت در سیستم ثبت شد");
            });

        }

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };

    }]);

    app.controller('DeleteNewsController', ['$scope', 'news', '$modalInstance', 'newsService', function ($scope, news, $modalInstance, newsService) {

        $scope.news = news;

        $scope.ok = function () {
            newsService.deleteNews(news.id).then(function onSuccess(data) {
                $modalInstance.close();
            });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('EditNewsController', ['$scope', 'data', 'newsService', 'toaster', function ($scope, data, newsService, toaster) {

        $scope.news = data;

        $scope.submitNews = function () {

            newsService.editNews($scope.news).then(function onSuccess(data) {
                toaster.pop("success", "", "خبر با موفقیت ویرایش شد");
            });

        }

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }]);


    app.controller('LayoutController', [
        '$scope', '$route', 'layoutService', 'toaster', '$window', '$timeout', function ($scope, $route, layoutService, toaster, $window, $timeout) {

            $scope.currentUser = { fullName: '' };

            $scope.$route = $route;

            layoutService.getLayoutData().then(function onSuccess(data) {

                $scope.currentUser = data.user;

            });

            $scope.signOut = function () {

                layoutService.signOut().then(function onSuccess(data) {
                    toaster.pop("success", "", "شما با موفقیت از سیستم خارج شدید");

                    $timeout(function () {
                        $window.location = "/account/#/loginadmin";
                    }, 1500);

                });
            }

        }
    ]);


    app.controller('TeamColorController', ['$scope', 'colors', '$modalInstance', function ($scope, colors, $modalInstance) {

        $scope.colors = colors;

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('AddCompetitionSportController', AddCompetitionSportController);

    AddCompetitionSportController.$inject = ['$scope', '$routeParams', 'data', 'toaster', 'competitionSportService', '$modal'];

    function AddCompetitionSportController($scope, $routeParams, data, toaster, competitionSportService, $modal) {

        $scope.viewModel = {};

        var initialModel = function () {

            $scope.competitionSportModel = {
                gender: 'Male',
                competitionId: $routeParams.competitionId
            };

            $scope.viewModel.selectedSport = {};

        };

        initialModel();

        $scope.competitionSportsList = data.competition.competitionSports;

        $scope.displayedCollection = [].concat($scope.competitionSportsList);


        $scope.sportsList = data.sportsList;

        $scope.competition = data.competition;

        $scope.onSportSelect = function ($item, $model) {

            initialModel();

            $scope.viewModel.selectedSport = $item;


            if ($scope.viewModel.selectedSport.sportCategories.length > 0) {

                $scope.viewModel.selectedSport.sportCategory = $scope.viewModel.selectedSport.sportCategories[0];
            } else {

                $scope.viewModel.selectedSport.sportCategory = null;
            }

            if ($scope.viewModel.selectedSport.sportDetails.length > 0) {

                $scope.viewModel.selectedSport.sportDetail = $scope.viewModel.selectedSport.sportDetails[0];
            } else {

                $scope.viewModel.selectedSport.sportDetail = null;
            }

        }


        $scope.addCompetitionSport = function (form) {

            $scope.competitionSportModel.sport = $scope.viewModel.selectedSport;

            if ($scope.viewModel.selectedSport.sportCategory != null)
                $scope.competitionSportModel.sportCategory = $scope.viewModel.selectedSport.sportCategory;

            if ($scope.viewModel.selectedSport.sportDetail != null)
                $scope.competitionSportModel.sportDetail = $scope.viewModel.selectedSport.sportDetail;


            var isDuplicate = false;

            angular.forEach($scope.competitionSportsList, function (item) {

                var sportGender = item.sport.id === $scope.competitionSportModel.sport.id && item.gender === $scope.competitionSportModel.gender;

                if (sportGender
                    && $scope.competitionSportModel.sportCategory == null && $scope.competitionSportModel.sportDetail == null) {

                    toaster.pop('error', '', 'رشته ورزشی وارد شده تکراری است.');
                    isDuplicate = true;
                    return;
                }

                if (sportGender && $scope.competitionSportModel.sportCategory != null && $scope.competitionSportModel.sportDetail == null)
                    if (item.sportCategory != null && item.sportCategory.id === $scope.competitionSportModel.sportCategory.id) {
                        toaster.pop('error', '', 'رشته ورزشی وارد شده تکراری است.');
                        isDuplicate = true;
                        return;
                    }

                if (sportGender && $scope.competitionSportModel.sportCategory == null && $scope.competitionSportModel.sportDetail != null)
                    if (item.sportDetail != null && item.sportDetail.id === $scope.competitionSportModel.sportDetail.id) {
                        toaster.pop('error', '', 'رشته ورزشی وارد شده تکراری است.');
                        isDuplicate = true;
                        return;
                    }

                if (sportGender && $scope.competitionSportModel.sportCategory != null && $scope.competitionSportModel.sportDetail != null)
                    if (item.sportDetail != null && item.sportCategory != null && item.sportDetail.id === $scope.competitionSportModel.sportDetail.id &&
                         item.sportCategory.id === $scope.competitionSportModel.sportCategory.id) {
                        toaster.pop('error', '', 'رشته ورزشی وارد شده تکراری است.');
                        isDuplicate = true;
                        return;
                    }
            });

            if (!isDuplicate) {

                var competitionSport = {
                    gender: $scope.competitionSportModel.gender,
                    competitionId: $scope.competitionSportModel.competitionId,
                    sportId: $scope.competitionSportModel.sport.id,
                    maxCompetitors: $scope.competitionSportModel.maxCompetitors,
                    maxTechnicalStaffs: $scope.competitionSportModel.maxTechnicalStaffs,
                    hasRule: $scope.competitionSportModel.hasRule,
                    rule: $scope.competitionSportModel.rule,
                    isIndividual: $scope.competitionSportModel.isIndividual
                };

                if ($scope.competitionSportModel.sportCategory != null)
                    competitionSport.sportCategoryId = $scope.competitionSportModel.sportCategory.id;

                if ($scope.competitionSportModel.sportDetail != null)
                    competitionSport.sportDetailId = $scope.competitionSportModel.sportDetail.id;

                var toasterSendData = toaster.pop('info', '', 'در حال ارسال اطلاعات به سرور، لطفا منتظر بمانید ...');

                competitionSportService.add(competitionSport).then(function success(data) {

                    $scope.competitionSportModel.id = data;

                    $scope.competitionSportModel.sportName = $scope.competitionSportModel.sport.name;

                    if ($scope.competitionSportModel.sportCategory != null) {
                        $scope.competitionSportModel.sportName += " " + $scope.competitionSportModel.sportCategory.name;
                    }
                    if ($scope.competitionSportModel.sportDetail != null) {
                        $scope.competitionSportModel.sportName += " " + $scope.competitionSportModel.sportDetail.name;
                    }

                    $scope.competitionSportsList.push($scope.competitionSportModel);

                    toaster.clear(toasterSendData);
                    toaster.pop('success', '', 'رشته جدید با موفقیت ثبت شد.');
                    initialModel();
                    form.$setPristine();

                }, function error() {

                    toaster.clear(toasterSendData);
                    toaster.pop('error', '', 'خطایی در ارسال اطلاعات رخ داده است. لطفا دوباره سعی نمایید.');

                });


            }

        }

        $scope.deleteCompetitionSport = function (index, competitionSport) {

            var modalInstance = $modal.open({
                templateUrl: 'deleteCompetitionSport.html',
                controller: 'DeleteCompetitionSportController',
                resolve: {
                    competitionSport: function () {
                        return competitionSport;
                    }

                }
            });

            modalInstance.result.then(function () {
                for (var i = 0; i < $scope.competitionSportsList.length; i++) {
                    if ($scope.competitionSportsList[i].id == competitionSport.id) {
                        $scope.competitionSportsList.splice(i, 1);
                        return;
                    }
                }

            }, function () {

            });

        }


        $scope.editCompetitionSport = function (competitionSport) {

            var modalInstance = $modal.open({
                templateUrl: 'editCompetitionSport.html',
                controller: 'EditCompetitionSportController',
                resolve: {
                    competitionSport: function () {
                        return competitionSportService.getEditData(competitionSport.id).then(function success(data) {
                            competitionSport.isIndividual = data.isIndividual;
                            competitionSport.hasRule = data.hasRule;
                            competitionSport.rule = data.rule;
                            return competitionSport;
                        });
                    }

                }
            });

            modalInstance.result.then(function () {


            }, function () {

            });

        }

        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]


        };


    }


    app.controller('DeleteCompetitionSportController', ['$scope', 'competitionSport', '$modalInstance', 'competitionSportService', function ($scope, competitionSport, $modalInstance, competitionSportService) {

        $scope.competitionSport = competitionSport;

        $scope.ok = function () {
            competitionSportService.deleteCompetitionSport(competitionSport.id).then(function onSuccess(data) {
                $modalInstance.close();
            });

        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);


    app.controller('EditCompetitionSportController', ['$scope', 'competitionSport', '$modalInstance', 'competitionSportService', function ($scope, competitionSport, $modalInstance, competitionSportService) {

        $scope.competitionSportModel = angular.copy(competitionSport);

        $scope.ok = function () {
            competitionSportService.editCompetitionSport($scope.competitionSportModel).then(function onSuccess(data) {

                competitionSport.isIndividual = $scope.competitionSportModel.isIndividual;
                competitionSport.maxTechnicalStaffs = $scope.competitionSportModel.maxTechnicalStaffs;
                competitionSport.maxCompetitors = $scope.competitionSportModel.maxCompetitors;
                competitionSport.hasRule = $scope.competitionSportModel.hasRule;

                $modalInstance.close();
            });

        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };


        $scope.editorOptions = {
            language: 'fa',
            //uiColor: '#000000',
            extraPlugins: 'divarea',
            skin: 'bootstrapck',
            ontentsLangDirection: 'rtl',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                '/', // Line break - next group will be placed in new line.
                { name: 'basicstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'Bold', 'Italic', 'BidiRtl', 'BidiLtr'] },
            ],

            stylesSet: [
                { name: 'Language: RTL', element: 'span', attributes: { 'direction': 'rtl', display: 'block' } },
                { name: 'Language: LTR', element: 'span', styles: { 'direction': 'ltr', display: 'block' } }
            ]

        };

    }]);



})();


