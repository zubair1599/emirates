
app.directive('horseSearchBar', ['$rootScope','$timeout', HorseSearchBarDirective]);



function HorseSearchBarDirective ($http,$rootScope,$timeout) {
    return {
        scope: {
            gethorse:'=gethorse'

        },
        controller: function ($scope, $element) {
            $scope.selectedHorse = "";
            $scope.selectedHorseBreed = "";
            $scope.selectedHorseOwner = "";
            $scope.selectedHorseValue = "";
            
            var temp = this;
            
            $scope.$watch('selectedHorseTxt', function (txt) {


                $element.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '/Default/SearchResults?word=' + txt,
                            dataType: "json",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data, function (item) {
                                    return {
                                        name: item.name,
                                        value: item.value,
                                        breed: item.breed,
                                        owner: item.owner
                                    };
                                }));
                            },
                            error: function (error) {
                                alert(error);
                            }
                        });
                    },
                    minLength: 1,
                    select: function (event, ui) {
                        $element.val(ui.item.name);
                        $scope.selectedHorse = ui.item.name;
                        $scope.selectedHorseBreed = ui.item.breed;

                        $scope.selectedHorseOwner = ui.item.owner;
                        $scope.selectedHorseValue = ui.item.value;
                        $scope.$apply();

                        //$scope.GetSelectedJson(ui.item.value);
                        $scope.gethorse(ui.item.value);

                        //setSelectedHorse.GetHorseJSON($scope.selectedHorseValue);
                        
                        //setSelectedHorse.servicePromise.promise.then(function (data) {
                        //    //$scope.resultJson = data;
                        //    $scope.mainC.selectedHorseJson = data;
                        //    $scope.mainC.horse =  ui.item.name;
                        //    $scope.mainC.horseBreed = ui.item.breed;
                        //    $scope.mainC.horseOwner = ui.item.owner;
                        //    $scope.mainC.horseHorseValue = ui.item.value;
                           
                            
                        //}, function (data) {
                        //    alert("error : " + data);
                        //});
                        

                        

                        return false;
                    },

                }).autocomplete("instance")._renderItem = function (ul, item) {
                    var test = 1;
                    return $("<li>")
                      .append("<a> " + item.name + "<br>" + "Breed : " + item.breed + "<br>" + "Owner : " + item.owner + " </a>")
                      .appendTo(ul);
                };
            }, true);

        },
        controllerAs: 'horseSearchController',
        link: function (scope, iElement, iAttrs) {

        }


    };
};

