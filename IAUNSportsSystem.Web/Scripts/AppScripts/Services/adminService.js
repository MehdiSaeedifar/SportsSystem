(function () {
    'use strict';

    angular
        .module('app')
        .factory('readinessService', readinessService);

    readinessService.$inject = ['$http'];

    function readinessService($http) {

        var getCompetitionSportUsers = function (competitionSportId) {

            return $http.get("/readiness/admin/getcompetitionsportuserslist", { params: { competitionSportId: competitionSportId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var approve = function (competitionSportId, userId) {

            return $http.get("/participation/home/approve", { params: { competitionSportId: competitionSportId, userId: userId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var reject = function (competitionSportId, userId) {

            return $http.get("/participation/home/reject", { params: { competitionSportId: competitionSportId, userId: userId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getCompetitorsList = function (participationId) {

            return $http.get("/participation/competitor/getcompetitorslist", { params: { participationId: participationId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getCompetitor = function (competitorId) {

            return $http.get("/participation/competitor/getcompetitor", { params: { competitorId: competitorId } })
                .then(function (response) {
                    return response.data;
                });
        };


        var editApproval = function (competitor) {

            return $http.post("/participation/competitor/editapproval", competitor)
                .then(function (response) {
                    return response.data;
                });
        };

        var getSportsList = function () {

            return $http.get("/sport/home/getsportslist")
                .then(function (response) {
                    return response.data;
                });
        };

        var addSport = function (sport) {

            return $http.post("/sport/home/add", sport)
                .then(function (response) {
                    return response.data;
                });
        };

        var deleteSport = function (sportId) {

            return $http.post("/sport/home/delete", { sportId: sportId })
                .then(function (response) {
                    return response.data;
                });
        };

        var getSportWithCategoriesAndDetails = function (sportId) {

            return $http.get("/sport/home/getsportwithcategoriesanddetails", { params: { sportId: sportId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var editSport = function (sport) {

            return $http.post("/sport/home/editsport", { sportModel: { id: sport.id, name: sport.name } })
                .then(function (response) {
                    return response.data;
                });
        };

        var addSportCategory = function (sportCategory) {

            return $http.post("/sport/home/addsportcategory", {
                sportCategoryModel: {
                    sportId: sportCategory.sportId,
                    sportCategoryName: sportCategory.sportCategoryName
                }
            }).then(function (response) {
                return response.data;
            });
        };

        var addSportDetail = function (sportDetail) {

            return $http.post("/sport/home/addsportdetail", {
                sportDetailModel: {
                    sportId: sportDetail.sportId,
                    sportDetailName: sportDetail.sportDetailName
                }
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteSportCategory = function (sportCategoryId) {

            return $http.post("/sport/home/deletesportcategory", {
                sportCategoryId: sportCategoryId
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteSportDetail = function (sportDetailId) {

            return $http.post("/sport/home/deletesportdetail", {
                sportDetailId: sportDetailId
            }).then(function (response) {
                return response.data;
            });
        };

        var getUniversitiesList = function () {

            return $http.get("/university/home/getuniversitieslist")
                .then(function (response) {
                    return response.data;
                });
        };

        var addUniversity = function (university) {

            return $http.post("/university/home/add", {
                universityModel: { name: university.name }
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteUniversity = function (universityId) {

            return $http.post("/university/home/delete", {
                universityId: universityId
            }).then(function (response) {
                return response.data;
            });
        };

        var editUniversity = function (university) {

            return $http.post("/university/home/edit", {
                id: university.id,
                name: university.name
            }).then(function (response) {
                return response.data;
            });
        };


        var getStudyFieldsList = function () {

            return $http.get("/studyfield/home/getall")
                .then(function (response) {
                    return response.data;
                });
        };

        var addStudyField = function (studyField) {

            return $http.post("/studyfield/home/add", {
                studyFieldModel: { name: studyField.name }
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteStudyField = function (studyFieldId) {

            return $http.post("/studyfield/home/delete", {
                studyFieldId: studyFieldId
            }).then(function (response) {
                return response.data;
            });
        };

        var editStudyField = function (studyField) {

            return $http.post("/studyfield/home/edit", {
                id: studyField.id,
                name: studyField.name
            }).then(function (response) {
                return response.data;
            });
        };


        var getStudyFieldDegreesList = function () {

            return $http.get("/studyfielddegree/home/getall")
                .then(function (response) {
                    return response.data;
                });
        };

        var addStudyFieldDegree = function (studyFieldDegree) {

            return $http.post("/studyfielddegree/home/add", {
                studyFieldDegreeModel: { name: studyFieldDegree.name }
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteStudyFieldDegree = function (studyFieldDegreeId) {

            return $http.post("/studyfielddegree/home/delete", {
                studyFieldDegreeId: studyFieldDegreeId
            }).then(function (response) {
                return response.data;
            });
        };

        var editStudyFieldDegree = function (studyFieldDegree) {

            return $http.post("/studyfielddegree/home/edit", {
                id: studyFieldDegree.id,
                name: studyFieldDegree.name
            }).then(function (response) {
                return response.data;
            });
        };


        var getUser = function (userId) {

            return $http.get("/user/home/get", { params: { userId: userId } }).then(function (response) {
                return response.data;
            });
        };

        var getUsersList = function () {

            return $http.get("/user/home/getall")
                .then(function (response) {
                    return response.data;
                });
        };

        var addUser = function (user) {

            return $http.post("/user/home/add", {
                userModel: user
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteUser = function (userId) {

            return $http.post("/user/home/delete", {
                userId: userId
            }).then(function (response) {
                return response.data;
            });
        };

        var editUser = function (user) {

            return $http.post("/user/home/edit", {
                userModel: user
            }).then(function (response) {
                return response.data;
            });
        };



        var service = {
            getCompetitionSportUsers: getCompetitionSportUsers,
            approve: approve,
            reject: reject,
            getCompetitorsList: getCompetitorsList,
            getCompetitor: getCompetitor,
            editApproval: editApproval,
            getSportsList: getSportsList,
            addSport: addSport,
            deleteSport: deleteSport,
            getSportWithCategoriesAndDetails: getSportWithCategoriesAndDetails,
            editSport: editSport,
            addSportCategory: addSportCategory,
            addSportDetail: addSportDetail,
            deleteSportCategory: deleteSportCategory,
            deleteSportDetail: deleteSportDetail,
            getUniversitiesList: getUniversitiesList,
            addUniversity: addUniversity,
            deleteUniversity: deleteUniversity,
            editUniversity: editUniversity,
            getStudyFieldsList: getStudyFieldsList,
            addStudyField: addStudyField,
            deleteStudyField: deleteStudyField,
            editStudyField: editStudyField,
            getStudyFieldDegreesList: getStudyFieldDegreesList,
            addStudyFieldDegree: addStudyFieldDegree,
            deleteStudyFieldDegree: deleteStudyFieldDegree,
            editStudyFieldDegree: editStudyFieldDegree,
            getUser: getUser,
            getUsersList: getUsersList,
            addUser: addUser,
            deleteUser: deleteUser,
            editUser: editUser,

        };

        return service;


    }

    var app = angular
        .module('app');
    app.factory('slideShowService', ['$http', function ($http) {

        var getSlideShow = function (slideShowItemId) {

            return $http.get("/slideshow/home/get", { params: { slideShowItemId: slideShowItemId } }).then(function (response) {
                return response.data;
            });
        };

        var getSlideShowsList = function () {

            return $http.get("/slideshow/home/getall")
                .then(function (response) {
                    return response.data;
                });
        };

        var addSlideShow = function (slideShowItemModel) {

            return $http.post("/slideshow/home/add", {
                slideShowItemModel: slideShowItemModel
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteSlideShow = function (slideShowItemId) {

            return $http.post("/slideshow/home/delete", {
                slideShowItemId: slideShowItemId
            }).then(function (response) {
                return response.data;
            });
        };

        var editSlideShow = function (slideShowItemModel) {

            return $http.post("/slideshow/home/edit", {
                slideShowItemModel: slideShowItemModel
            }).then(function (response) {
                return response.data;
            });
        };


        var service = {
            getSlideShow: getSlideShow,
            getSlideShowsList: getSlideShowsList,
            addSlideShow: addSlideShow,
            deleteSlideShow: deleteSlideShow,
            editSlideShow: editSlideShow
        };

        return service;

    }]);


    app.factory('slideShowImageService', ['$http', function ($http) {

        var getAllImages = function () {

            return $http.get("/file/slideshow/getallimages").then(function (response) {
                return response.data;
            });
        }

        var deleteSlideImage = function (fileName) {

            return $http.post("/file/slideshow/delete", {
                fileName: fileName
            }).then(function (response) {
                return response.data;
            });
        }

        var service = {
            getAllImages: getAllImages,
            deleteSlideImage: deleteSlideImage
        };

        return service;

    }]);

    app.factory('dormService', ['$http', function ($http) {

        var getAll = function () {

            return $http.get("/dorm/admin/getall").then(function (response) {
                return response.data;
            });
        }

        var addDorm = function (dorm) {

            return $http.post("/dorm/admin/add", {
                dormModel: dorm
            }).then(function (response) {
                return response.data;
            });
        };

        var editDorm = function (dorm) {

            return $http.post("/dorm/admin/edit", {
                dormModel: dorm
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteDorm = function (dormId) {

            return $http.post("/dorm/admin/delete", {
                dormId: dormId
            }).then(function (response) {
                return response.data;
            });
        };

        var service = {
            getAll: getAll,
            deleteDorm: deleteDorm,
            editDorm: editDorm,
            addDorm: addDorm
        };

        return service;

    }]);


    app.factory('technicalStaffRoleService', ['$http', function ($http) {

        var getAll = function () {

            return $http.get("/technicalstaffrole/admin/getall").then(function (response) {
                return response.data;
            });
        }

        var addTechnicalStaffRole = function (technicalStaffRole) {

            return $http.post("/technicalstaffrole/admin/add", {
                technicalStaffRoleModel: technicalStaffRole
            }).then(function (response) {
                return response.data;
            });
        };

        var editTechnicalStaffRole = function (technicalStaffRole) {

            return $http.post("/technicalstaffrole/admin/edit", {
                technicalStaffRoleModel: technicalStaffRole
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteTechnicalStaffRole = function (technicalStaffRoleId) {

            return $http.post("/technicalstaffrole/admin/delete", {
                technicalStaffRoleId: technicalStaffRoleId
            }).then(function (response) {
                return response.data;
            });
        };

        var service = {
            getAll: getAll,
            deleteTechnicalStaffRole: deleteTechnicalStaffRole,
            editTechnicalStaffRole: editTechnicalStaffRole,
            addTechnicalStaffRole: addTechnicalStaffRole
        };

        return service;

    }]);

    app.factory('representativeUserService', ['$http', function ($http) {

        var getAll = function () {

            return $http.get("/representativeuser/admin/getall").then(function (response) {
                return response.data;
            });
        }

        var addRepresentativeUser = function (representativeUser) {

            return $http.post("/representativeuser/admin/add", {
                representativeUserModel: representativeUser
            }).then(function (response) {
                return response.data;
            });
        };

        var editRepresentativeUser = function (representativeUser) {

            return $http.post("/representativeuser/admin/edit", {
                representativeUserModel: representativeUser
            }).then(function (response) {
                return response.data;
            });
        };

        var deleteRepresentativeUser = function (representativeUserId) {

            return $http.post("/representativeuser/admin/delete", {
                representativeUserId: representativeUserId
            }).then(function (response) {
                return response.data;
            });
        };

        var service = {
            getAll: getAll,
            deleteRepresentativeUser: deleteRepresentativeUser,
            editRepresentativeUser: editRepresentativeUser,
            addRepresentativeUser: addRepresentativeUser
        };

        return service;

    }]);


    app.factory('competitionSportService', ['$http', function ($http) {

        var getAddData = function (competitionId) {

            return $http.get("/competitionsport/home/getadddata", {
                params: {
                    competitionId: competitionId
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }


        var add = function (competitionSport) {

            return $http.post("/competitionsport/home/add", { competitionSportModel: competitionSport })
                .then(function (response) {
                    return response.data;
                });

        };


        var getCompetitionSportListForReadiness = function (competitionId) {
            return $http.get("/competitionsport/home/getcompetitionsportlistforreadiness", {
                params: { competitionId: competitionId }
            })
                .then(function (response) {
                    return response.data;
                });
        };

        var deleteCompetitionSport = function (competitionSportId) {

            return $http.post("/competitionsport/home/delete", { competitionSportId: competitionSportId })
                .then(function (response) {
                    return response.data;
                });

        };

        var getEditData = function (competitionSportId) {

            return $http.get("/competitionsport/home/geteditdata", {
                params: {
                    competitionSportId: competitionSportId
                }
            }).then(function (response) {
                return response.data;
            });
        }


        var editCompetitionSport = function (competitionSport) {

            return $http.post("/competitionsport/home/edit", { competitionSportModel: competitionSport })
                .then(function (response) {
                    return response.data;
                });

        };

        var service = {
            getAddData: getAddData,
            add: add,
            getCompetitionSportListForReadiness: getCompetitionSportListForReadiness,
            deleteCompetitionSport: deleteCompetitionSport,
            getEditData: getEditData,
            editCompetitionSport: editCompetitionSport
        };

        return service;

    }]);

    app.factory('competitionRepresentativeUserService', ['$http', function ($http) {

        var getAll = function (competitionId) {

            return $http.get("/competitionrepresentativeuser/admin/getall", {
                params: {
                    competitionId: competitionId
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }


        var add = function (competitionId, representativeUsers) {

            return $http.post("/competitionrepresentativeuser/admin/add", {
                competitionId: competitionId,
                representativeUsers: representativeUsers
            })
                .then(function (response) {
                    return response.data;
                });

        };


        var getCompetitionSportListForReadiness = function (competitionId) {
            return $http.get("/competitionsport/home/getcompetitionsportlistforreadiness", {
                params: { competitionId: competitionId }
            })
                .then(function (response) {
                    return response.data;
                });
        };

        var deleteCompetitionSport = function (competitionSportId) {

            return $http.post("/competitionsport/home/delete", { competitionSportId: competitionSportId })
                .then(function (response) {
                    return response.data;
                });

        };

        var getEditData = function (competitionSportId) {

            return $http.get("/competitionsport/home/geteditdata", {
                params: {
                    competitionSportId: competitionSportId
                }
            }).then(function (response) {
                return response.data;
            });
        }


        var editCompetitionSport = function (competitionSport) {

            return $http.post("/competitionsport/home/edit", { competitionSportModel: competitionSport })
                .then(function (response) {
                    return response.data;
                });

        };

        var service = {
            getAll: getAll,
            add: add,
            getCompetitionSportListForReadiness: getCompetitionSportListForReadiness,
            deleteCompetitionSport: deleteCompetitionSport,
            getEditData: getEditData,
            editCompetitionSport: editCompetitionSport
        };

        return service;

    }]);


    app.factory('registerService', ['$http', function ($http) {

        var getRepresentativeUsers = function (competitionId) {

            return $http.get("/register/admin/getrepresentativeusers", {
                params: {
                    competitionId: competitionId
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }


        var getSportsList = function (competitionId, representativeUserId) {

            return $http.get("/register/admin/getsportslist", {
                params: {
                    competitionId: competitionId,
                    representativeUserId: representativeUserId
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }

        var getCompetitorsList = function (participationId) {

            return $http.get("/participation/competitor/getcompetitorslist", { params: { participationId: participationId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getCompetitorEditData = function (competitorId) {

            return $http.get("/register/admin/getcompetitoreditdata", { params: { competitorId: competitorId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getTehnicalStaffsList = function (participationId) {

            return $http.get("/register/admin/gettechnicalstaffslist", { params: { participationId: participationId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getTehnicalStaffEditData = function (technicalStaffId, participationId) {

            return $http.get("/register/admin/gettechnicalstaffeditdata", { params: { technicalStaffId: technicalStaffId, participationId: participationId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var editTechnicalStaffApproval = function (technicalStaff) {

            return $http.post("/register/admin/edittechnicalstaffapproval", { technicalStaffModel: technicalStaff })
                .then(function (response) {
                    return response.data;
                });
        };

        var getCommonTechnicalStaffsList = function (competitionId, representativeUserId) {

            return $http.get("/register/admin/getcommontechnicalstaffslist", { params: { competitionId: competitionId, representativeUserId: representativeUserId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getCommonTehnicalStaffEditData = function (technicalStaffId, participationId) {

            return $http.get("/register/admin/getcommontechnicalstaffeditdata", { params: { technicalStaffId: technicalStaffId, participationId: participationId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var getTeamColors = function (participationId) {

            return $http.get("/register/admin/getteamcolor", { params: { participationId: participationId } })
                .then(function (response) {
                    return response.data;
                });
        };

        var service = {
            getRepresentativeUsers: getRepresentativeUsers,
            getSportsList: getSportsList,
            getCompetitorsList: getCompetitorsList,
            getCompetitorEditData: getCompetitorEditData,
            getTehnicalStaffsList: getTehnicalStaffsList,
            getTehnicalStaffEditData: getTehnicalStaffEditData,
            editTechnicalStaffApproval: editTechnicalStaffApproval,
            getCommonTechnicalStaffsList: getCommonTechnicalStaffsList,
            getCommonTehnicalStaffEditData: getCommonTehnicalStaffEditData,
            getTeamColors: getTeamColors
        };

        return service;

    }]);



    app.factory('announcementService', ['$http', function ($http) {

        var getAddData = function (competitionId) {

            return $http.get("/announcement/admin/getadddata", {
                params: {
                    competitionId: competitionId
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }


        var addAnnouncement = function (announcement) {

            return $http.post("/announcement/admin/add", { announcementModel: announcement })
                .then(function (response) {
                    return response.data;
                });

        };


        var getCompetitionAnnouncementsList = function (competitionId) {
            return $http.get("/announcement/admin/getall", {
                params: { competitionId: competitionId }
            })
                .then(function (response) {
                    return response.data;
                });
        };

        var deleteAnnouncement = function (announcementId) {

            return $http.post("/announcement/admin/delete", { announcementId: announcementId })
                .then(function (response) {
                    return response.data;
                });

        };

        var getEditData = function (announcementId) {

            return $http.get("/announcement/admin/geteditdata", {
                params: {
                    announcementId: announcementId
                }
            }).then(function (response) {
                return response.data;
            });
        }


        var editAnnouncement = function (announcement) {

            return $http.post("/announcement/admin/edit", { announcementModel: announcement })
                .then(function (response) {
                    return response.data;
                });

        };

        var service = {
            getAddData: getAddData,
            addAnnouncement: addAnnouncement,
            getCompetitionAnnouncementsList: getCompetitionAnnouncementsList,
            deleteAnnouncement: deleteAnnouncement,
            getEditData: getEditData,
            editAnnouncement: editAnnouncement
        };

        return service;

    }]);

    app.factory('newsService', ['$http', function ($http) {

        var getAddData = function (competitionId) {

            return $http.get("/competitionsport/home/getadddata", {
                params: {
                    competitionId: competitionId
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }


        var addNews = function (news) {

            return $http.post("/news/admin/add", { newsModel: news })
                .then(function (response) {
                    return response.data;
                });

        };


        var getAll = function () {
            return $http.get("/news/admin/getall")
                .then(function (response) {
                    return response.data;
                });
        };

        var deleteNews = function (newsId) {

            return $http.post("/news/admin/delete", { newsId: newsId })
                .then(function (response) {
                    return response.data;
                });

        };

        var getEditData = function (newsId) {

            return $http.get("/news/admin/geteditdata", {
                params: {
                    newsId: newsId
                }
            }).then(function (response) {
                return response.data;
            });
        }


        var editNews = function (news) {

            return $http.post("/news/admin/edit", { newsModel: news })
                .then(function (response) {
                    return response.data;
                });

        };

        var service = {
            getAddData: getAddData,
            addNews: addNews,
            getAll: getAll,
            deleteNews: deleteNews,
            getEditData: getEditData,
            editNews: editNews
        };

        return service;

    }]);

    angular
        .module('app')
        .factory('competitionService', competitionService);

    competitionService.$inject = ['$http'];

    function competitionService($http) {


        var getAll = function () {

            return $http.get("/competition/home/getall").then(function (response) {
                return response.data;
            });

        }

        var getEditData = function (competitionId) {

            return $http.get("/competition/home/geteditdata", {
                params: {
                    competitionId: competitionId
                }
            }).then(function (response) {
                return response.data;
            });

        }

        var addCompetition = function (competition) {

            return $http.post("/competition/home/add", { competitionModel: competition })
                .then(function (response) {
                    return response.data;
                });

        };

        var editCompetition = function (competition) {

            return $http.post("/competition/home/edit", { competitionModel: competition })
                .then(function (response) {
                    return response.data;
                });

        };

        var getCompetitionListForReadiness = function () {
            return $http.get("/competition/home/getCompetitionlistforreadiness").then(function (response) {
                return response.data;
            });

        };

        var getReadyCompetitionsList = function () {
            return $http.get("/competition/home/getreadycompetitionslist").then(function (response) {
                return response.data;
            });

        };

        var deleteCompetition = function (competitionId) {

            return $http.post("/competition/admin/delete", { competitionId: competitionId })
                .then(function (response) {
                    return response.data;
                });

        };

        var service = {
            getAll: getAll,
            addCompetition: addCompetition,
            getCompetitionListForReadiness: getCompetitionListForReadiness,
            getReadyCompetitionsList: getReadyCompetitionsList,
            getEditData: getEditData,
            editCompetition: editCompetition,
            deleteCompetition: deleteCompetition
        };

        return service;
    }


    app.factory('layoutService', ['$http', function ($http) {


        var getLayoutData = function () {
            return $http.get("/admin/home/getlayoutdata").then(function (response) {
                return response.data;
            });

        };



        var signOut = function () {
            return $http.post("/account/signout").then(function (response) {
                return response.data;
            });

        }


        var service = {
            getLayoutData: getLayoutData,
            signOut: signOut
        };

        return service;

    }]);


})();