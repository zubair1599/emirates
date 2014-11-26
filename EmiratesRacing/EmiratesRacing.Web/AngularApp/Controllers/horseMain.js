

app.controller('horseMain', ['$scope', horseMain]);

function horseMain($scope) {

    this.horse = "";
    this.horseBreed = "";
    this.horseOwner = "";
    this.horseHorseValue = "";
    this.selectedHorseJson = "";
    var ty = this;



    //$scope.$watch('selectedHorse', function (value) {
    //    ty.horse = value;
    //});
    //$scope.$watch('selectedHorseBreed', function (value) {
    //    ty.horseBreed = value;
    //});
    //$scope.$watch('selectedHorseOwner', function (value) {
    //    ty.horseOwner = value;
    //});
    //$scope.$watch('selectedHorseValue', function (value) {
    //    ty.horseHorseValue = value;
    //});

    //$scope.$watch('resultJson', function (value) {
    //    ty.selectedHorseJson = value;
    //    $scope.resultJsonMain = value;
    //    //$timeout(function () {
    //    //    $scope.$apply();
    //   // });
    //});



};