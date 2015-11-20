(function () {
    'use strict';


    var myApp = angular.module("sportSystem.commonConstants", []);

    myApp.constant('regExes', (function () {
        // Define your variable
        var persianCharacters = /^([\u0600-\u06FF]+\s?)+$/;
        var mobileNumber = /^09\d{9}$/;
        var password = /^(?=.*\d)(?=.*[a-zA-Z]).{0,}$/;
        var number = /^\d+$/;

        // Use the variable in your constants
        return {

            persianChars: persianCharacters,
            mobileNumber: mobileNumber,
            password: password,
            number: number
        }
    })());



})();