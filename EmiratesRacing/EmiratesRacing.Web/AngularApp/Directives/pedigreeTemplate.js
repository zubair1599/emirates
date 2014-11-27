app.directive('pedigreeTemplate', ['setSelectedHorse', '$timeout','$rootScope', Directive]);

function Directive(setSelectedHorse, $timeout,$rootScope) {

    return {
        scope: {
            gethorse: '=gethorse',
            horseDetails: '=json',
            comingController :'=controller'
    },

    controller: function ($scope, $element) {

            //$scope.$watch('$parent.mainC.selectedHorseJson', function (data) {
            //    $scope.horseDetails = data;
               
            //});
        alert();


    },
        controllerAs: 'pedigreeC',
        restrict: 'E',
        templateUrl: '/StaticViews/PeDigreeTemplate.html'
    };

};