// id of last message seen
var messageId;

function initializeMessagePolling() {
    setInterval(messagePolling, 5000);
}

function messagePolling() {

    $.getJSON(messageUrl, { id: messageId }, function (result) {

        if (result.id != messageId) {

            messageId = result.id;

            $.each(result.messages, function (index, item) {

                var li = $("<li>");

                var name = item.User == null ? "System" : item.User.Name;
                var strong = $("<strong>").html(name);

                li.append(strong);
                li.append(item.Txt);

                $("#messages").prepend(li);
            });
        }
    });

}