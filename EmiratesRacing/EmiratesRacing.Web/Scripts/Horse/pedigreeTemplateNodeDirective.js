


app.directive('pedigreeTemplateNode', Directive);

function Directive() {

    return {
        scope: {
            rowSpan :'=rowspan', 
            colSpan :'=colspan', 
            horse:'=horse'

        },
        restrict: 'A',
        templateUrl: '/StaticViews/PeDigreeNode.html',
        controllerAs: 'node',
        controller: function ($scope) {
            
           // alert($scope.name);
            this.rowSpan = $scope.rowSpan;
            this.colSpan = $scope.colSpan;
            this.horse = $scope.horse;
            var date = new Date($scope.horse.DateOfBirth);
            $scope.horse.DateOfBirth = date.getFullYear();
                
                
            
            

        }

    };

};