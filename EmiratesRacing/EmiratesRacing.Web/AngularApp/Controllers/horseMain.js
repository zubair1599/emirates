

app.controller('horseMain', ['$scope', horseMain]);

function horseMain($scope) {

    this.horse = "";
    this.horseBreed = "";
    this.horseOwner = "";
    this.horseHorseValue = "";
    var ty = this;



    $scope.$watch('selectedHorse', function (value) {
        ty.horse = value;
    });
    $scope.$watch('selectedHorseBreed', function (value) {
        ty.horseBreed = value;
    });
    $scope.$watch('selectedHorseOwner', function (value) {
        ty.horseOwner = value;
    });
    $scope.$watch('selectedHorseValue', function (value) {
        ty.horseHorseValue = value;
    });



};