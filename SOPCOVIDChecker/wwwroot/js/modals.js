$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopup = (url, size) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form_modal .modal-dialog').addClass('modal-' + size);
            $('#form_modal .modal-content').html(res);
            $('#form_modal').modal('show');
            // to make popup draggable
            /*$('.modal-dialog').draggable({
                handle: ".modal-header"
            });*/
        },
        error: function (xhr, ajaxOptions, thrownError) {
            if (xhr.status === 401) {
                location.reload();
            }
            else {
                alert(xhr.responseText);
                alert(thrownError);
            }
        }
    })
}

partialViewGet = (form, return_container) => {
    try {
        $.ajax({
            type: 'GET',
            async: true,
            url: form.action,
            data: $(form).serialize(),
            contentType: false,
            processData: false,
            success: function (output) {
                $(return_container).html(output)
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxPost = (form, return_container) => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $(return_container).html(res.html)
                    $('#form_modal .modal-body').html('');
                    $('#form_modal .modal-title').html('');
                    $('#form_modal').modal('hide');
                }
                else {
                    $('#form_modal .modal-content').html(res.html);
                }
                if (res.toast !== '') {
                    toastr[res.type](res.toast);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}