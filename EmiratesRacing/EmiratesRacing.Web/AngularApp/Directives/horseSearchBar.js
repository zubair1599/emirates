
app.directive('horseSearchBar', ['setSelectedHorse', '$rootScope','$timeout', HorseSearchBarDirective]);



function HorseSearchBarDirective ($http, setSelectedHorse,$rootScope,$timeout) {
    return {

        controller: function ($scope, $element, setSelectedHorse) {
            $scope.selectedHorse = "";
            $scope.selectedHorseBreed = "";
            $scope.selectedHorseOwner = "";
            $scope.selectedHorseValue = "";
            $scope.resultJson = "S";
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
                        //setSelectedHorse.SetJSon($scope.selectedHorseValue);
                        setSelectedHorse.GetHorseJSON($scope.selectedHorseValue);
                        //var myPromise = setSelectedHorse.servicePromise.promise;
                        setSelectedHorse.servicePromise.promise.then(function (data) {
                            //$scope.resultJson = data;
                            $scope.mainC.selectedHorseJson = data;
                            $scope.mainC.horse =  ui.item.name;
                            $scope.mainC.horseBreed = ui.item.breed;
                            $scope.mainC.horseOwner = ui.item.owner;
                            $scope.mainC.horseHorseValue = ui.item.value;
                           
                            
                            //$timeout(function () {
                            //    $scope.$apply();
                            //});
                            
                            //$rootScope.$broadcast('UpdateSelectedHorseDetails');
                        }, function (data) {
                            alert("error : " + data);
                        });
                        

                        

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
        controllerAs: 'autoCompleteController',
        link: function (scope, iElement, iAttrs) {

        }


    };
};

