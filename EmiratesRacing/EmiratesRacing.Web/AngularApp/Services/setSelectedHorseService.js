app.factory('setSelectedHorse', ['$rootScope', '$http','$q', setSelectedHorse]);





function setSelectedHorse($rootScope, $http,$q) {

    var serviceDefer = new Object();
    serviceDefer.servicePromise = $q.defer();
    serviceDefer.message = '';
    serviceDefer.GetHorseJSON = function (id)
    {
        serviceDefer.servicePromise = $q.defer();
        serviceDefer.message = '';
        $http.get('https://stagebelweb.azurewebsites.net/api/horse/horsepedigree/216444').success(function (horseJson) {
            serviceDefer.message = horseJson;
            serviceDefer.servicePromise.resolve(horseJson);


        }).
          error(function (data, status, headers, config) {
              serviceDefer.servicePromise.reject();
          });

    };

    return serviceDefer;

};