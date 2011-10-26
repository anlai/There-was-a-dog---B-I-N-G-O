function InitializeWaitingAttendancePolling() { setInterval(WaitingAttendancePolling, 5000); }
function InitializeGameAttendancePolling() { setInterval(GameAttendancePolling, 5000); }

function WaitingAttendancePolling() {

    $.getJSON(waitingUrl, {}, function (result) {

        $("#waiting-attendance table tbody").empty();

        $.each(result, function (index, item) {

            var row = createRow(item.Name, item.Update);
            $("#waiting-attendance table tbody").append(row);

        });

    });
    
}

function GameAttendancePolling() {

    $.getJSON(gameUrl, {}, function (result) {

        $("#game-attendance table tbody").empty();
        
        $.each(result, function (index, item) {

            var row = createRow(item.Name, item.Update);
            $("#game-attendance table tbody").append(row);

        });

    });
    
}

function createRow(name, update) {
    var row = $("<tr>");
    row.append($("<td>").html(name));
    row.append($("<td>").html(update));

    return row;
}