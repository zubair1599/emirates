/// <reference path="E:\Respo.Shan\emirates\EmiratesRacing\EmiratesRacing.Web\Scripts/bootstrap.js" />
/// <reference path="E:\Respo.Shan\emirates\EmiratesRacing\EmiratesRacing.Web\Scripts/bootstrap.js" />
var page = require('webpage').create(),
    system = require('system'),
    address, output, size;

if (system.args.length < 3 || system.args.length > 5) {
    console.log('Usage: rasterize.js URL filename [paperwidth*paperheight|paperformat] [zoom]');
    console.log('  paper (pdf output) examples: "5in*7.5in", "10cm*20cm", "A4", "Letter"');
    phantom.exit(1);
} else {
    address = system.args[1];
    output = system.args[2];
    page.viewportSize = { width: 2000, height: 500 };
    if (system.args.length > 3 && system.args[2].substr(-4) === ".pdf") {
        size = system.args[3].split('*');
        page.paperSize = size.length === 2 ? { width: size[0], height: size[1], margin: '0px' }
                                           : { format: system.args[3], orientation: 'portrait', margin: '1cm' };
    }
    if (system.args.length > 4) {
        page.zoomFactor = system.args[4];
    }
    page.open(address, function (status) {
        if (status !== 'success') {
            console.log('Unable to load the address!');
            phantom.exit();
        } else {

            page.includeJs('http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js', function () {
                console.log('jQuery in');
                var response = page.evaluate(function () {
                    $(document).ready(function () {
                        //console.log('Document ready...');
                        //return;
                    });
                });
               
            });

            page.includeJs('../Scripts/bootstrap.js', function () {
                console.log('boot in');
               

            });


            window.setTimeout(function () {
                page.render(output);
                window.setTimeout(function () {
                    phantom.exit();
                },3000);
                
            }, 200); //time out to wait 
        }
    });
}
