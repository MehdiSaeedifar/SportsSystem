(function () {
    'use strict';

    var app = angular
        .module('userPanelApp');

    app.controller('UserPanelController', UserPanelController);

    UserPanelController.$inject = ['$scope'];

    function UserPanelController($scope) {


    }


    app.controller('UserPanelReadinessListController', [
        '$scope', 'competitionsList', function ($scope, competitionsList) {

            $scope.competitionsList = competitionsList;
        }
    ]);


    app.controller('UserReadyCompetitionsListController', [
        '$scope', 'competitionsList', function ($scope, competitionsList) {

            $scope.competitionsList = competitionsList;
        }
    ]);


    app.controller('ApprovedCompetitionParticipationsListController', [
        '$scope', 'competition', '$routeParams', function ($scope, competition, $routeParams) {


            $scope.competition = competition;

            $scope.competitionSportsList = competition.participations;

            $scope.displayedCompetitionSportsList = [].concat($scope.competitionSportsList);

            $scope.competitionId = $routeParams.competitionId;

        }
    ]);


    app.controller('AddTechnicalStaffController', [
        '$scope', '$routeParams', 'toaster', 'data', 'technicalStaffService', 'Upload', 'regExes', '$document', function ($scope, $routeParams, toaster, data, technicalStaffService, Upload, regExes, $document) {

            $scope.errors = [];

            $scope.persianRegex = regExes.persianChars;
            $scope.mobileNumberRegex = regExes.mobileNumber;
            $scope.numberRegEx = regExes.number;

            $scope.competitionSport = data.competitionSport;

            var frmCheckNationalCode;

            var initialize = function () {

                $scope.technicalStaff = {
                    participationId: $routeParams.participationId
                };

                $scope.birthDate = {};
                $scope.imageFiles = {};

                $scope.showForm = false;

                $scope.uploadProgressBars = {};
                $scope.uploadProgressBars.userImage = {
                    value: 0
                };
            }

            initialize();

            $scope.technicalStaffRoles = data.technicalStaffRoles;


            $scope.checkNationalCode = function (formCheckNationalCode) {

                frmCheckNationalCode = formCheckNationalCode;

                technicalStaffService.getByNationalCode($scope.technicalStaff.nationalCode, $scope.technicalStaff.participationId).then(function onSuccess(data) {
                    if (!data)
                        $scope.showForm = true;

                    else {

                        $scope.showForm = false;
                        $scope.technicalStaff = data;

                        var birthDate = moment($scope.technicalStaff.birthDate);

                        $scope.birthDate = {
                            day: birthDate.jDate(),
                            month: birthDate.jMonth() + 1,
                            year: birthDate.jYear()
                        };

                        $scope.technicalStaff.technicalStaffRoleId = data.roleId;
                        $scope.imageFiles.userImageUrl = '/file/home/gettechnicalstaffimage?fileName=' + $scope.technicalStaff.image;
                    }

                    $scope.technicalStaff.participationId = $routeParams.participationId;
                });
            }


            $scope.addTechnicalStaff = function (frmAddTechnicalStaff) {

                if (!$scope.technicalStaff.id) {
                    var tmpDate = moment($scope.birthDate.year + '/' +
                        $scope.birthDate.month + '/' +
                        $scope.birthDate.day, 'jYYYY/jM/jD');
                    if (!tmpDate.isValid()) {

                        toaster.pop('error', "", "تاریخ تولد اشتباه است");
                        frmAddTechnicalStaff.birthDateDay.$setValidity("number", false);
                        frmAddTechnicalStaff.birthDateMonth.$setValidity("number", false);
                        frmAddTechnicalStaff.birthDateYear.$setValidity("number", false);
                        return;
                    }

                    $scope.technicalStaff.birthDate = tmpDate.format('YYYY/M/D');
                }

                technicalStaffService.addTechnicalStaff($scope.technicalStaff).then(function onSuccess(data) {

                    toaster.pop('success', '', 'کادر فنی جدید با موفقیت در سیستم ثبت شد.');

                    $scope.competitionSport.technicalStaffsCount = $scope.competitionSport.technicalStaffsCount + 1;

                    frmAddTechnicalStaff.$setPristine();
                    frmCheckNationalCode.$setPristine();
                    initialize();

                    var someElement = angular.element(document.getElementById('topSection'));
                    $document.scrollToElement(someElement, 0, 500);

                    $scope.errors = [];

                }, function onError(errorData) {

                    $scope.errors = errorData.data;

                });


            }

            $scope.clearNationalCode = function () {
                initialize();
            }

            $scope.uploadUserImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/uploaduserimage',
                            file: file
                        }).progress(function (evt) {

                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.uploadProgressBars.userImage.value = progressPercentage;

                        }).success(function (data, status, headers, config) {
                            $scope.imageFiles.userImageUrl = data.url;
                            $scope.technicalStaff.image = data.name;
                        });
                    }
                }
            };

        }
    ]);

    app.controller('EditTechnicalStaffController', [
        '$scope', '$routeParams', 'toaster', 'data', 'technicalStaffService', 'Upload', 'regExes', '$sce', function ($scope, $routeParams, toaster, data, technicalStaffService, Upload, regExes, $sce) {

            $scope.errors = [];

            $scope.participationId = $routeParams.participationId;

            $scope.technicalStaff = data.technicalStaff;

            $scope.technicalStaff.error = $sce.trustAsHtml($scope.technicalStaff.error);

            $scope.persianRegex = regExes.persianChars;
            $scope.mobileNumberRegex = regExes.mobileNumber;
            $scope.numberRegEx = regExes.number;


            var birthDate = moment($scope.technicalStaff.birthDate);

            $scope.birthDate = {
                day: birthDate.jDate(),
                month: birthDate.jMonth() + 1,
                year: birthDate.jYear()

            };

            $scope.technicalStaff.technicalStaffRoleId = $scope.technicalStaff.roleId;

            $scope.technicalStaff.participationId = $routeParams.participationId;

            $scope.technicalStaffRoles = data.technicalStaffRoles;

            $scope.imageFiles = {};

            $scope.uploadProgressBars = {};
            $scope.uploadProgressBars.userImage = {
                value: 0
            };

            $scope.imageFiles.userImageUrl = '/file/home/gettechnicalstaffimage?fileName=' + $scope.technicalStaff.image;

            $scope.submitButton = { text: "ثبت اطلاعات", disable: false };


            $scope.addTechnicalStaff = function (frmAddTechnicalStaff) {

                var tmpDate = moment($scope.birthDate.year + '/' +
                    $scope.birthDate.month + '/' +
                    $scope.birthDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ تولد اشتباه است");
                    //frmAddTechnicalStaff.birthDateDay.$setValidity("number", false);
                    //frmAddTechnicalStaff.birthDateMonth.$setValidity("number", false);
                    //frmAddTechnicalStaff.birthDateYear.$setValidity("number", false);
                    return;
                }

                $scope.technicalStaff.birthDate = tmpDate.format('YYYY/M/D');

                $scope.submitButton.text = "لطفا منتظر بمانید...";
                $scope.submitButton.disable = true;

                technicalStaffService.editTechnicalStaff($scope.technicalStaff).then(function onSuccess(data) {

                    toaster.pop('success', '', 'اطلاعات کادر فنی با موفقیت در سیستم ویرایش شد.');

                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                    $scope.errors = [];

                }, function onError(errorData) {
                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                    $scope.errors = errorData.data;

                });


            }


            $scope.uploadUserImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/uploaduserimage',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.uploadProgressBars.userImage.value = progressPercentage;
                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.userImageUrl = data.url;
                            $scope.technicalStaff.image = data.name;
                        });
                    }
                }
            };

        }
    ]);

    app.controller('TechnicalStaffsListController', [
        '$scope', '$routeParams', 'toaster', 'data', 'competitorService', '$modal', function ($scope, $routeParams, toaster, data, competitorService, $modal) {

            $scope.participationId = $routeParams.participationId;

            $scope.technicalStaffsList = data.technicalStaffs;

            $scope.displayedTechnicalStaffsList = [].concat($scope.technicalStaffsList);

            $scope.competitionSport = data.competitionSport;

            $scope.deleteTechnicalStaff = function (technicalStaff) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteTechnicalStaff.html',
                    controller: 'DeleteTechnicalStaffController',
                    resolve: {
                        technicalStaff: function () {
                            technicalStaff.participationId = $scope.participationId;
                            return technicalStaff;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    for (var i = 0; i < $scope.technicalStaffsList.length; i++) {
                        if ($scope.technicalStaffsList[i].id == technicalStaff.id) {
                            $scope.technicalStaffsList.splice(i, 1);
                            return;
                        }
                    }

                }, function () {

                });

            }


        }
    ]);

    app.controller('DeleteTechnicalStaffController', [
        '$scope', 'technicalStaff', '$modalInstance', 'technicalStaffService', 'toaster', function ($scope, technicalStaff, $modalInstance, technicalStaffService, toaster) {

            $scope.technicalStaff = technicalStaff;


            $scope.ok = function () {
                technicalStaffService.deleteTechnicalStaff(technicalStaff.id, technicalStaff.participationId).then(function onSuccess(data) {
                    $modalInstance.close();
                }, function onError(errorsData) {

                    toaster.pop("error", "", errorsData.data[0]);

                });


            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };

        }
    ]);


    app.controller('CompetitorsListController', [
        '$scope', '$routeParams', 'toaster', 'data', 'competitorService', '$modal', function ($scope, $routeParams, toaster, data, competitorService, $modal) {

            $scope.participationId = $routeParams.participationId;

            $scope.competitorsList = data.competitiors;

            $scope.displayedCompetitorsList = [].concat($scope.competitorsList);

            $scope.competitionSport = data.competitionSport;

            $scope.deleteCompetitor = function (competitor) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteCompetitor.html',
                    controller: 'DeleteCompetitorController',
                    resolve: {
                        competitor: function () {
                            return competitor;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    for (var i = 0; i < $scope.competitorsList.length; i++) {
                        if ($scope.competitorsList[i].id == competitor.id) {
                            $scope.competitorsList.splice(i, 1);
                            return;
                        }
                    }

                }, function () {

                });

            }


        }
    ]);


    app.controller('DeleteCompetitorController', [
        '$scope', 'competitor', '$modalInstance', 'competitorService', 'toaster', function ($scope, competitor, $modalInstance, competitorService, toaster) {

            $scope.competitor = competitor;

            $scope.ok = function () {
                competitorService.deleteCompetitor(competitor.id).then(function onSuccess(data) {
                    $modalInstance.close();
                }, function onError(errorData) {

                    toaster.pop("error", "", errorData.data[0]);

                });

            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };

        }

    ]);


    app.controller('EditCompetitorController', [
        '$scope', '$routeParams', 'toaster', 'data', 'competitorService', 'regExes', 'Upload', '$sce', function ($scope, $routeParams, toaster, data, competitorService, regExes, Upload, $sce) {

            $scope.errors = [];

            $scope.persianRegex = regExes.persianChars;
            $scope.mobileNumberRegex = regExes.mobileNumber;
            $scope.numberRegEx = regExes.number;

            $scope.viewModel = {};

            $scope.viewModel.studyFields = data.studyFields;
            $scope.viewModel.studyFieldDegrees = data.studyFieldDegrees;

            $scope.competitionSport = data.competitionSport;


            var initial = function () {
                $scope.competitor = data.competitor;

                $scope.competitor.error = $sce.trustAsHtml($scope.competitor.error);

                $scope.imageFiles = {};
                $scope.imageFiles.userImageUrl = "/file/home/getuserimage?fileName=" + $scope.competitor.userImage;
                $scope.imageFiles.studentCertificateImageUrl = "/file/home/getstudentcertificateimage?fileName=" + $scope.competitor.studentCertificateImage;
                $scope.imageFiles.insuranceImageUrl = "/file/home/getinsuranceimage?fileName=" + $scope.competitor.insuranceImage;
                $scope.imageFiles.azmoonConfirmationImageUrl = "/file/home/getazmoonconfirmationimage?fileName=" + $scope.competitor.azmoonConfirmationImage;

                var birthDate = moment($scope.competitor.birthDate);

                $scope.birthDate = {
                    day: birthDate.jDate(),
                    month: birthDate.jMonth() + 1,
                    year: birthDate.jYear()

                };

                var insuranceEndDate = moment($scope.competitor.insuranceEndDate);

                $scope.insuranceEndDate = {
                    day: insuranceEndDate.jDate(),
                    month: insuranceEndDate.jMonth() + 1,
                    year: insuranceEndDate.jYear()

                };

            }


            initial();

            $scope.uploadProgressBars = {};

            $scope.uploadProgressBars.userImage = {
                value: 0
            };


            $scope.uploadProgressBars.studentCertificateImage =
            {
                value: 0
            };

            $scope.uploadProgressBars.insuranceImage =
            {
                value: 0
            };

            $scope.uploadProgressBars.azmoonConfirmationImage =
            {
                value: 0
            };

            $scope.uploadUserImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/uploaduserimage',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                            $scope.uploadProgressBars.userImage.value = progressPercentage;

                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.userImageUrl = data.url;
                            $scope.competitor.userImage = data.name;
                        });
                    }
                }
            };


            $scope.uploadStudentCertificateImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/imageUpload',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                            $scope.uploadProgressBars.studentCertificateImage.value = progressPercentage;

                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.studentCertificateImageUrl = data.url;
                            $scope.competitor.studentCertificateImage = data.name;
                        });
                    }
                }
            };


            $scope.uploadInsuranceImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/imageUpload',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                            $scope.uploadProgressBars.insuranceImage.value = progressPercentage;

                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.insuranceImageUrl = data.url;
                            $scope.competitor.insuranceImage = data.name;
                        });
                    }
                }
            };

            $scope.uploadAzmoonConfirmationImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/imageUpload',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                            $scope.uploadProgressBars.azmoonConfirmationImage.value = progressPercentage;

                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.azmoonConfirmationImageUrl = data.url;
                            $scope.competitor.azmoonConfirmationImage = data.name;
                        });
                    }
                }
            };

            $scope.submitButton = { text: "ثبت اطلاعات", disable: false };

            $scope.editCompetitor = function (frmAddCompetitor) {


                var tmpDate = moment($scope.birthDate.year + '/' +
                    $scope.birthDate.month + '/' +
                    $scope.birthDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ تولد اشتباه است");
                    //frmAddCompetitor.birthDateDay.$setValidity("number", false);
                    //frmAddCompetitor.birthDateMonth.$setValidity("number", false);
                    //frmAddCompetitor.birthDateYear.$setValidity("number", false);
                    return;
                }

                $scope.competitor.birthDate = tmpDate.format('YYYY/M/D');

                tmpDate = moment($scope.insuranceEndDate.year + '/' +
                    $scope.insuranceEndDate.month + '/' +
                    $scope.insuranceEndDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ پایان بیمه اشتباه است");
                    return;
                }

                $scope.competitor.insuranceEndDate = tmpDate.format('YYYY/M/D');

                $scope.submitButton.text = "لطفا منتظر بمانید...";
                $scope.submitButton.disable = true;

                competitorService.editCompetitor($scope.competitor).then(function success(data) {

                    toaster.pop('success', '', 'مشخصات بازیکن با موفقیت در سیستم ویرایش شد.');

                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                    $scope.errors = [];

                }, function error(errorData) {
                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                    $scope.errors = errorData.data;

                });

            }

        }

    ]);


    app.controller('AddCommonTechnicalStaffController', [
        '$scope', '$routeParams', 'toaster', 'data', 'commonTechnicalStaffService', 'Upload', 'regExes', '$document', function ($scope, $routeParams, toaster, data, commonTechnicalStaffService, Upload, regExes, $document) {

            $scope.errors = [];

            $scope.persianRegex = regExes.persianChars;
            $scope.mobileNumberRegex = regExes.mobileNumber;
            $scope.numberRegEx = regExes.number;

            var initialize = function () {
                $scope.technicalStaff = {

                };

                $scope.birthDate = {};

                $scope.imageFiles = {};

                $scope.uploadProgressBars = {};
                $scope.uploadProgressBars.userImage = {
                    value: 0
                };

            }

            initialize();

            $scope.technicalStaffRoles = data.technicalStaffRoles;

            $scope.competition = data.competition;

            $scope.competitionId = $routeParams.competitionId;

            $scope.addTechnicalStaff = function (frmAddTechnicalStaff) {

                if (!$scope.technicalStaff.id) {
                    var tmpDate = moment($scope.birthDate.year + '/' +
                        $scope.birthDate.month + '/' +
                        $scope.birthDate.day, 'jYYYY/jM/jD');
                    if (!tmpDate.isValid()) {

                        toaster.pop('error', "", "تاریخ تولد اشتباه است");
                        frmAddTechnicalStaff.birthDateDay.$setValidity("number", false);
                        frmAddTechnicalStaff.birthDateMonth.$setValidity("number", false);
                        frmAddTechnicalStaff.birthDateYear.$setValidity("number", false);
                        return;
                    }

                    $scope.technicalStaff.birthDate = tmpDate.format('YYYY/M/D');
                }

                commonTechnicalStaffService.addTechnicalStaff($scope.technicalStaff, $routeParams.competitionId).then(function onSuccess(data) {
                    toaster.pop('success', '', 'خدمه جدید با موفقیت در سیستم ثبت شد.');

                    $scope.competition.technicalStaffsCount = $scope.competition.technicalStaffsCount + 1;

                    frmAddTechnicalStaff.$setPristine();

                    initialize();

                    var someElement = angular.element(document.getElementById('topSection'));
                    $document.scrollToElement(someElement, 0, 500);

                    $scope.errors = [];

                }, function onError(errorsData) {

                    $scope.errors = errorsData.data;

                });


            }


            $scope.uploadUserImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/uploaduserimage',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.uploadProgressBars.userImage.value = progressPercentage;
                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.userImageUrl = data.url;
                            $scope.technicalStaff.image = data.name;
                        });
                    }
                }
            };


        }
    ]);

    app.controller('EditCommonTechnicalStaffController', [
        '$scope', '$routeParams', 'toaster', 'data', 'commonTechnicalStaffService', 'Upload', 'regExes', '$sce', function ($scope, $routeParams, toaster, data, commonTechnicalStaffService, Upload, regExes, $sce) {

            $scope.errors = [];

            $scope.competitionId = $routeParams.competitionId;

            $scope.technicalStaff = data.technicalStaff;

            $scope.technicalStaff.competitionId = $scope.competitionId;

            $scope.technicalStaff.error = $sce.trustAsHtml($scope.technicalStaff.error);

            $scope.persianRegex = regExes.persianChars;
            $scope.mobileNumberRegex = regExes.mobileNumber;
            $scope.numberRegEx = regExes.number;

            var birthDate = moment($scope.technicalStaff.birthDate);

            $scope.birthDate = {
                day: birthDate.jDate(),
                month: birthDate.jMonth() + 1,
                year: birthDate.jYear()
            };

            $scope.technicalStaff.technicalStaffRoleId = $scope.technicalStaff.roleId;

            $scope.technicalStaffRoles = data.technicalStaffRoles;

            $scope.imageFiles = {};

            $scope.uploadProgressBars = {};
            $scope.uploadProgressBars.userImage = {
                value: 0
            };

            $scope.submitButton = { text: "ثبت اطلاعات", disable: false };

            $scope.imageFiles.userImageUrl = '/file/home/gettechnicalstaffimage?fileName=' + $scope.technicalStaff.image;


            $scope.addTechnicalStaff = function (frmAddTechnicalStaff) {

                var tmpDate = moment($scope.birthDate.year + '/' +
                    $scope.birthDate.month + '/' +
                    $scope.birthDate.day, 'jYYYY/jM/jD');
                if (!tmpDate.isValid()) {

                    toaster.pop('error', "", "تاریخ تولد اشتباه است");
                    //frmAddTechnicalStaff.birthDateDay.$setValidity("number", false);
                    //frmAddTechnicalStaff.birthDateMonth.$setValidity("number", false);
                    //frmAddTechnicalStaff.birthDateYear.$setValidity("number", false);
                    return;
                }

                $scope.technicalStaff.birthDate = tmpDate.format('YYYY/M/D');

                $scope.submitButton.text = "لطفا منتظر بمانید...";
                $scope.submitButton.disable = true;

                commonTechnicalStaffService.editTechnicalStaff($scope.technicalStaff).then(function onSuccess(data) {

                    toaster.pop('success', '', 'مشخصات خدمه با موفقیت در سیستم ویرایش شد.');

                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                    $scope.errors = [];

                }, function onError(errorData) {

                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                    $scope.errors = errorData.data;

                });


            }

            $scope.uploadUserImage = function (files) {

                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        Upload.upload({
                            url: '/file/home/uploaduserimage',
                            //fields: { 'username': $scope.username },
                            file: file
                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.uploadProgressBars.userImage.value = progressPercentage;
                        }).success(function (data, status, headers, config) {
                            //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                            $scope.imageFiles.userImageUrl = data.url;
                            $scope.technicalStaff.image = data.name;
                        });
                    }
                }
            };

        }
    ]);

    app.controller('CommonTechnicalStaffsListController', [
        '$scope', '$routeParams', 'toaster', 'data', 'commonTechnicalStaffService', '$modal', function ($scope, $routeParams, toaster, data, commonTechnicalStaffService, $modal) {


            $scope.competitionId = $routeParams.competitionId;

            $scope.technicalStaffsList = data.technicalStaffs;

            $scope.displayedTechnicalStaffsList = [].concat($scope.technicalStaffsList);


            $scope.competitionName = data.competitionName;
            $scope.maxTechnicalStaffs = data.maxTechnicalStaffs;

            $scope.deleteTechnicalStaff = function (technicalStaff) {

                var modalInstance = $modal.open({
                    templateUrl: 'deleteTechnicalStaff.html',
                    controller: 'DeleteCommonTechnicalStaffController',
                    resolve: {
                        technicalStaff: function () {
                            technicalStaff.participationId = $scope.participationId;
                            return technicalStaff;
                        }
                    }
                });

                modalInstance.result.then(function () {

                    for (var i = 0; i < $scope.technicalStaffsList.length; i++) {
                        if ($scope.technicalStaffsList[i].id == technicalStaff.id) {
                            $scope.technicalStaffsList.splice(i, 1);
                            return;
                        }
                    }

                }, function () {

                });

            }


        }
    ]);

    app.controller('DeleteCommonTechnicalStaffController', [
        '$scope', 'technicalStaff', '$modalInstance', 'commonTechnicalStaffService', 'toaster', function ($scope, technicalStaff, $modalInstance, commonTechnicalStaffService, toaster) {

            $scope.technicalStaff = technicalStaff;


            $scope.ok = function () {
                commonTechnicalStaffService.deleteTechnicalStaff(technicalStaff.id).then(function onSuccess(data) {
                    $modalInstance.close();
                }, function onError(errorData) {

                    toaster.pop("error", "", errorData.data[0]);

                });
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };

        }
    ]);

    app.controller('TeamColorController', [
        '$scope', 'data', 'participationService', '$routeParams', 'regExes', 'toaster', function ($scope, data, participationService, $routeParams, regExes, toaster) {

            $scope.persianRegex = regExes.persianChars;

            $scope.submitButton = { text: "ثبت اطلاعات", disable: false };

            $scope.competitionSport = data.competitionSport;
            $scope.participationId = $routeParams.participationId;

            $scope.firstColor = data.colors[0];
            $scope.secondColor = data.colors[1];
            $scope.thirdColor = data.colors[2];


            $scope.addTeamColor = function () {

                var teamColors = [];
                teamColors.push($scope.firstColor);
                teamColors.push($scope.secondColor);
                teamColors.push($scope.thirdColor);

                $scope.submitButton.text = "لطفا منتظر بمانید...";
                $scope.submitButton.disable = true;

                participationService.addTeamColor($routeParams.participationId, teamColors).then(function onSuccess(data) {

                    toaster.pop('success', '', 'اطلاعات با موفقیت در سیستم ثبت شد.');

                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                }, function error(data) {

                    $scope.submitButton.text = "ثبت اطلاعات";
                    $scope.submitButton.disable = false;

                });

            };
        }
    ]);

    app.controller('CardsListController', [
        '$scope', 'data', function ($scope, data) {
            $scope.competitionsList = data;
        }
    ]);

    app.controller('LayoutController', [
        '$scope', 'layoutService', '$route', '$window', 'toaster', '$timeout', function ($scope, layoutService, $route, $window, toaster, $timeout) {


            $scope.currentUser = { fullName: '' };

            $scope.$route = $route;

            layoutService.getLayoutData().then(function onSuccess(data) {

                $scope.currentUser = data.user;

            });


            $scope.signOut = function () {

                layoutService.signOut().then(function onSuccess(data) {
                    toaster.pop("success", "", "شما با موفقیت از سیستم خارج شدید");

                    $timeout(function () {
                        $window.location = "/account/#/login";
                    }, 1500);

                });
            }

        }
    ]);


    app.controller('EditRepresentativeUserController', [
        '$scope', 'data', 'representativeUserService', 'regExes', 'toaster', function ($scope, data, representativeUserService, regExes, toaster) {


            $scope.passwordRegEx = regExes.password;
            $scope.numberRegEx = regExes.number;
            $scope.persianRegEx = regExes.persianChars;
            $scope.mobileNumberRegEx = regExes.mobileNumber;

            $scope.errors = [];


            $scope.user = data;


            $scope.registerUser = function () {

                var toasterMessage = toaster.pop('info', '', 'لطفا منتظر بمانید...');

                representativeUserService.editRepresentativeUser($scope.user).then(function success(data) {

                    toaster.clear(toasterMessage);

                    toaster.pop("success", "", 'اطلاعات شما با موفقیت در سیستم ثبت شد، لطفا منتظر بمانید...');


                }, function error(errorData) {

                    $scope.errors = errorData.data;

                    angular.forEach($scope.errors, function (item) {
                        if (item == "پست الکترونیکی وارد شده قبلا توسط فرد دیگری ثبت شده است.") {
                            $scope.user.email = null;
                            return;
                        }
                    });
                    toaster.clear(toasterMessage);
                });
            };

        }]);

    app.controller('ChangePasswordController', [
       '$scope', 'representativeUserService', 'regExes', 'toaster', function ($scope, representativeUserService, regExes, toaster) {

           $scope.passwordRegEx = regExes.password;

           $scope.errors = [];

           $scope.user = {};

           $scope.changePassword = function () {

               representativeUserService.changePassword($scope.user).then(function success(data) {

                   toaster.pop("success", "", 'کلمه عبور با موفقیت تغییر کرد.');

               }, function error(errorData) {

                   $scope.errors = errorData.data;

                   angular.forEach($scope.errors, function (item) {
                       if (item == "کلمه عبور فعلی اشتباه است.") {
                           $scope.user.currentPassword = null;
                           return;
                       }
                   });

               });
           };

       }]);


    app.controller('DashboardController', [
        '$scope', '$routeParams', 'toaster', 'data', 'commonTechnicalStaffService', '$modal', function ($scope, $routeParams, toaster, data, commonTechnicalStaffService, $modal) {


            $scope.competitorsList = data.competitors;

            $scope.displayedCompetitorsList = [].concat($scope.competitorsList);


            $scope.technicalStaffsList = data.technicalStaffs;

            $scope.displayedTechnicalStaffsList = [].concat($scope.technicalStaffsList);

        }

    ]);


    angular
       .module('userPanelApp')
       .controller('AddCompetitorController', AddCompetitorController);

    AddCompetitorController.$inject = ['$scope', '$routeParams', 'participationData', 'toaster', 'regExes', 'participationService', 'Upload', '$document'];

    function AddCompetitorController($scope, $routeParams, participationData, toaster, regExes, participationService, Upload, $document) {

        $scope.errors = [];

        $scope.persianRegex = regExes.persianChars;
        $scope.mobileNumberRegex = regExes.mobileNumber;
        $scope.numberRegEx = regExes.number;


        $scope.competitionSport = participationData.competitionSport;

        $scope.viewModel = {};

        $scope.viewModel.studyFields = participationData.studyFields;
        $scope.viewModel.studyFieldDegrees = participationData.studyFieldDegrees;

        $scope.competitorsCount = participationData.CompetitorsCount;

        $scope.maxCompetitorsNumber = participationData.maxCompetitorsNumber;


        var initial = function () {

            $scope.competitor = {
                participationId: $routeParams.participationId
            }

            $scope.imageFiles = {};

            $scope.birthDate = {};
            $scope.insuranceEndDate = {};

            $scope.competitor.studyFieldId = $scope.viewModel.studyFields[0].id;
            $scope.competitor.studyFieldDegreeId = $scope.viewModel.studyFieldDegrees[0].id;

            $scope.submitButton = { text: "ثبت اطلاعات", disable: false };
        }


        initial();


        $scope.uploadProgressBars = {};

        $scope.uploadProgressBars.userImage = {
            value: 0
        };


        $scope.uploadProgressBars.studentCertificateImage =
        {
            value: 0
        };

        $scope.uploadProgressBars.insuranceImage =
        {
            value: 0
        };

        $scope.uploadProgressBars.azmoonConfirmationImage =
        {
            value: 0
        };

        $scope.uploadUserImage = function (files) {

            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/home/uploaduserimage',
                        //fields: { 'username': $scope.username },
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                        $scope.uploadProgressBars.userImage.value = progressPercentage;

                    }).success(function (data, status, headers, config) {
                        //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                        $scope.imageFiles.userImageUrl = data.url;
                        $scope.competitor.userImage = data.name;
                    });
                }
            }
        };


        $scope.uploadStudentCertificateImage = function (files) {

            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/home/imageUpload',
                        //fields: { 'username': $scope.username },
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                        $scope.uploadProgressBars.studentCertificateImage.value = progressPercentage;

                    }).success(function (data, status, headers, config) {
                        //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                        $scope.imageFiles.studentCertificateImageUrl = data.url;
                        $scope.competitor.studentCertificateImage = data.name;
                    });
                }
            }
        };


        $scope.uploadInsuranceImage = function (files) {

            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/home/imageUpload',
                        //fields: { 'username': $scope.username },
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                        $scope.uploadProgressBars.insuranceImage.value = progressPercentage;

                    }).success(function (data, status, headers, config) {
                        //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                        $scope.imageFiles.insuranceImageUrl = data.url;
                        $scope.competitor.insuranceImage = data.name;
                    });
                }
            }
        };

        $scope.uploadAzmoonConfirmationImage = function (files) {

            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    Upload.upload({
                        url: '/file/home/imageUpload',
                        //fields: { 'username': $scope.username },
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                        $scope.uploadProgressBars.azmoonConfirmationImage.value = progressPercentage;

                    }).success(function (data, status, headers, config) {
                        //console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                        $scope.imageFiles.azmoonConfirmationImageUrl = data.url;
                        $scope.competitor.azmoonConfirmationImage = data.name;
                    });
                }
            }
        };



        $scope.addCompetitor = function (frmAddCompetitor) {



            var tmpDate = moment($scope.birthDate.year + '/' +
                $scope.birthDate.month + '/' +
                $scope.birthDate.day, 'jYYYY/jM/jD');
            if (!tmpDate.isValid()) {

                toaster.pop('error', "", "تاریخ تولد اشتباه است");
                frmAddCompetitor.birthDateDay.$setValidity("number", false);
                frmAddCompetitor.birthDateMonth.$setValidity("number", false);
                frmAddCompetitor.birthDateYear.$setValidity("number", false);
                return;
            }

            $scope.competitor.birthDate = tmpDate.format('YYYY/M/D');

            tmpDate = moment($scope.insuranceEndDate.year + '/' +
                $scope.insuranceEndDate.month + '/' +
                $scope.insuranceEndDate.day, 'jYYYY/jM/jD');
            if (!tmpDate.isValid()) {

                toaster.pop('error', "", "تاریخ پایان بیمه اشتباه است");
                return;
            }

            $scope.competitor.insuranceEndDate = tmpDate.format('YYYY/M/D');

            $scope.submitButton.text = "لطفا منتظر بمانید...";
            $scope.submitButton.disable = true;

            participationService.addCompetitor($scope.competitor).then(function success(data) {

                toaster.pop('success', '', 'بازیکن جدید با موفقیت در سیستم ثبت شد.');

                $scope.competitionSport.competitorsCount = $scope.competitionSport.competitorsCount + 1;
                frmAddCompetitor.$setPristine();
                initial();

                var someElement = angular.element(document.getElementById('topSection'));
                $document.scrollToElement(someElement, 0, 500);

                $scope.errors = [];

            }, function error(errorData) {

                $scope.submitButton.text = "ثبت اطلاعات";
                $scope.submitButton.disable = false;

                $scope.errors = errorData.data;

            });
        }

    }



    app.controller('ParticipationController', ParticipationController);

    ParticipationController.$inject = ['$scope', '$routeParams', 'competition', 'participationService', '$modal', 'toaster', '$location'];

    function ParticipationController($scope, $routeParams, competition, participationService, $modal, toaster, $location) {

        $scope.serach = {};

        $scope.clearSearch = function () {
            $scope.search.name = '';
        };

        $scope.submitButton = {
            text: "ثبت اطلاعات",
            disabled: false
        };



        $scope.competition = competition;

        $scope.competitionSportsList = competition.competitionSports;

        $scope.displayedCompetitionSportsList = [].concat($scope.competitionSportsList);

        $scope.selectedCompetitionSports = [];

        angular.forEach($scope.competitionSportsList, function (item) {

            if (item.isParticipated === true) {
                $scope.selectedCompetitionSports.push(item.id);
            }

        });

        $scope.toggleSelection = function toggleSelection(csId) {

            var idx = $scope.selectedCompetitionSports.indexOf(csId);

            // is currently selected
            if (idx > -1) {
                $scope.selectedCompetitionSports.splice(idx, 1);
            }

                // is newly selected
            else {
                $scope.selectedCompetitionSports.push(csId);
            }
        };

        $scope.sumbitParticipationData = function () {

            $scope.submitButton.disabled = true;
            $scope.submitButton.text = 'لطفا منتظر بمانید ...';

            participationService.addParticipation($scope.selectedCompetitionSports, $routeParams.competitionId).then(function onSuccess(data) {
                $scope.submitButton.disabled = false;
                $scope.submitButton.text = 'ثبت اطلاعات';
                $location.path("/readiness/competitionsportslist/confirm/" + $routeParams.competitionId);

            }, function onError(data) {
                $scope.submitButton.disabled = false;
                $scope.submitButton.text = 'ثبت اطلاعات';
            });
        }

        $scope.showRuleModal = function (competitionSport) {

            var modalInstance = $modal.open({
                templateUrl: 'competitionSportRuleModal.html',
                controller: 'CompetitionSportRuleModalController',
                size: 'lg',
                resolve: {
                    competitionSport: function () {
                        return competitionSport;
                    }
                }
            });

            modalInstance.result.then(function () {

            }, function () {

            });
        };

    }

    app.controller('CompetitionSportRuleModalController', ['$scope', 'competitionSport', '$modalInstance', '$sce', function ($scope, competitionSport, $modalInstance, $sce) {

        $scope.competitionSport = competitionSport;

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.toTrustedHTML = function (html) {
            return $sce.trustAsHtml(html);
        }

    }]);


    app.controller('ConfirmReadinessController', ['$scope', 'data', '$modal', '$timeout', 'toaster', '$window', function ($scope, data, $modal, $timeout, toaster, $window) {

        $scope.competition = data;

        $scope.competitionSportsList = data.competitionSports;

        $scope.displayedCompetitionSportsList = [].concat($scope.competitionSportsList);

        $scope.selectedCompetitionSports = [];

        $scope.sumbitParticipationData = function () {

            toaster.pop("success", "", 'اطلاعات شما با موفقیت در سیستم ثبت شد');

            $timeout(function () {

                $window.location.href = "/userpanel/#/dashboard";

            }, 1000);

        }


        $scope.showRuleModal = function (competitionSport) {

            var modalInstance = $modal.open({
                templateUrl: 'competitionSportRuleModal.html',
                controller: 'CompetitionSportRuleModalController',
                size: 'lg',
                resolve: {
                    competitionSport: function () {
                        return competitionSport;
                    }
                }
            });

            modalInstance.result.then(function () {

            }, function () {

            });
        };

    }]);


})();
