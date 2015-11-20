(function () {
    'use strict';

    angular
        .module('userPanelApp')
        .factory('participationService', participationService);

    participationService.$inject = ['$http'];

    function participationService($http) {


        var getCompetitionSportsList = function (competitionId) {
            return $http.get("/participation/home/getcompetitionsportslist", {
                params: {
                    competitionId: competitionId
                }
            }).then(function (response) {
                return response.data;
            });

        }

        var addParticipation = function (competitionSports, competitionId) {
            return $http.post("/participation/home/add",
                {
                    competitionId: competitionId,
                    competitionSports: competitionSports
                }).then(function (response) {
                    return response.data;
                });

        }

        var getAddCompetitorData = function (participationId) {
            return $http.post("/participation/competitor/getaddcompetitordata",
                {
                    participationId: participationId
                }).then(function (response) {
                    return response.data;
                });

        }

        var addCompetitor = function (competitor) {
            return $http.post("/participation/competitor/add", competitor).then(function (response) {
                return response.data;
            });

        }

        var getReadyCompetitionsList = function () {
            return $http.get("/competition/home/getreadycompetitionslist").then(function (response) {
                return response.data;
            });

        };


        var getUserReadyCompetitionsList = function () {
            return $http.get("/competition/home/getuserreadycompetitionslist").then(function (response) {
                return response.data;
            });

        };

        var getApprovedCompetitionParticipationsList = function (competitionId) {
            return $http.get("/competitionsport/home/getapprovedcompetitionparticipationslist", {
                params: {
                    competitionId: competitionId
                }
            }).then(function (response) {
                return response.data;
            });

        };

        var addTeamColor = function (participationId, colors) {
            return $http.post("/participation/userpanel/addteamcolor", {
                participationId: participationId,
                colors: colors
            }).then(function (response) {
                return response.data;
            });

        }


        var getTeamColors = function (participationId) {
            return $http.get("/participation/userpanel/getteamcolors", {
                params: {
                    participationId: participationId
                }
            }).then(function (response) {
                return response.data;
            });

        };

        var service = {
            getCompetitionSportsList: getCompetitionSportsList,
            addParticipation: addParticipation,
            getAddCompetitorData: getAddCompetitorData,
            addCompetitor: addCompetitor,
            getReadyCompetitionsList: getReadyCompetitionsList,
            getUserReadyCompetitionsList: getUserReadyCompetitionsList,
            getApprovedCompetitionParticipationsList: getApprovedCompetitionParticipationsList,
            addTeamColor: addTeamColor,
            getTeamColors: getTeamColors
        };

        return service;
    }


    var app = angular
        .module('userPanelApp');
    app.factory('technicalStaffService', ['$http', function ($http) {


        var getAddData = function (participationId) {
            return $http.get("/technicalstaff/userpanel/getadddata", { params: { participationId: participationId } }).then(function (response) {
                return response.data;
            });

        };


        var addTechnicalStaff = function (technicalStaff) {
            return $http.post("/technicalstaff/userpanel/add",
                {
                    technicalStaffModel: technicalStaff
                }).then(function (response) {
                    return response.data;
                });

        }

        var getByNationalCode = function (nationalCode, participationId) {
            return $http.get("/technicalstaff/userpanel/getByNationalCode", {
                params: {
                    nationalCode: nationalCode,
                    participationId: participationId
                }
            }).then(function (response) {
                return response.data;
            });

        };

        var getList = function (participationId) {
            return $http.get("/technicalstaff/userpanel/getlist", { params: { participationId: participationId } }).then(function (response) {
                return response.data;
            });

        };

        var deleteTechnicalStaff = function (technicalStaffId, participationId) {
            return $http.post("/technicalstaff/userpanel/delete",
                {
                    technicalStaffId: technicalStaffId,
                    participationId: participationId
                }).then(function (response) {
                    return response.data;
                });

        }

        var getEditData = function (technicalStaffId, participationId) {
            return $http.get("/technicalstaff/userpanel/geteditdata", { params: { participationId: participationId, technicalStaffId: technicalStaffId } }).then(function (response) {
                return response.data;
            });

        };

        var editTechnicalStaff = function (technicalStaff) {
            return $http.post("/technicalstaff/userpanel/edit",
                {
                    technicalStaffModel: technicalStaff,
                }).then(function (response) {
                    return response.data;
                });

        }

        var service = {
            addTechnicalStaff: addTechnicalStaff,
            getAddData: getAddData,
            getByNationalCode: getByNationalCode,
            getList: getList,
            deleteTechnicalStaff: deleteTechnicalStaff,
            getEditData: getEditData,
            editTechnicalStaff: editTechnicalStaff
        };



        return service;

    }]);

    app.factory('competitorService', ['$http', function ($http) {


        var getList = function (participationId) {
            return $http.get("/competitor/userpanel/index", { params: { participationId: participationId } }).then(function (response) {
                return response.data;
            });

        };


        var deleteCompetitor = function (competitorId) {
            return $http.post("/competitor/userpanel/delete",
                {
                    competitorId: competitorId
                }).then(function (response) {
                    return response.data;
                });

        }

        var getCompetitor = function (competitorId, participationId) {
            return $http.get("/competitor/userpanel/get", { params: { competitorId: competitorId, participationId: participationId } }).then(function (response) {
                return response.data;
            });

        };


        var editCompetitor = function (competitor) {
            return $http.post("/competitor/userpanel/edit",
                {
                    competitorModel: competitor
                }).then(function (response) {
                    return response.data;
                });

        }


        var service = {
            getList: getList,
            deleteCompetitor: deleteCompetitor,
            getCompetitor: getCompetitor,
            editCompetitor: editCompetitor
        };

        return service;

    }]);


    var app = angular
       .module('userPanelApp');
    app.factory('commonTechnicalStaffService', ['$http', function ($http) {


        var getAddData = function (competitionId) {
            return $http.get("/commontechnicalstaff/userpanel/getadddata", {
                params: {
                    competitionId: competitionId
                }
            }).then(function (response) {
                return response.data;
            });

        };


        var addTechnicalStaff = function (technicalStaff, competitionId) {
            return $http.post("/commontechnicalstaff/userpanel/add",
                {
                    technicalStaffModel: technicalStaff,
                    competitionId: competitionId
                }).then(function (response) {
                    return response.data;
                });

        }

        //var getByNationalCode = function (nationalCode) {
        //    return $http.get("/technicalstaff/userpanel/getByNationalCode", {
        //        params: {
        //            nationalCode: nationalCode
        //        }
        //    }).then(function (response) {
        //        return response.data;
        //    });

        //};

        var getList = function (competitionId) {
            return $http.get("/commontechnicalstaff/userpanel/getlist", { params: { competitionId: competitionId } }).then(function (response) {
                return response.data;
            });

        };

        var deleteTechnicalStaff = function (technicalStaffId) {
            return $http.post("/commontechnicalstaff/userpanel/delete",
                {
                    technicalStaffId: technicalStaffId,
                }).then(function (response) {
                    return response.data;
                });

        }

        var getEditData = function (technicalStaffId) {
            return $http.get("/commontechnicalstaff/userpanel/geteditdata", { params: { technicalStaffId: technicalStaffId } }).then(function (response) {
                return response.data;
            });

        };

        var editTechnicalStaff = function (technicalStaff) {
            return $http.post("/commontechnicalstaff/userpanel/edit",
                {
                    technicalStaffModel: technicalStaff,
                }).then(function (response) {
                    return response.data;
                });

        }

        var service = {
            addTechnicalStaff: addTechnicalStaff,
            getAddData: getAddData,
            //getByNationalCode: getByNationalCode,
            getList: getList,
            deleteTechnicalStaff: deleteTechnicalStaff,
            getEditData: getEditData,
            editTechnicalStaff: editTechnicalStaff
        };



        return service;

    }]);


    app.factory('redinessService', ['$http', function ($http) {


        var getConfirmCompetitionSportsList = function (competitionId) {
            return $http.get("/participation/home/getconfirmcompetitionsportslist", {
                params: {
                    competitionId: competitionId
                }
            }).then(function (response) {
                return response.data;
            });

        };


        var service = {
            getConfirmCompetitionSportsList: getConfirmCompetitionSportsList,
        };



        return service;

    }]);


    app.factory('cardService', ['$http', function ($http) {


        var getCardsList = function (competitionId) {
            return $http.get("/card/userpanel/getcardslist").then(function (response) {
                return response.data;
            });

        };


        var service = {
            getCardsList: getCardsList,
        };



        return service;

    }]);


    app.factory('layoutService', ['$http', function ($http) {


        var getLayoutData = function () {
            return $http.get("/userpanel/home/getlayoutdata").then(function (response) {
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


    app.factory('representativeUserService', ['$http', function ($http) {


        var getRepresentativeUser = function () {
            return $http.get("/representativeuser/userpanel/geteditdata").then(function (response) {
                return response.data;
            });

        };



        var editRepresentativeUser = function (representativeUser) {
            return $http.post("/representativeuser/userpanel/edit", {
                representativeUserModel: representativeUser
            }).then(function (response) {
                return response.data;
            });

        }

        var changePassword = function (changePasswordModel) {
            return $http.post("/representativeuser/userpanel/changepassword", {
                changePasswordModel: changePasswordModel
            }).then(function (response) {
                return response.data;
            });

        }



        var service = {
            getRepresentativeUser: getRepresentativeUser,
            editRepresentativeUser: editRepresentativeUser,
            changePassword: changePassword
        };

        return service;

    }]);


    app.factory('dashboardUserService', ['$http', function ($http) {


        var getRejectedPersons = function () {

            return $http.get("/userpanel/home/getrejectedpersons").then(function (response) {
                return response.data;
            });

        };

        var service = {
            getRejectedPersons: getRejectedPersons,
        };

        return service;

    }]);


})();