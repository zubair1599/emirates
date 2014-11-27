

app.controller('horseMain', ['$scope','setSelectedHorse', horseMain]);

function horseMain($scope, setSelectedHorse) {

    this.horse = "";
    this.horseBreed = "";
    this.horseOwner = "";
    this.horseHorseValue = "";
    this.selectedHorse = '';
    var ty = this;

    $scope.GetSelectedJson = function(id) {
        setSelectedHorse.GetHorseJSON(id);
        setSelectedHorse.servicePromise.promise.then(function (data) {
            ty.selectedHorse= data;

        }, function (data) {
            alert("error :" + data);
        });

    };


};