$(document).ready(function () {
    $('#txtUploadFile').on('change', function (e) {
        var files = e.target.files;
        var myID = 3; //uncomment this to make sure the ajax URL works
        if (files.length > 0) {
            if (window.FormData !== undefined) {
           var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }

                $.ajax({
                    type: "POST",
                    url: '/Home/UploadFile?id=' + myID,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        $("#result").html(result);
                        //console.log(result);
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                    }
                });
            } else {
                alert("This browser doesn't support HTML5 file uploads!");
            }
        }
    });

    $('#adminform').submit(function (event) {
        event.preventDefault();

        SubmitForm($(this), this.action);
        return false;
    });


});

function SubmitForm(theform, action) {
    $.ajax({
        url: action,
        type: 'POST',
        data: theform.serialize(),
        success: function (result) {
            $("#submitsesult").html(result.msg);

            if (result.doRedirect) {
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];
                window.location.href = baseUrl + result.redirect;
            }
        },
        error: function (xhr, status, errorThrown) {
            alert("failed");
        }
    });
}

function Delete(id) {
    $.ajax({
        url: '/Home/Delete/' + id,
        type: 'POST',
        success: function (result) {
            $("#submitsesult").html(result.msg);

            if (result.doRedirect) {
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host; // + "/" + getUrl.pathname.split('/')[1];
                window.location.href = baseUrl + result.redirect;
            }
        },
        error: function (xhr, status, errorThrown) {
            alert("failed");
        }
    });

}


