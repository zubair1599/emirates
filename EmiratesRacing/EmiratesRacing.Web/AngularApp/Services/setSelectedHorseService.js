app.factory('setSelectedHorse', ['$rootScope', '$http','$q', setSelectedHorse]);





function setSelectedHorse($rootScope, $http,$q) {

    var serviceDefer = $q.defer();
    serviceDefer.message = '';
    serviceDefer.GetHorseJSON = function (ID)
    {
        $http.get('http://stagebelweb.azurewebsites.net/api/horse/horsepedigree/216444').success(function (horseJson) {
            serviceDefer.message = horseJson;
            serviceDefer.resolve(horseJson);


        }).
          error(function (data, status, headers, config) {
              serviceDefer.reject();
          });

    };

    return serviceDefer;

};