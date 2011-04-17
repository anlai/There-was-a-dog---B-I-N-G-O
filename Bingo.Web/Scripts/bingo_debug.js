//=====================================
// Debug Functions
//=====================================
function enableDebug() {

    serverTime.setSeconds(serverTime.getSeconds() + 1);

    $("#serverTime").html(extractTime(serverTime));
    $("#clientTime").html(extractTime(new Date()));

    if ((serverTime.getSeconds() % 10) == 0) $("#calculatedPollTime").html(extractTime(serverTime));
}

function extractTime(dateTime) {

    var hour = dateTime.getHours();
    var minute = dateTime.getMinutes();
    var second = dateTime.getSeconds();

    return hour + ":" + minute + ":" + second;

}

function initializeDebug() {

    $("#timeDiff").html(calculateTimeDelta());
    $("#debug").show();
    setInterval(enableDebug, 1000);

}
