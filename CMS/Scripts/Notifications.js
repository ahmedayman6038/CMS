$(document).ready(function () {
    function messageNotification(notify) {
        GetMessagesData();
        FormatText();
        $('#messages').on('scroll', function () {
            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                GetMessagesData();
            }
        });
        $("#messagesDropdown").click(function () {
            $('#messageSpan').addClass('hideMessages');
            MarkMessagesSeen();
        });
        notify.client.sendMessageNotification = function (itemId, title, content, date) {
            $('#messageData').prepend('<div class="dropdown-divider"></div><a class="dropdown-item not-readed" href= "/Mails/Details/' + itemId + '">' +
                '<span class="text-success"><strong>' + title + '<span class="text-danger" id="newSpan"> (New) </span></strong></span><span class="small float-right text-muted">' + date + '</span>' +
                '<div class="dropdown-message small">' + content + '</div></a>');
            FormatText();
            $("#messageSpan").show();
            newMessages++;
            $("#messagesCounter").text(newMessages);
            $('#messageSpan').removeClass('hideMessages');
            DiscardM++;
        };
    }
    function alertNotification(notify) {
        GetAlertsData();
        FormatText();
        $('#alerts').on('scroll', function () {
            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                GetAlertsData();
            }
        });  
        $("#alertsDropdown").click(function () {
            $('#alertSpan').addClass('hideMessages');
            MarkAlertsSeen();
        });
        notify.client.sendAlertNotification = function (itemId, title, content, date) {
            $('#alertData').prepend('<div class="dropdown-divider"></div><a class="dropdown-item" href= "#">' +
                '<span class="text-success"><strong>' + title + '<span class="text-danger" id="newSpan"> (New) </span></strong></span><span class="small float-right text-muted">' + date + '</span>' +
                '<div class="dropdown-message small">' + content + '</div></a>');
            FormatText();
            $("#alertSpan").show();
            newAlerts++;
            $("#alertCounter").text(newAlerts);
            $('#alertSpan').removeClass('hideMessages');
            DiscardA++;
        };
    }
    function FormatText() {
        $("div.dropdown-message").text(function (index, currentText) {
            if (currentText.length >= 50) {
                return currentText.substr(0, 50) + '...';
            } else {
                return currentText;
            }
        });
    }
    function MarkMessagesSeen() {
        $.ajax({
            type: 'GET',
            url: '/Dashboard/SeenMessages',
            dataType: 'html',
            success: function (data) {

            },
            complete: function () {
                $('#messageSpan').addClass('hideMessages');
                newMessages = 0;
                $("#messagesCounter").text(newMessages);
            },
            error: function () {
                alert("an error occuered");
            }
        });
    }
    function MarkAlertsSeen() {
        $.ajax({
            type: 'GET',
            url: '/Dashboard/SeenAlerts',
            dataType: 'html',
            success: function (data) {

            },
            complete: function () {
                $('#alertSpan').addClass('hideMessages');
                newAlerts = 0;
                $("#alertCounter").text(newAlerts);
            },
            error: function () {
                alert("an error occuered");
            }
        });
    }
    function GetMessagesData() {
        if (!endMessages) {
            $.ajax({
                type: 'GET',
                url: '/Dashboard/GetMessages',
                data: { "pageindex": pageIndexM, "pagesize": pageSize, "discard": DiscardM },
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                var readed = "";
                                var seen = "";
                                if (data[i].WasRead == false) {
                                    readed = "not-readed";
                                }
                                if (data[i].WasSeen == false) {
                                    newMessages++;
                                } else {
                                    seen = "hideMessages";
                                }
                                $("#messageData").append('<div class="dropdown-divider"></div><a class="dropdown-item ' + readed + '" href= "/Mails/Details/' + data[i].ItemId + '">' +
                                    '<span class="text-success"><strong>' + data[i].Title + '<span class="text-danger ' + seen + '" id="newSpan"> (New) </span></strong></span><span class="small float-right text-muted">' + data[i].Date + '</span>' +
                                    '<div class="dropdown-message small">' + data[i].Content + '</div></a>');
                            }
                            pageIndexM++;
                        } else {
                            $("#endData").show();
                            endMessages = true;
                        }
                    }
                },
                beforeSend: function () {
                    $("#progress").show();
                    $("#endData").hide();
                },
                complete: function () {
                    $("#progress").hide();
                    FormatText();
                    $('#messageSpan').removeClass('hideMessages');
                    $("#messagesCounter").text(newMessages);
                    if ($("#messagesCounter").text() == 0) {
                        $('#messageSpan').addClass('hideMessages');
                    }
                },
                error: function () {
                    $("#error").show();
                }
            });
        }
    }
    function GetAlertsData() {
        if (!endAlerts) {
            $.ajax({
                type: 'GET',
                url: '/Dashboard/GetAlerts',
                data: { "pageindex": pageIndexA, "pagesize": pageSize, "discard": DiscardA },
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                var readed = "";
                                var seen = "";
                                if (data[i].WasSeen == false) {
                                    newAlerts++;
                                } else {
                                    seen = "hideMessages";
                                }
                                $("#alertData").append('<div class="dropdown-divider"></div><a class="dropdown-item" href= "#">' +
                                    '<span class="text-success"><strong>' + data[i].Title + '<span class="text-danger ' + seen + '" id="newSpan"> (New) </span></strong></span><span class="small float-right text-muted">' + data[i].Date + '</span>' +
                                    '<div class="dropdown-message small">' + data[i].Content + '</div></a>');
                            }
                            pageIndexA++;
                        } else {
                            $("#endData2").show();
                            endAlerts = true;
                        }
                    }
                },
                beforeSend: function () {
                    $("#progress2").show();
                    $("#endData2").hide();
                },
                complete: function () {
                    $("#progress2").hide();
                    FormatText();
                    $('#alertSpan').removeClass('hideMessages');
                    $("#alertCounter").text(newAlerts);
                    if ($("#alertCounter").text() == 0) {
                        $('#alertSpan').addClass('hideMessages');
                    }
                },
                error: function () {
                    $("#error2").show();
                }
            });
        }
    }
    // Calling Funnctions and Start hub Connections
    var pageSize = 5;

    var pageIndexM = 0;
    var DiscardM = 0;
    var notSeenM = 0;
    var newMessages = $("#messagesCounter").text();

    var pageIndexA = 0;
    var DiscardA = 0;
    var notSeenA = 0;
    var newAlerts = $("#alertCounter").text();

    var endMessages = false;
    var endAlerts = false;

    var notify = $.connection.notificationHub;
    messageNotification(notify);
    alertNotification(notify);
    $.connection.hub.start();
});