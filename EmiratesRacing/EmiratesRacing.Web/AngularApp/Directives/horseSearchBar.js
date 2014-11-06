
app.directive('horseSearchBar', ['$http', 'setSelectedHorse', '$rootScope', HorseSearchBarDirective]);



function HorseSearchBarDirective ($http, setSelectedHorse,$rootScope) {
    return {

        controller: function ($scope, $element, $http) {
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
                        //setSelectedHorse.SetJSon($scope.selectedHorseValue);
                        setSelectedHorse.GetHorseJSON($scope.selectedHorseValue);
                        var promise = setSelectedHorse.promise;
                        promise.then(function (data) {

                            $rootScope.$broadcast('UpdateSelectedHorseDetails');
                        }, function (data) {
                            alert("error :" + data);
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

