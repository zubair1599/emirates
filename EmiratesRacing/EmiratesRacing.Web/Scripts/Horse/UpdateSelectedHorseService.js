app.factory('UpdateSelectedHorse', Service);
function Service($rootscope) {
 
    var sharedService = {};

    sharedService.resultJSON = '';

    sharedService.prepForPublish = function (msg) {
        this.resultJSON = msg;
        this.publishItem();
    };

    sharedService.publishItem = function () {
        $rootScope.$broadcast('handlePublish');
    };

    return sharedService;

};