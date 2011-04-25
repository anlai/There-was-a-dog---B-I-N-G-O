var kkeys = [], konami = "38,38,40,40,37,39,37,39,66,65";
var mickey = "77,73,67,75,69,89"

var hasMickey = false;
var hasKonami = false;

$(document).keydown(bindEasterEgg);

function bindEasterEgg(e) {
    kkeys.push(e.keyCode);
    if (kkeys.toString().indexOf(konami) >= 0) {
        $(document).unbind('keydown', arguments.callee);

            // do seomthing

    }

    if (kkeys.toString().indexOf(mickey) >= 0) {
        $(document).unbind('keydown', arguments.callee);
        hasMickey = !hasMickey;
    }
}
