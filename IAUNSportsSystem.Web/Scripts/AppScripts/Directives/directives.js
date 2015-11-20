(function () {
    'use strict';

    angular
        .module('sportSystem.directives', [])
        .directive('uniqueInput', uniqueInput);

    uniqueInput.$inject = ['$window','$http'];

    function uniqueInput($window,$http) {
        var directive = {
            link: link,
            restrict: 'A',
            require: 'ngModel'
        };
        return directive;


        function link(scope, element, attrs,ngModel) {

            element.bind('blur', function (e) {

                ngModel.$setValidity('unique', true);

                $http.get(attrs.uniqueInput, {
                    params: {
                        term : element.val()
                    }
                }).success(function (data) {
                    if (data) {
                        ngModel.$setValidity('unique', false);
                    }
                    else {
                        ngModel.$setValidity('unique', true);
                    }

                });

            });
        }

    }

})();