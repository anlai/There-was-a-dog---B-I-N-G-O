//=====================================
// Game Variables
//=====================================
// user id
var userId = '@Model.UserId';
// game id
var gameId;
// indicator if game is over
var isgameOver = false;



//=====================================
// Public Functions
//=====================================

// reports bingo to the server and reports back
function reportBingo() {
    var selectedNumbers = [];
    $.each($(".selected"), function (index, item) { selectedNumbers.push(parseInt($(item).html())); });

    var data = { id: gameId, userId: userId, numbers: selectedNumbers };

    $.ajax({
        url: reportBingoUrl,
        data: JSON.stringify(data),
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            // result == true, game over
            isgameOver = result;
        }
    });
}

// load all balls for an inprogress game
function initialize() {

    // setup physics world
    initializePhysics();

    initilizeGame();

    initilizePolling();
}

// game over call
function gameOver() {
    // clear out the timer
    clearInterval(ballIntervalId);

    // show game over dialog
    $("#gameover_container").dialog("open");
}

//=====================================
// Private Functions
//=====================================

// get all initialization info for existing game
function initilizeGame() {
    // get existing game information
    $.get(initializeUrl, function (result) {

        if (result == undefined || result.nogame) {
            window.location = '@Url.Action("Index")';
        }

        var wait = 1000;

        $.each(result.balls, function (index, item) {

            setTimeout(function () { createBall(item.Letter, item.Number); }, index * wait);

        });

        gameId = result.gameId;
        messageId = result.messageId;
    });
}

// sync polling to server time
function initilizePolling() {
    setTimeout(setPolling, calculateTimeDelta());
}

// calculate time to wait to sync with server
function calculateTimeDelta() {
    var date = serverTime;

    // interval is 10 seconds, so wait till next 10 second interval
    var seconds = 10 - (date.getSeconds() % 10)

    // return seconds converted to milliseconds
    return seconds * 1000;
}

// setup polling
function setPolling() {
    // do initial call since we are lined up with the clock
    getNewBalls();

    // check for a checked in student every 5 minutes
    ballIntervalId = setInterval(getNewBalls, pollingInterval);
}