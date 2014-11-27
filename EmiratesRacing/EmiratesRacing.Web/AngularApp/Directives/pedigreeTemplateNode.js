


app.directive('pedigreeTemplateNode',['setSelectedHorse','$rootScope', Directive]);

function Directive(setSelectedHorse,$rootScope) {

    return {
        scope: {
            rowSpan :'=rowspan', 
            colSpan :'=colspan', 
            horse:'=horse',
            gethorse:'=gethorse'
        }, 
        restrict: 'A',
        templateUrl: '/StaticViews/PeDigreeNode.html',
        controllerAs: 'node',
        controller: function ($scope) {
            
           // alert($scope.name);
            this.rowSpan = $scope.rowSpan;
            this.colSpan = $scope.colSpan;
            this.horse = $scope.horse;
            //$scope.YearOfBirth = new Date($scope.horse.DateOfBirth).getFullYear();
            //$scope.HorseDetails = $scope.horse.HorseDetails;
            
            $scope.NewPedigree = function () {

               //$scope.$parent.$parent.mainC.GetSelectedJson($scope.horse.Id);
                $scope.gethorse($scope.horse.id);

            }
          
            

        }

    };

};